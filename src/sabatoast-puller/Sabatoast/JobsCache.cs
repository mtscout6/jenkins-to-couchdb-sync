using System.Collections.Generic;
using sabatoast_puller.Jenkins.DTO;

namespace sabatoast_puller.Sabatoast
{
    public class JobsCache : IJobsCache
    {
        private readonly Dictionary<string, Job> _cache;

        public JobsCache()
        {
            _cache = new Dictionary<string, Job>();
        }

        public void AddJob(RootJob rawJob)
        {
            if (_cache.ContainsKey(rawJob.Name))
            {
                return;
            }

            var job = new Job(rawJob.Name, rawJob.Url);
            _cache.Add(rawJob.Name, job);
        }
    }
}