using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnapTalk.BLL.Interfaces;
using SnapTalk.BLL.Services;
using SnapTalk.Common.DTO;

namespace SnapTalk.WebAPI.Controllers;

[ApiController]
[Route("api/messages")]
[Authorize]
public class MessagesController(IMessageService messageService):ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendMessage([FromForm] SendMessageRequest request)
    {
        var response = await messageService.SendMessage(request);
        return Ok(response);
    }
    
    [HttpGet("{chatId}")]
    public async Task<IActionResult> GetMessages(Guid chatId)
    {
        var response = await messageService.GetMessages(chatId);
        return Ok(response);
    }
    
    [HttpDelete("{messageId}")]
    public async Task<IActionResult> DeleteMessage(Guid messageId)
    {
        var response = await messageService.DeleteMessage(messageId);
        return Ok(response);
    }
}