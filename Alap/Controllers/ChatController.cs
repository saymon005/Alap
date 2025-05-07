using Alap.Data;
using Alap.DTOs;
using Alap.Models;
using Alap.Repositories;
using Alap.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alap.Controllers
{
    [Route("api/chat")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TavilyService _tavily;
        private readonly UserManager<IdentityUser> _userManager;

        public ChatController(IUnitOfWork unitOfWork, TavilyService tavily,
            UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _tavily = tavily;
            _userManager = userManager;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var trimmedMessage = request.Message?.Trim();
            if (string.IsNullOrWhiteSpace(trimmedMessage))
                return BadRequest("Message cannot be empty or whitespace.");

            var userMessage = new ChatMessage
            {
                UserId = user.Id,
                Sender = user.UserName,
                Message = trimmedMessage
            };

            await _unitOfWork.ChatMessages.AddAsync(userMessage);
            await _unitOfWork.SaveChangesAsync();

            var botReply = await _tavily.GetBotResponse(request.Message);
            var botMessage = new ChatMessage
            {
                UserId = user.Id,
                Sender = "Bot",
                Message = botReply
            };

            await _unitOfWork.ChatMessages.AddAsync(botMessage);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new
            {
                userMessage = new { userMessage.Sender, userMessage.Message },
                botMessage = new { botMessage.Sender, botMessage.Message }
            });
        }


        [HttpGet("history")]
        public async Task<IActionResult> GetHistory(int page = 1, int pageSize = 20)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var messages = await _unitOfWork.ChatMessages.GetUserMessagesAsync(user.Id, page, pageSize);
            return Ok(messages);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditMessage(int id, [FromBody] EditMessageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var message = await _unitOfWork.ChatMessages.GetByIdAsync(id);
            if (message == null || message.IsDeleted) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null || message.UserId != user.Id) return Forbid();

            var trimmedMessage = request.Message?.Trim();
            if (string.IsNullOrWhiteSpace(trimmedMessage))
                return BadRequest("Message cannot be empty or whitespace.");

            message.Message = trimmedMessage;
            await _unitOfWork.SaveChangesAsync();

            return Ok(message);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _unitOfWork.ChatMessages.GetByIdAsync(id);
            if (message == null || message.IsDeleted) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (message.UserId != user.Id) return Forbid();

            message.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveMessage(int id)
        {
            var message = await _unitOfWork.ChatMessages.GetByIdAsync(id);
            if (message == null || message.IsDeleted) return NotFound();

            message.IsApproved = true;
            await _unitOfWork.SaveChangesAsync();

            return Ok(message);
        }
    }

}
