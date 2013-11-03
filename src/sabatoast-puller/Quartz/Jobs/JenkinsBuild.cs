using Common.Logging;
using Quartz;
using FubuCore;
using sabatoast_puller.Jenkins;
using sabatoast_puller.Quartz.Schedulers;

namespace sabatoast_puller.Quartz.Jobs
{
    public class JenkinsBuild : IJobWithScheduler
    {
        public IScheduler Scheduler { get; set; }

        private readonly IJenkinsClient _jenkinsClient;
        private readonly IBuildScheduler _buildScheduler;
        private readonly ILog _log;

        public JenkinsBuild(IJenkinsClient jenkinsClient, IBuildScheduler buildScheduler, ILog log)
        {
            _jenkinsClient = jenkinsClient;
            _buildScheduler = buildScheduler;
            _log = log;
        }

        public void Execute(IJobExecutionContext context)
        {
            var job = (string) context.JobDetail.JobDataMap["job"];
            var build = (int) context.JobDetail.JobDataMap["build"];

            _log.Info("Polling {0} build {1}".ToFormat(job, build));
            _jenkinsClient.Build(job, build)
                .ContinueWith(t =>
                    {
                        var buildData = t.Result;

                        if (!buildData.Building)
                        {
                            _buildScheduler.Remove(Scheduler, job, build);
                        }
                    });
        }
    }
}