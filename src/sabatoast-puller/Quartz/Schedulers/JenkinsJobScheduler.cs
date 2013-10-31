using Quartz;
using Quartz.Impl;
using sabatoast_puller.Quartz.Jobs;
using sabatoast_puller.Quartz.Triggers;

namespace sabatoast_puller.Quartz.Schedulers
{
    public class JenkinsJobScheduler : IJenkinsJobScheduler
    {
        private const string QuartzGroupKey = "job";
        private readonly IHalfMinuteTriggerBuilder _triggerBuilder;

        public JenkinsJobScheduler(IHalfMinuteTriggerBuilder triggerBuilder)
        {
            _triggerBuilder = triggerBuilder;
        }

        public string Group { get { return QuartzGroupKey; } }

        public void Schedule(IScheduler scheduler, string job, string url)
        {
            var jobDetail = new JobDetailImpl(job, Group, typeof (JenkinsJob));
            var trigger = _triggerBuilder.Build();

            scheduler.ScheduleJob(jobDetail, trigger);
            scheduler.TriggerJob(jobDetail.Key);
        }

        public JobKey KeyFor(string job)
        {
            return new JobKey(job, Group);
        }
    }
}