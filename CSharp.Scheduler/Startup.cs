using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CrystalQuartz.Application;
using CrystalQuartz.AspNetCore;
using CSharp.Scheduler.Core;
using CSharp.Scheduler.Model;
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

            //get jobs by settings
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var settings = config.GetSection("QuartzSettings").Get<List<QuartzSetting>>();

            var props = new NameValueCollection
            {
                //settings
                // [""]  = ""    
            };

            var schedulerFactory = new StdSchedulerFactory();

            var scheduler = schedulerFactory.GetScheduler().Result;

            if (settings?.Any() ?? false)
            {
                var jobServiceAssembly = Assembly.Load("CSharp.Scheduler");
               
                foreach (var setting in settings)
                {
                    var curJob = jobServiceAssembly.CreateInstance(setting.JobFullPath);
                    if (curJob == default) throw new DllNotFoundException($"配置的Job - {setting.JobName}不存在！");
                    var job = JobBuilder.Create(curJob.GetType())
                        .WithIdentity(setting.JobName ?? setting.JobFullPath.Split(".").Last())
                        .Build();

                    var trigger = TriggerBuilder.Create()
                        .WithCronSchedule(setting.Cron)
                        .Build();

                    var dateTime = scheduler.ScheduleJob(job, trigger).Result;
                }

                scheduler.Start().GetAwaiter().GetResult();
            }

            app.UseCrystalQuartz(() => scheduler);
        }
    }
}