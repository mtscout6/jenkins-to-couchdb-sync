using System.Collections.Generic;
using FubuCore;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using sabatoast_puller.Quartz.Jobs;
using sabatoast_puller.Quartz.Triggers;

namespace sabatoast_puller.Quartz.Schedulers
{
    public class JenkinsJobScheduler : IJenkinsJobScheduler
    {
        private const string QuartzGroupKey = "job";
        private readonly IHalfMinuteTriggerBuilder _triggerBuilder;
        private readonly IBuildScheduler _buildScheduler;

        public JenkinsJobScheduler(IHalfMinuteTriggerBuilder triggerBuilder, IBuildScheduler buildScheduler)
        {
            _triggerBuilder = triggerBuilder;
            _buildScheduler = buildScheduler;
        }

        public string Group { get { return QuartzGroupKey; } }

        public void Schedule(IScheduler scheduler, string job, string url)
        {
            var jobDetail = new JobDetailImpl(job, Group, typeof (JenkinsJob));
            var triggerGroup = TriggerGroup(job);
            var trigger = _triggerBuilder.Build(triggerGroup);

            scheduler.ScheduleJob(jobDetail, trigger);
            scheduler.TriggerJob(jobDetail.Key);
        }

        public void Remove(IScheduler scheduler, string job)
        {
            _buildScheduler.RemoveAll(scheduler, job);

            scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals(TriggerGroup(job)))
                .Each(trigger => scheduler.UnscheduleJob(trigger));

            scheduler.DeleteJob(KeyFor(job));
        }

        public JobKey KeyFor(string job)
        {
            return new JobKey(job, Group);
        }

        public string TriggerGroup(string job)
        {
            return "{0}::{1}".ToFormat(Group, job);
        }
    }
}