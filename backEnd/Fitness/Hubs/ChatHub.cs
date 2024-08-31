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
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }
        public async Task SendMessage(Message message)
        {
            try
            {
                Console.WriteLine(message);
                await Clients.All.SendAsync("ReceiveMessage", message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message to friend");
                throw; // 抛出异常以确保客户端知道有问题
            }
            /*  MessageBLL messageBLL = new MessageBLL();
                messageBLL.InsertMessage(message);*/
        }

    }
}
