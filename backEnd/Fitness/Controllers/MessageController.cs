using Fitness.BLL;
using Fitness.BLL.Core;
using Fitness.DAL;
using Fitness.Models;
using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Message = Fitness.Models.Message;

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
        private readonly JWTHelper _jwthelper =new();
        [HttpGet]
        public ActionResult<List<Message>> GetMessages(string token)
        {
            int userID =_jwthelper.ValidateToken(token).userID;
            List<Message> messages = MessageDAL.GetMessages(userID);
            return messages;
        }

        [HttpGet]
        public ActionResult<string>  MarkedMessagesAsRead(List<int> messagesID)
        {
            try {
                MessageDAL.MarkMessagesAsRead(messagesID);
                return "标记成功";
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);
                return "标记失败";
            }
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
