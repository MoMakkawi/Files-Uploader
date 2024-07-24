using Application.Contracts;

using Microsoft.AspNetCore.Mvc;

using Application.Services;
using Application.Models.Attachments;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttachmentsController(
    IMapper mapper,
    IAttachmentServiceAsync attachmentServiceAsync,
    IAttachmentRepositoryAsync attachmentRepositoryAsync,
    IUserRepositoryAsync userRepositoryAsync) 
    : ControllerBase
{
    [HttpPost]
    [Route("save")]
    public async Task<IActionResult> SaveAsync([FromForm] List<SaveAttachmentCommand> saveAttachmentCommands)
    {
        var user = await userRepositoryAsync.GetLoginUserAsync(HttpContext);
        var attachments = await attachmentServiceAsync.SaveAsync(user, saveAttachmentCommands);
        var attachmentDTOs = mapper.Map<List<AttachmentDTO>>(attachments);
        return Ok(attachmentDTOs);
    }

    [HttpGet("base64")]
    public async Task<ActionResult<string>> GetAttachmentAsBase64Async(string uniqueName)
        => await attachmentServiceAsync.GetAsBase64Async(uniqueName) 
            is string attachmentAsBase64
            ? Ok(attachmentAsBase64)
            : BadRequest($"There no attachment have this name : {uniqueName}");

    [HttpGet("login-user")]
    public async Task<ActionResult<IEnumerable<AttachmentWithBase64DTO>>> GetLoginUserAttachmentsAsBase64Async()
    {
        var user = await userRepositoryAsync.GetLoginUserAsync(HttpContext);
        var attachmentsWithBase64 = mapper.Map<IEnumerable<AttachmentWithBase64DTO>>(user.Attachments.AsEnumerable());
        return Ok(attachmentsWithBase64);
    }

    [HttpGet("user-id")]
    public async Task<ActionResult<IEnumerable<AttachmentWithBase64DTO>>> GetAttachmentsWithBase64ByUserIdAsync(Guid userId)
    {
        var user = await userRepositoryAsync.GetByIdAsync(userId);
        if (user is null) return BadRequest($"User by Id : {userId} is not exist.");
        var attachmentsWithBase64 = mapper.Map<IEnumerable<AttachmentWithBase64DTO>>(user.Attachments.AsEnumerable());
        return Ok(attachmentsWithBase64);
    }

    [HttpGet("id")]
    public async Task<ActionResult<AttachmentWithBase64DTO>> GetAttachmentWithBase64ByIdAsync(Guid id)
    {
        var attachment = await attachmentRepositoryAsync.GetByIdAsync(id);
        if (attachment is null) return BadRequest($"attachment by Id : {id} is not exist.");
        var attachmentsWithBase64 = mapper.Map<AttachmentWithBase64DTO>(attachment);
        return Ok(attachmentsWithBase64);
    }

}
