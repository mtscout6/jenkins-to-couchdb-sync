using Quartz;

namespace sabatoast_puller.Quartz.Schedulers
{
    public interface IRootJobScheduler
    {
        void Schedule(IScheduler scheduler);
    }
}