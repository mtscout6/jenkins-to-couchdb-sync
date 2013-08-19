using System;
using Quartz;
using RestSharp;
using sabatoast_puller.Jenkins;
using sabatoast_puller.Sabatoast;
using FubuCore;
using System.Collections.Generic;

namespace sabatoast_puller.Quartz.Jobs
{
    public class Root : IJob
    {
        private readonly IJenkinsRestClient _restClient;
        private readonly IJobsCache _jobCache;

        public Root(IJenkinsRestClient restClient, IJobsCache jobCache)
        {
            _restClient = restClient;
            _jobCache = jobCache;
        }

        public void Execute(IJobExecutionContext context)
        {
            var request = new RestRequest("api/json", Method.GET);

            _restClient.ExecuteAsync<Jenkins.DTO.Root>(request, response =>
                {
                    if (response.ResponseStatus != ResponseStatus.Completed)
                    {
                        Console.WriteLine("Failed to retrieve {0}".ToFormat(_restClient.BuildUri(request).PathAndQuery));
                        return;
                    }

                    response.Data.Jobs.Each(_jobCache.AddJob);
                });
        }
    }
}