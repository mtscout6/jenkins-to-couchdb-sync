using Quartz;
using Quartz.Impl;
using FubuCore;
using sabatoast_puller.Quartz.Jobs;
using sabatoast_puller.Quartz.Triggers;

namespace sabatoast_puller.Quartz.Schedulers
{
    public class JenkinsJobScheduler : IJenkinsJobScheduler
    {
        private readonly IScheduler _scheduler;
        private readonly IHalfMinuteTriggerBuilder _triggerBuilder;

        public JenkinsJobScheduler(IHalfMinuteTriggerBuilder triggerBuilder, IScheduler scheduler)
        {
            _scheduler = scheduler;
            _triggerBuilder = triggerBuilder;
        }

        public void Schedule(string job, string url)
        {
            var jobDetail = new JobDetailImpl("job/{0}".ToFormat(job), typeof (JenkinsJob));
            jobDetail.JobDataMap["job"] = job;
            jobDetail.JobDataMap["url"] = url;

            var trigger = _triggerBuilder.Build();

            _scheduler.ScheduleJob(jobDetail, trigger);
            _scheduler.TriggerJob(jobDetail.Key);
        }
    }
}