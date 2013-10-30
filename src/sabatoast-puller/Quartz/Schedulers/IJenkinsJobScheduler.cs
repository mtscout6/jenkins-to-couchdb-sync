using sabatoast_puller.Jenkins.Models;

namespace sabatoast_puller.Quartz.Schedulers
{
    public interface IJenkinsJobScheduler
    {
        void Schedule(string job, string url);
    }
}