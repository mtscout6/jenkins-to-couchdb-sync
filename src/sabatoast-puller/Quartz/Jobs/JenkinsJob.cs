using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Logging;
using Quartz;
using Quartz.Impl.Matchers;
using sabatoast_puller.Couch;
using sabatoast_puller.Jenkins;
using FubuCore;
using sabatoast_puller.Jenkins.Models;
using sabatoast_puller.Quartz.Schedulers;

namespace sabatoast_puller.Quartz.Jobs
{
    public class JenkinsJob : IJobWithScheduler
    {
        public IScheduler Scheduler { get; set; }

        private readonly IJenkinsClient _client;
        private readonly ILog _log;
        private readonly ICouchClient _couchClient;
        private readonly IBuildScheduler _buildScheduler;

        public JenkinsJob(IJenkinsClient client, ILog log, ICouchClient couchClient, IBuildScheduler buildScheduler)
        {
            _client = client;
            _log = log;
            _couchClient = couchClient;
            _buildScheduler = buildScheduler;
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
            ProcessBuilds(job);
        }

        void ProcessBuilds(JenkinsJobModel job)
        {
            var builds = new HashSet<int>(
                Scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(_buildScheduler.Group(job.Name)))
                         .Select(x => int.Parse(x.Name)));

            _couchClient.GetSavedCompleteBuildsFor(job.Name)
                        .ContinueWith(t =>
                            {
                                var completeBuilds = t.Result;

                                job.Builds
                                   .Where(build => !completeBuilds.Contains(build.Number))
                                   .Each(build =>
                                       {
                                           if (builds.Contains(build.Number))
                                           {
                                               builds.Remove(build.Number);
                                               return;
                                           }

                                           _log.Info("Scheduling build {0} for {1}".ToFormat(build.Number, job.Name));
                                           _buildScheduler.Schedule(Scheduler, job.Name, build.Number);
                                       });

                                builds.Each(b => _buildScheduler.Remove(Scheduler, job.Name, b));

                            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}