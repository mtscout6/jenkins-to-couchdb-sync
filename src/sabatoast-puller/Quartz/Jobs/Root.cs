using System.Threading.Tasks;
using Common.Logging;
using Quartz;
using sabatoast_puller.Jenkins;
using System.Collections.Generic;
using FubuCore;

namespace sabatoast_puller.Quartz.Jobs
{
    public class Root : IJob
    {
        private readonly IJenkinsClient _jenkinsClient;
        private readonly ILog _log;

        public Root(IJenkinsClient jenkinsClient, ILog log)
        {
            _jenkinsClient = jenkinsClient;
            _log = log;
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
                    _log.Debug("Processing job {0}".ToFormat(job.Name));
                });
        }
    }
}