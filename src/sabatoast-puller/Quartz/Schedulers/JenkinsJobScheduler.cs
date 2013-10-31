using Quartz;
using Quartz.Impl;
using FubuCore;
using sabatoast_puller.Quartz.Jobs;
using sabatoast_puller.Quartz.Triggers;

namespace sabatoast_puller.Quartz.Schedulers
{
    public class JenkinsJobScheduler : IJenkinsJobScheduler
    {
        private readonly IHalfMinuteTriggerBuilder _triggerBuilder;

        public JenkinsJobScheduler(IHalfMinuteTriggerBuilder triggerBuilder)
        {
            _triggerBuilder = triggerBuilder;
        }

        public void Schedule(IScheduler scheduler, string job, string url)
        {
            var jobDetail = new JobDetailImpl("job/{0}".ToFormat(job), typeof (JenkinsJob));
            jobDetail.JobDataMap["job"] = job;
            jobDetail.JobDataMap["url"] = url;

            var trigger = _triggerBuilder.Build();

            scheduler.ScheduleJob(jobDetail, trigger);
            scheduler.TriggerJob(jobDetail.Key);
        }
    }
}