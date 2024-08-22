using FitnessWebAPI.BLL;
using FitnessWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Back_end_Web_API_chl.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageBLL _messageBLL;

        public MessageController(MessageBLL messageBLL)
        {
            _messageBLL = messageBLL;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            if (await _messageBLL.SendMessageAsync(message))
            {
                return Ok(new { success = true });
            }
            return BadRequest(new { success = false, message = "Failed to send message." });
        }

        [HttpGet]
        public IActionResult GetChatHistory(int userId)
        {
            var messages = _messageBLL.GetChatHistory(userId);
            if (messages != null && messages.Count > 0)
            {
                return Ok(messages);
            }
            return NotFound(new { success = false, message = "No messages found." });
        }
    }
}
