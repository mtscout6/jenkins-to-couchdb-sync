using System.Threading.Tasks;
using Common.Logging;
using Quartz;
using sabatoast_puller.Jenkins;
using FubuCore;
using sabatoast_puller.Jenkins.Models;

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
            var job = context.JobDetail.Key.Name;

            _log.Info("Polling Jenkins job {0}".ToFormat(job));

            _client.Job(job)
                .ContinueWith(t => Process(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        void Process(JenkinsJobModel job)
        {
            // TODO: Implement Job Handling
        }
    }
}