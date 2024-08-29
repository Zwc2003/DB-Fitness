using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness.BLL;
using Microsoft.AspNetCore.SignalR;

namespace Fitness.Models
{
    public class ChatHub : Hub
    {
        public async Task SendMessageToFriend(Message message)
        {
            var receiverConnectionId = message.receiverID.ToString();
            await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", message); 
            MessageBLL messageBLL = new MessageBLL();
            messageBLL.InsertMessage(message);
        }

    }
}
