using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Fitness.Models
{
    public class ChatHub : Hub
    {

        // 当客户端连接时触发
        public override Task OnConnectedAsync()
        {
            // 这里可以添加连接时的处理逻辑，比如将用户添加到某个组
            return base.OnConnectedAsync();
        }

        // 当客户端断开连接时触发
        public override Task OnDisconnectedAsync(Exception exception)
        {
            // 这里可以添加断开连接时的处理逻辑，比如将用户从组中移除
            return base.OnDisconnectedAsync(exception);
        }

    }
}
