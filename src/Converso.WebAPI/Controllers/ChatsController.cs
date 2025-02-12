using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnapTalk.BLL.Interfaces;
using SnapTalk.Common.DTO;

namespace SnapTalk.WebAPI.Controllers;

[Authorize]
[Route("api/chats")]
[ApiController]
public class ChatsController(IChatsService chatsService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatRequest request)
    {
        var response = await chatsService.CreateChat(request);

        return Ok(response);
    }
    
    [HttpPost("{chatId}")]
    public async Task<IActionResult> JoinChat([FromRoute] JoinChatRequest request)
    {
        var response = await chatsService.JoinChat(request);

        return Ok(response);
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchChats([FromQuery] string title)
    {
        var response = await chatsService.SearchChats(title);

        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetChats()
    {
        var response = await chatsService.GetChats();

        return Ok(response);
    }
    
    [HttpGet("{chatId}")]
    public async Task<IActionResult> GetChat([FromRoute] Guid chatId)
    {
        var response = await chatsService.GetChat(chatId);

        return Ok(response);
    }
    
    [HttpDelete("{chatId}")]
    public async Task<IActionResult> DeleteChat([FromRoute] Guid chatId)
    {
        var response = await chatsService.DeleteChat(chatId);

        return Ok(response);
    }
    
    [HttpDelete("{chatId}/userChat")]
    public async Task<IActionResult> LeaveChat([FromRoute] Guid chatId)
    {
        var response = await chatsService.LeaveChat(chatId);

        return Ok(response);
    }
}