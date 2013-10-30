using Common.Logging;
using Quartz;
using sabatoast_puller.Jenkins;
using FubuCore;

namespace sabatoast_puller.Quartz.Jobs
{
    public class JenkinsJob : IJob
    {
        private readonly IJenkinsClient _client;
        private readonly ILog _log;

        public JenkinsJob(IJenkinsClient client, ILog log)
        {
            _client = client;
            _log = log;
        }

        public void Execute(IJobExecutionContext context)
        {
            var job = context.Get("job");
            var url = context.Get("url");

            _log.Info("Polling Jenkins job {0}".ToFormat(job));
        }
    }
}