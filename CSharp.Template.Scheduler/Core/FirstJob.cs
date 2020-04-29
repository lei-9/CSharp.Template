using System;
using System.Globalization;
using System.Threading.Tasks;
using Quartz;

namespace CSharp.Template.Scheduler.Core
{
    public class FirstJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"now : {DateTime.Now.ToString(provider: CultureInfo.InvariantCulture)}");

            return Task.CompletedTask;
        }
    }
}