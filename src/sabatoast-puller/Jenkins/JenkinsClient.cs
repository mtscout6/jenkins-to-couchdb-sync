using System.Net;
using System.Threading.Tasks;
using Common.Logging;
using RestSharp;
using sabatoast_puller.Couch;
using sabatoast_puller.Jenkins.Models;
using FubuCore;

namespace sabatoast_puller.Jenkins
{
    public interface IJenkinsClient
    {
        Task<Root> Root();
    }

    public class JenkinsClient : IJenkinsClient
    {
        private readonly IJenkinsRestClient _client;
        private readonly ICouchClient _couchClient;
        private readonly ILog _log;

        public JenkinsClient(IJenkinsRestClient client, ICouchClient couchClient, ILog log)
        {
            _client = client;
            _couchClient = couchClient;
            _log = log;
        }

        public Task<Root> Root()
        {
            var request = new RestRequest("api/json", Method.GET);
            return Process<Root>(request);
        }

        Task<T> Process<T>(IRestRequest request) where T : ICouchDocument
        {
            var jenkinsRequestTask = _client.ExecuteTaskAsync<T>(request);

            var responseTask = jenkinsRequestTask.ContinueWith(t =>
                {
                    var response = t.Result;
                    var url = _client.BuildUri(request).PathAndQuery;

                    if (response.ResponseStatus != ResponseStatus.Completed)
                    {
                        var message = "Failed to connect to Jenkins: {0}".ToFormat(url);
                        _log.Error(message);
                        throw new JenkinsFailedException(message);
                    }

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var message = "Failed to retrieve from Jenkins: {0}".ToFormat(url);
                        _log.Error(message);
                        throw new JenkinsFailedException(message);
                    }

                    return response;
                });

            // TODO: Save off to couch here

            return responseTask.ContinueWith(t => t.Result.Data, TaskContinuationOptions.ExecuteSynchronously);
        }
    }
}