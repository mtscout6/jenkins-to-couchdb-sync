using System;
using System.Net;
using System.Threading.Tasks;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using sabatoast_puller.Couch;
using sabatoast_puller.Jenkins.Models;
using FubuCore;

namespace sabatoast_puller.Jenkins
{
    public interface IJenkinsClient
    {
        Task<RootModel> Root();
        Task<JenkinsJobModel> Job(string job);
        Task<Build> Build(string job, int build);
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

        public Task<RootModel> Root()
        {
            var request = new RestRequest("api/json", Method.GET);
            return Process<RootModel>(request);
        }

        public Task<JenkinsJobModel> Job(string job)
        {
            var request = new RestRequest("job/{0}/api/json".ToFormat(job), Method.GET);
            return Process<JenkinsJobModel>(request);
        }

        public Task<Build> Build(string job, int build)
        {
            var request = new RestRequest("job/{0}/{1}/api/json".ToFormat(job, build), Method.GET);
            return Process<Build>(request, b => b["job"] = job);
        }

        Task<T> Process<T>(IRestRequest request, Action<JObject> modify = null) where T : ICouchDocument
        {
            var jenkinsRequestTask = _client.ExecuteTaskAsync<T>(request);
            var url = _client.BuildUri(request).PathAndQuery;

            var responseTask = jenkinsRequestTask.ContinueWith(t =>
                {
                    var response = t.Result;

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
                }, TaskContinuationOptions.OnlyOnRanToCompletion);

            responseTask.ContinueWith(t =>
                {
                    var jenkinsResponse = t.Result;

                    _couchClient.Get<T>(jenkinsResponse.Data._id)
                                .ContinueWith(ct =>
                                    {
                                        bool save;
                                        var couchResponse = ct.Result;

                                        var jenkinsData = JsonConvert.DeserializeObject<JObject>(jenkinsResponse.Content);
                                        jenkinsData["_id"] = jenkinsResponse.Data._id;
                                        jenkinsData["type"] = jenkinsResponse.Data.type;

                                        if (modify != null)
                                        {
                                            modify(jenkinsData);
                                        }

                                        if (couchResponse.StatusCode == HttpStatusCode.OK)
                                        {
                                            var couchData = JsonConvert.DeserializeObject<JObject>(couchResponse.Content);
                                            jenkinsData["_rev"] = couchData["_rev"];

                                            save = !JToken.DeepEquals(jenkinsData, couchData);
                                        }
                                        else
                                        {
                                            save = true;
                                        }

                                        if (save)
                                        {
                                            _couchClient.Save(jenkinsData);
                                        }

                                    }, TaskContinuationOptions.OnlyOnRanToCompletion);
                }, TaskContinuationOptions.OnlyOnRanToCompletion);

            responseTask.ContinueWith(t =>
                {
                    _log.Error("Failed to process Jenkins request for url '{0}'".ToFormat(url), t.Exception);
                }, TaskContinuationOptions.OnlyOnFaulted);

            return responseTask.ContinueWith(t => t.Result.Data, TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}