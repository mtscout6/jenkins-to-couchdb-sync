using Common.Logging;
using Quartz;
using Quartz.Spi;
using StructureMap;
using sabatoast_puller.Quartz.Jobs;
using FubuCore;

namespace sabatoast_puller.Quartz
{
    public class StructureMapSchedulerJobFactory : IJobFactory
    {
        private readonly IContainer _container;
        private readonly ILog _log;

        public StructureMapSchedulerJobFactory(IContainer container, ILog log)
        {
            _container = container;
            _log = log;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var rawJob = _container.GetInstance(bundle.JobDetail.JobType);

            var jobWithScheduler = rawJob as IJobWithScheduler;

            if (jobWithScheduler != null)
            {
                jobWithScheduler.Scheduler = scheduler;
                return jobWithScheduler;
            }

            var job = rawJob as IJob;

            if (job == null)
            {
                _log.Error("Job does not implement IJob '{0}'".ToFormat(bundle.JobDetail.JobType));
                return null;
            }

            return job;
        }

        /// <summary>
        /// Allows the the job factory to destroy/cleanup the job if needed.
        /// </summary>
        public void ReturnJob(IJob job)
        {
            // NO-OP
        }
    }
}