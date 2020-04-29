using System;
using System.Threading.Tasks;
using CSharp.Template.Scheduler.Core;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;

namespace CSharp.Template.Scheduler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            
            var job = JobBuilder.Create<FirstJob>().Build();
            
            TriggerBuilder.Create().WithCronSchedule("").Build();
            
            var trigger = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever()).Build();

            await scheduler.ScheduleJob(job, trigger);
            
            await scheduler.Start();
            
            Console.ReadKey();
        }
    }
}