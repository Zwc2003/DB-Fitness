using Fitness.BLL;
using Fitness.DAL;
using Fitness.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        [HttpGet]
        public async Task Subscribe(int userID)
        {
            HttpContext.Response.Headers.Add("Content-Type", "text/event-stream");

            int userId = userID; 

            while (true)
            {
                var messages = GetUnreadMessages(userId);
                if (messages != null && messages.Count > 0)
                {
                    string jsonMessages = JsonConvert.SerializeObject(messages);
                    await HttpContext.Response.WriteAsync($"data: {jsonMessages}\n\n");
                    await HttpContext.Response.Body.FlushAsync();

                    // 标记这些消息为已读
                    var messageIds = messages.Select(m => m.messageID).ToList();
                    MessageDAL.MarkMessagesAsRead(messageIds);
                }

                await Task.Delay(1000); // 检查新消息的间隔时间
            }
        }

        private List<Message> GetUnreadMessages(int userId)
        {
            return MessageDAL.GetUnreadMessages(userId);
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
