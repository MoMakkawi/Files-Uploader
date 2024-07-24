using Application.Contracts;
using Application.Models.Identities;
using Application.Models.Security;
using Application.Services;

using AutoMapper;
using Domain.Identities;
using Domain.Security;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(
    IAuthenticationServiceAsync authenticationServiceAsync,
    IUserRepositoryAsync userRepositoryAsync,
    IAttachmentServiceAsync attachmentServiceAsync,
    IMapper mapper) : ControllerBase
{
    [HttpPost("auth")]
    public async Task<ActionResult<AuthenticationResponse>> AuthenticateUser(AuthenticationRequest request)
    {
        // some code for verifying if the user is valid (e.g: exist in DB)
        var user = await userRepositoryAsync.GetByUserNameAsync(request.UserName);
        if (user is null) return BadRequest("User is not exist.");
        if (user.Password != request.Password) return BadRequest("Username or password wrong");

        // If user valid
        var accessTokenData = authenticationServiceAsync.GenerateToken(request.UserName);
        var refreshTokenData = authenticationServiceAsync.GenerateRefreshToken(accessTokenData.Token);

        var authenticationResponseDTO = new AuthenticationResponse
        {
            AccessTokenData = accessTokenData,
            RefreshTokenData = refreshTokenData
        };

        if (!user.HasFirstLogin)
        {
            user.HasFirstLogin = true;
            await userRepositoryAsync.UpdateAsync(user);
        }

        return Ok(authenticationResponseDTO);
    }

    [HttpPost("refresh")]
    public ActionResult<AuthenticationResponse> Refresh([FromBody] RefreshTokenRequest request)
    {
        // Validate refresh token
        if (!authenticationServiceAsync.TryToGetValidRefreshTokenData(request, out TokenData RefreshTokenData))
            return Unauthorized();

        // get username from access token
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(request.AccessToken);
        string userName = jwtToken.Claims.First(x => x.Type == "nameid").Value;

        var newAccessTokenData = authenticationServiceAsync.GenerateToken(userName);
        var newRefreshTokenData = authenticationServiceAsync.GenerateRefreshToken(request.AccessToken);

        var refreshResponseDTO = new AuthenticationResponse
        {
            AccessTokenData = newAccessTokenData,
            RefreshTokenData = newRefreshTokenData
        };

        return Ok(refreshResponseDTO);
    }

    [HttpPost("Register")]
    public async Task<ActionResult<UserDTO>> AddUser([FromForm] RegisterCommand registerCommand)
    {
        var existUser = await userRepositoryAsync.GetByUserNameAsync(registerCommand.UserName);
        if (existUser is not null) return BadRequest($"There an user have this username : {registerCommand.UserName}");

        var user = mapper.Map<User>(registerCommand);
        var addedUser = await userRepositoryAsync.CreateAsync(user);

        await attachmentServiceAsync.SaveAsync(addedUser, registerCommand.Attachments);

        return Ok(mapper.Map<UserDTO>(addedUser));
    }

}
