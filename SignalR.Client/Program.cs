using System;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalR.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // 依赖包：Microsoft.AspNetCore.SignalR.Client

            //http://www.80iter.com/blog/1446511389621678
            //官网
            //https://docs.microsoft.com/zh-cn/aspnet/core/signalr/dotnet-client?view=aspnetcore-2.2
            //https://docs.microsoft.com/zh-cn/aspnet/signalr/overview/guide-to-the-api/hubs-api-guide-net-client

            HubConnection connection = new HubConnectionBuilder()
               .WithUrl("http://localhost:5002/api/chatHub")
               .Build();
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var newMessage = $"{user}: {message}";
                Console.WriteLine(newMessage);
            });
            connection.StartAsync();

            connection.InvokeAsync("SendMessage", "client", "连接成功");
            //connection.Closed += (error) => {
            //    // Do your close logic.
            //    return Task.CompletedTask;
            //};

            //在 3.0 之前，SignalR.NET 客户端不会自动重新连接。 必须编写代码将手动重新连接你的客户端。
            connection.Closed += async (error) =>
            {
                //等待时间
                await Task.Delay(new Random().Next(0, 5) * 1000);
                //重新连接
                await connection.StartAsync();
            };

            Console.ReadLine();
        }
    }
}
