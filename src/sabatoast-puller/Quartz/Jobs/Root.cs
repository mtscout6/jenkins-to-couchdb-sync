using System;
using Common.Logging;
using Quartz;
using RestSharp;
using sabatoast_puller.Jenkins;
using FubuCore;

namespace sabatoast_puller.Quartz.Jobs
{
    public class Root : IJob
    {
        private readonly IJenkinsRestClient _jenkinsClient;
        private readonly ILog _log;

        public Root(IJenkinsRestClient jenkinsClient, ILog log)
        {
            _jenkinsClient = jenkinsClient;
            _log = log;
        }

        public void Execute(IJobExecutionContext context)
        {
            var request = new RestRequest("api/json", Method.GET);

            _jenkinsClient.ExecuteAsync<Jenkins.DTO.Root>(request, response =>
                {
                    if (response.ResponseStatus != ResponseStatus.Completed)
                    {
                        _log.Error("Failed to retrieve {0}".ToFormat(_jenkinsClient.BuildUri(request).PathAndQuery));
                        return;
                    }

                    ProcessRootData(response.Data);
                });
        }

        void ProcessRootData(Jenkins.DTO.Root rootData)
        {

        }
    }
}