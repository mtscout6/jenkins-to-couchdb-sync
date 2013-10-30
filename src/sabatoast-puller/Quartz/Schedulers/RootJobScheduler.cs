using Quartz;
using Quartz.Impl;
using sabatoast_puller.Quartz.Jobs;
using sabatoast_puller.Quartz.Triggers;

namespace sabatoast_puller.Quartz.Schedulers
{
    public class RootJobScheduler : IRootJobScheduler
    {
        private readonly IScheduler _scheduler;
        private readonly IHourlyTrigger _trigger;

        public RootJobScheduler(IScheduler scheduler, IHourlyTrigger trigger)
        {
            _scheduler = scheduler;
            _trigger = trigger;
        }

        public void Schedule()
        {
            var jobDetail = new JobDetailImpl(typeof(Root).Name, typeof(Root));
            _scheduler.ScheduleJob(jobDetail, _trigger);
            _scheduler.TriggerJob(jobDetail.Key);
        }
    }
}