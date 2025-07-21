using bnbClone_API.DTOs.MessagesDTOs;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static bnbClone_API.DTOs.MessagesDTOs.SendMessageDTO;


namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService messageService;
        public MessageController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        [HttpGet("{conversationId}")]
        public async Task<IActionResult> GetAllByConversation(int conversationId)
        {
            var messages = await messageService.GetMessagesByConversationIdAsync(conversationId);
            return Ok(messages);
        }
        [HttpGet("details/{messageId}")]
        public async Task<IActionResult> GetMessageByIdAsync(int messageId)
        {
            var message = await messageService.GetMessageByIdAsync(messageId);
            return message is null ? NotFound() : Ok(message);
        }
        [HttpGet("latest/{conversationId}")]
        public async Task<IActionResult> GetLatestMessageAsync(int conversationId)
        {
            var message= await messageService.GetLatestMessageAsync(conversationId);
            return message is null ? NotFound() : Ok(message);
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDTO SMDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var success = await messageService.SendMessageAsync(SMDTO);
            if (!success)
            {
                return StatusCode(500, "Failed to send message");
            }
            return Ok("Message sent Successfully");
        }
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkMessageAsReadAsync(int messageId)
        {
            var success= await messageService.MarkMessageAsReadAsync(messageId);
            if (!success)
            {
                return NotFound("Message Not Found");
            }
            return Ok("Message marked as read");
        }
    }
}