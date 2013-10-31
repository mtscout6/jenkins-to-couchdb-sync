using Quartz;

namespace sabatoast_puller.Quartz.Schedulers
{
    public interface IJenkinsJobScheduler
    {
        string Group { get; }
        void Schedule(IScheduler scheduler, string job, string url);
        JobKey KeyFor(string job);
    }
}