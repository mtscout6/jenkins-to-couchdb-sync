using Quartz;

namespace sabatoast_puller.Quartz.Schedulers
{
    public interface IJenkinsJobScheduler
    {
        string Group { get; }
        void Schedule(IScheduler scheduler, string job, string url);
        void Remove(IScheduler scheduler, string job);
        JobKey KeyFor(string job);
        string TriggerGroup(string job);
    }
}