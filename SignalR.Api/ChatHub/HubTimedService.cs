using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalR.Api.ChatHub
{
    /// <summary>
    /// SignalR定时任务
    /// </summary>
    public class HubTimedService : BackgroundService
    {
        private readonly ApiHub _apiHub;
        public HubTimedService(IConfiguration Configuration, ApiHub apiHub)
        {
            _apiHub = apiHub;
        }
        private Timer _timer;
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //初始化定时器
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }
        private void DoWork(object state)
        {
            //    Task.Delay(TimeSpan.FromSeconds(1)).Wait();        //等待1秒

            _apiHub.SendMessage("", "");
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return ExecuteAsync(cancellationToken);
        }

        public override void Dispose()
        {
            base.Dispose();
            _timer?.Dispose();
        }
    }
}
