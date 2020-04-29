using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using CrystalQuartz.Application;
using CrystalQuartz.AspNetCore;
using CSharp.Scheduler.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;

namespace CSharp.Scheduler
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); }); });


            Task.Run(async () =>
            {
                var props = new NameValueCollection
                {
                    //settings
                    // [""]  = ""    
                };

                var schedulerFactory = new StdSchedulerFactory(props);
                
                var scheduler = await schedulerFactory.GetScheduler();

                var job = JobBuilder.Create<FirstJob>()
                    .WithIdentity("每隔两秒", "第一个").Build();

                var trigger = TriggerBuilder.Create()
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever())
                    .WithIdentity("第一个触发", "时间触发组")
                    .Build();

                var dateTime = await scheduler.ScheduleJob(job, trigger);

                await scheduler.Start();

                app.UseCrystalQuartz(() => scheduler, new CrystalQuartzOptions
                {
                    // Path = "" //设置前端链接，默认为localhost:port/{Path}
                });
            });
        }
    }
}