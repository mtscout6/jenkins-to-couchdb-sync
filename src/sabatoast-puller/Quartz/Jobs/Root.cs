using System.Threading.Tasks;
using Common.Logging;
using Quartz;
using StructureMap;
using sabatoast_puller.Jenkins;
using System.Collections.Generic;
using FubuCore;
using sabatoast_puller.Quartz.Schedulers;

namespace sabatoast_puller.Quartz.Jobs
{
    public class Root : IJobWithScheduler
    {
        private readonly IJenkinsClient _jenkinsClient;
        private readonly ILog _log;
        public IScheduler Scheduler { get; set; }
        private readonly IJenkinsJobScheduler _jobScheduler;

        public Root(IJenkinsClient jenkinsClient, ILog log, IJenkinsJobScheduler jobScheduler)
        {
            _jenkinsClient = jenkinsClient;
            _log = log;
            _jobScheduler = jobScheduler;
        }

        public void Execute(IJobExecutionContext context)
        {
            _log.Info("Polling Jenkins job list");
            _jenkinsClient.Root()
                          .ContinueWith(t => Process(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        void Process(Jenkins.Models.Root root)
        {
            root.Jobs.Each(job =>
                {
                    _log.Debug("Scheduling job {0}".ToFormat(job.Name));
                    _jobScheduler.Schedule(Scheduler, job.Name, job.Url);
                });
        }

    }
}