using System;
using Quartz;
using RestSharp;
using sabatoast_puller.Jenkins;

namespace sabatoast_puller.Quartz.Jobs
{
    public class Root : IJob
    {
        private readonly IJenkinsRestClient _restClient;

        public Root(IJenkinsRestClient restClient)
        {
            _restClient = restClient;
        }

        public void Execute(IJobExecutionContext context)
        {
            var request = new RestRequest("api/json", Method.GET);

            _restClient.ExecuteAsync<Jenkins.DTO.Root>(request, response =>
                {
                    Console.WriteLine("Response Received");
                });
        }
    }
}