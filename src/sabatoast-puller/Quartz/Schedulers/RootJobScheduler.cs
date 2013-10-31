using Quartz;
using Quartz.Impl;
using sabatoast_puller.Quartz.Jobs;
using sabatoast_puller.Quartz.Triggers;

namespace sabatoast_puller.Quartz.Schedulers
{
    public class RootJobScheduler : IRootJobScheduler
    {
        private readonly IHourlyTrigger _trigger;

        public RootJobScheduler(IHourlyTrigger trigger)
        {
            _trigger = trigger;
        }

        public void Schedule(IScheduler scheduler)
        {
            var jobDetail = new JobDetailImpl(typeof(Root).Name, typeof(Root));
            scheduler.ScheduleJob(jobDetail, _trigger);
            scheduler.TriggerJob(jobDetail.Key);
        }
    }
}