using Common.Logging;
using Quartz;
using FubuCore;

namespace sabatoast_puller.Quartz.Jobs
{
    public class JenkinsBuild : IJob
    {
        private readonly ILog _log;

        public JenkinsBuild(ILog log)
        {
            _log = log;
        }

        public void Execute(IJobExecutionContext context)
        {
            var job = (string) context.JobDetail.JobDataMap["job"];
            var build = (int) context.JobDetail.JobDataMap["build"];

            _log.Info("Polling {0} build {1}".ToFormat(job, build));
        }
    }
}