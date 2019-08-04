using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalR.Api.ChatHub;

namespace SignalR.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region CORS跨域
            services.AddCors(opt =>
            {
                #region 指定固定源跨域
                //var site = Configuration["Origins"] ?? ""; //从配置文件读取
                //string[] Origins = site.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                ////一般采用这种方法
                //opt.AddPolicy("part", policy =>
                //{
                //    policy
                //    //支持多个域名端口，注意端口号后不要带/斜杆：比如localhost:8000/，是错的
                //    .WithOrigins(Origins)
                //    .AllowAnyHeader()
                //    .AllowAnyMethod()
                //    .AllowCredentials();
                //});
                opt.AddPolicy("part2", set =>
                {
                    set.SetIsOriginAllowed(origin => true)
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
                });
                opt.AddPolicy("Signal", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(new[] { "http://localhost:5001" }));

                #endregion

            });
            #endregion
            services.AddSingleton<ApiHub>();
            services.AddSignalR();
            //services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, HubTimedService>();
            //services.AddHostedService<HubTimedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("Signal");
            app.UseHttpsRedirection();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ApiHub>("/api/chatHub");

            });
            app.UseMvc();

            
        }
    }
}
