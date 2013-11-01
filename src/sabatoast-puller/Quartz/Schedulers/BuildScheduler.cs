using Common.Logging;
using FubuCore;
using Quartz;
using Quartz.Impl;
using sabatoast_puller.Quartz.Jobs;
using sabatoast_puller.Quartz.Triggers;

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
            var jobDetail = new JobDetailImpl(build.ToString(), group, typeof (JenkinsBuild));

            jobDetail.JobDataMap["job"] = job;
            jobDetail.JobDataMap["build"] = build;

            var trigger = _triggerBuilder.Build(group);

            scheduler.ScheduleJob(jobDetail, trigger);
            scheduler.TriggerJob(jobDetail.Key);
        }

        public void Remove(IScheduler scheduler, string job, int build)
        {
            _log.Warn("NOT IMPLEMENTED REMOVE BUILD");
        }

        public void RemoveAll(IScheduler scheduler, string job)
        {
            _log.Warn("NOT IMPLEMENTED REMOVE ALL BUILDS");
        }
    }
}