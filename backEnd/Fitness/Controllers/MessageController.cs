using Fitness.BLL;
using Fitness.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.Controllers
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
            return BadRequest(new { success = false, message = "发送消息失败" });
        }

        [HttpGet]
        public ActionResult<List<Message>> GetChatHistory(int userId)
        {
            var messages = _messageBLL.GetChatHistory(userId);
            if (messages != null && messages.Count > 0)
            {
                return Ok(messages);
            }
            return NotFound(new { success = false, message = "无聊天记录" });
        }
    }
}
