using Common.Logging;
using FubuCore;
using Quartz;
using Quartz.Impl;
using sabatoast_puller.Quartz.Jobs;
using sabatoast_puller.Quartz.Triggers;
using System.Collections.Generic;

namespace sabatoast_puller.Quartz.Schedulers
{
    public class BuildScheduler : IBuildScheduler
    {
        private readonly IHalfMinuteTriggerBuilder _triggerBuilder;
        private readonly ILog _log;

        public BuildScheduler(IHalfMinuteTriggerBuilder triggerBuilder, ILog log)
        {
            _triggerBuilder = triggerBuilder;
            _log = log;
        }

        public string Group(string job)
        {
            return "{0}::builds".ToFormat(job);
        }

        public void Schedule(IScheduler scheduler, string job, int build)
        {
            var group = Group(job);

            if (scheduler.GetJobDetail(JobKey(job, build)) != null)
                return;

            var jobDetail = new JobDetailImpl(build.ToString(), group, typeof (JenkinsBuild));

            jobDetail.JobDataMap["job"] = job;
            jobDetail.JobDataMap["build"] = build;

            var trigger = _triggerBuilder.Build(group);

            scheduler.ScheduleJob(jobDetail, trigger);
            scheduler.TriggerJob(jobDetail.Key);
        }

        public void Remove(IScheduler scheduler, string job, int build)
        {
            var jobKey = JobKey(job, build);

            scheduler.GetTriggersOfJob(jobKey).Each(t => scheduler.UnscheduleJob(t.Key));
            scheduler.DeleteJob(jobKey);
        }

        public void RemoveAll(IScheduler scheduler, string job)
        {
            // TODO: Implement this
            _log.Warn("NOT IMPLEMENTED REMOVE ALL BUILDS");
        }

        JobKey JobKey(string job, int build)
        {
            return new JobKey(build.ToString(), Group(job));
        }
    }
}