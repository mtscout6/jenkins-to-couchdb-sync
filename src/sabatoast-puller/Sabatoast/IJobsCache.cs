using sabatoast_puller.Jenkins.DTO;

namespace sabatoast_puller.Sabatoast
{
    public interface IJobsCache
    {
        void AddJob(RootJob job);
    }
}