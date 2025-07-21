using bnbClone_API.DTOs.ConversationsDTOs;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService conversationService;
        public ConversationController(IConversationService conversationService)
        {
            this.conversationService = conversationService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdWithUsersAndPropertyAsync(int id) 
        {
            var conversation = await conversationService.GetByIdWithUsersAndPropertyAsync(id);
            if (conversation == null)
                return NotFound("Conversation not found");

            return Ok(conversation);
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserConversationsAsync(int userId)
        {
            var conversations= await conversationService.GetUserConversationsAsync(userId);
            return Ok(conversations);
        }
        [HttpPost("start")]
        public async Task<IActionResult> StartConversation([FromBody] StartConversationDTO start)
        {
            var conversationId = await conversationService.StartConversationAsync(start);
            if (conversationId == null) return StatusCode(500,"Failed to start Conversation");
            return Ok(new { ConversationId = conversationId });
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateConversation([FromBody] UpdateConversationDTO update)
        {
            var updated = await conversationService.UpdateConversationAsync(update);
            if(!updated) return NotFound("Failed to update Conversation");
            return Ok(updated);
        }
    }
}
