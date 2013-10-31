using Quartz;

namespace sabatoast_puller.Quartz.Schedulers
{
    public interface IBuildScheduler
    {
        string Group(string job);
        void Schedule(IScheduler scheduler, string job, int build);
        void Remove(IScheduler scheduler, string job, int build);
        void RemoveAll(IScheduler scheduler, string job);
    }
}