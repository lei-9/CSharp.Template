using System;
using System.Globalization;
using System.Threading.Tasks;
using Quartz;

namespace CSharp.Scheduler.Core
{
    public class FirstJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"now : {DateTime.Now.ToString(CultureInfo.InvariantCulture)}");
            
            return Task.CompletedTask;
        }
    }
}