using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using StructureMap;
using FubuCore;

namespace sabatoast_puller.Quartz.Triggers
{
    public interface IHourlyTrigger : ITrigger { }

    public class HourlyTrigger : CronTriggerImpl, IHourlyTrigger
    {
        public HourlyTrigger() : this(0) { }

        public HourlyTrigger(int onMinute) : base("Hourly Trigger")
        {
            CronExpressionString = "0 {0} * * * ?".ToFormat(onMinute);
        }
    }

    public static class HourlyTriggerExtensions
    {
        public static IJobDetail ScheduleHourlyAndTrigger<TJob>(this IScheduler scheduler, IContainer container)
        {
            var jobDetail = scheduler.ScheduleHourly<TJob>(container);
            scheduler.TriggerJob(jobDetail.Key);
            return jobDetail;
        }

        public static IJobDetail ScheduleHourly<TJob>(this IScheduler scheduler, IContainer container)
        {
            var jobDetail = new JobDetailImpl(typeof(TJob).Name, typeof(TJob));
            var trigger = container.GetInstance<IHourlyTrigger>();

            scheduler.ScheduleJob(jobDetail, trigger);
            return jobDetail;
        }
    }
}