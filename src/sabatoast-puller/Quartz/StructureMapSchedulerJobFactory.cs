using Quartz;
using Quartz.Spi;
using StructureMap;

namespace sabatoast_puller.Quartz
{
    public class StructureMapSchedulerJobFactory : IJobFactory
    {
        private readonly IContainer _container;

        public StructureMapSchedulerJobFactory(IContainer container)
        {
            _container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return (IJob) _container.GetInstance(bundle.JobDetail.JobType);
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