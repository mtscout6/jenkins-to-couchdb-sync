using Quartz;

namespace sabatoast_puller.Quartz.Schedulers
{
    public interface IJenkinsJobScheduler
    {
        void Schedule(IScheduler scheduler, string job, string url);
    }
}