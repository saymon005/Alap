using Alap.Data;
using Alap.DTOs;
using Alap.Models;
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
        private readonly ApplicationDbContext _context;
        private readonly TavilyService _tavily;
        private readonly UserManager<IdentityUser> _userManager;

        public ChatController(ApplicationDbContext context, TavilyService tavily, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _tavily = tavily;
            _userManager = userManager;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var userMessage = new ChatMessage
            {
                UserId = user.Id,
                Sender = user.UserName,
                Message = request.Message
            };

            _context.ChatMessages.Add(userMessage);
            await _context.SaveChangesAsync();

            var botReply = await _tavily.GetBotResponse(request.Message);

            var botMessage = new ChatMessage
            {
                UserId = user.Id,
                Sender = "Bot",
                Message = botReply
            };

            _context.ChatMessages.Add(botMessage);
            await _context.SaveChangesAsync();

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

            var messages = await _context.ChatMessages
                .Where(m => m.UserId == user.Id && !m.IsDeleted)
                .OrderByDescending(m => m.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(messages);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditMessage(int id, [FromBody] EditMessageRequest request)
        {
            var message = await _context.ChatMessages.FindAsync(id);
            if (message == null || message.IsDeleted)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (message.UserId != user.Id)
                return Forbid();

            message.Message = request.Message;
            await _context.SaveChangesAsync();

            return Ok(message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.ChatMessages.FindAsync(id);
            if (message == null || message.IsDeleted)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (message.UserId != user.Id)
                return Forbid();

            message.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveMessage(int id)
        {
            var message = await _context.ChatMessages.FindAsync(id);
            if (message == null || message.IsDeleted)
                return NotFound();

            message.IsApproved = true;
            await _context.SaveChangesAsync();

            return Ok(message);
        }
    }
}
