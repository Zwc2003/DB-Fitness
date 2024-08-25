using Fitness.DAL;
using Fitness.Models;
using Microsoft.AspNetCore.SignalR;
using Fitness.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.BLL
{
    public class MessageBLL:IMessageBLL
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageBLL(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<bool> SendMessageAsync(Message message)
        {
            bool result = MessageDAL.Insert(message);
            if (result)
            {
                // 通知接收方有新消息
                await _hubContext.Clients.User(message.receiverID.ToString()).SendAsync("ReceiveMessage", message);
                return true;
            }
            return false;
        }

        public List<Message> GetChatHistory(int userId)
        {

            List<Message> messages = MessageDAL.GetMessageByUserID(userId);

            return messages;
        }

    }
}
