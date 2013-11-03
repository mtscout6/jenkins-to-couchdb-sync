using Common.Logging;
using Quartz;
using FubuCore;
using sabatoast_puller.Jenkins;

namespace sabatoast_puller.Quartz.Jobs
{
    public class JenkinsBuild : IJob
    {
        private readonly IJenkinsClient _jenkinsClient;
        private readonly ILog _log;

        public JenkinsBuild(IJenkinsClient jenkinsClient, ILog log)
        {
            _jenkinsClient = jenkinsClient;
            _log = log;
        }

        public void Execute(IJobExecutionContext context)
        {
            var job = (string) context.JobDetail.JobDataMap["job"];
            var build = (int) context.JobDetail.JobDataMap["build"];

            _log.Info("Polling {0} build {1}".ToFormat(job, build));
            _jenkinsClient.Build(job, build);
        }
    }
}