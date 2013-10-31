using Quartz;

namespace sabatoast_puller.Quartz.Jobs
{
    public interface IJobWithScheduler : IJob
    {
        IScheduler Scheduler { get; set; }
    }
}