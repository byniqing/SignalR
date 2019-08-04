using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Api.ChatHub
{
    /// <summary>
    /// 集线器基类
    /// </summary>
    /// 
    //[HubName("stockTicker")]
    //[HubMethodName("stockTicker")]
    public class ApiHub : Hub
    {

        /// <summary>
        /// 客户端建立连接
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            //NLogHelp.Info($"用户ConnectionId是：{Context.ConnectionId}，连接到服务器。");
            //string userId = Context.QueryString["userId"];
            //Context.User.Claims
            //GetMessage();

            //Groups.AddToGroupAsync("", "");
            // 业务代码处理
            return base.OnConnectedAsync();
        }
        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        /// <summary>
        /// 前端调用
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        //[HubMethodName("send")] //自定义方法,前端调用的方法名
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task GetMessage()
        {
            var id = Context.ConnectionId;
            await Clients.All.SendAsync("ReceiveMessage", "有人上线", "服务端Message" + id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetUserId()
        {
            //把用户id和ConnectionId绑定，用户主动推送数据
            var id = Context.ConnectionId;
            await Clients.All.SendAsync("ReceiveMessage", "服务端user", "服务端Message" + id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task PushMessageAsync(object data)
        {
            var result = new
            {
                code = 2000,
                msg = "success",
                body = new
                {
                    name = "zhagns",
                    age = 20
                }
            };
            var json = JsonConvert.SerializeObject(result);

            await Clients.Client("").SendAsync("ReceiveMessage", json);
        }

        //定于一个通讯管道，用来管理我们和客户端的连接
        public async Task GetLatestCount(string random)
        {
            await Clients.All.SendAsync("ReceiveUpdate", "来自服务端的消息");
        }
    }
}
