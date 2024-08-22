using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace FitnessWebAPI.Models
{
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            // 可以根据需求执行一些初始化操作，比如加载未读消息等
            await base.OnConnectedAsync();
        }

        public async Task SendMessageToUser(int receiverId, string message)
        {
            var senderId = Context.UserIdentifier;

            // 将消息发送给指定的客户端
            await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", new
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = message,
                Timestamp = DateTime.UtcNow
            });
        }
    }

}
