namespace CSharp.Scheduler.Model
{
    public class QuartzSetting
    {
        // /// <summary>
        // /// job管理名字
        // /// </summary>
        // public string SchedulerName { get; set; }
        /// <summary>
        /// 要执行job的命名空间 + 类名
        /// </summary>
        public string JobFullPath { get; set; }
        /// <summary>
        /// 时间规则表达式
        /// </summary>
        public string Cron { get; set; }
        /// <summary>
        /// 定时任务名字
        /// </summary>
        public string JobName { get; set; }
        // /// <summary>
        // /// 定时任务分组
        // /// </summary>
        // public string JobGroup { get; set; }
        // /// <summary>
        // /// 时间规则名字
        // /// </summary>
        // public string CronName { get; set; }
        // /// <summary>
        // /// 时间规则分组
        // /// </summary>
        // public string CronGroup { get; set; }
    }
}