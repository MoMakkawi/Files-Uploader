using Application.Contracts;

using Microsoft.AspNetCore.Mvc;

using Application.Services;
using Application.Models.Attachments;
using AutoMapper;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttachmentsController(
    IMapper mapper,
    IAttachmentServiceAsync attachmentServiceAsync,
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
    public async Task<ActionResult<string>> GetImageAsBase64Async(string uniqueName)
        => await attachmentServiceAsync.GetAsBase64Async(uniqueName) 
            is string imageAsBase64
            ? Ok(imageAsBase64)
            : BadRequest($"There no attachment have this name : {uniqueName}");

}
