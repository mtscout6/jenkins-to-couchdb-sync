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
using Quartz.Util;

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
            return Process<Build>(request, b => b.Job = job, b => b["job"] = job);
        }

        Task<T> Process<T>(IRestRequest request, Action<T> modifyJenkinsData = null, Action<JObject> modifyRawJson = null) where T : ICouchDocument
        {
            var jenkinsRequestTask = _client.ExecuteTaskAsync<T>(request);
            var url = _client.BuildUri(request).PathAndQuery;

            var responseTask = jenkinsRequestTask.ContinueWith(t =>
                {
                    var response = t.Result;

                    if (response.ResponseStatus != ResponseStatus.Completed)
                    {
                        var message = "Failed to connect to Jenkins: {0} [Response Status: {1}]".ToFormat(url, response.ResponseStatus);
                        _log.Error(message);
                        throw new JenkinsFailedException(message);
                    }

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var message = "Failed to retrieve from Jenkins: {0} [Status Code: {1}]".ToFormat(url, response.StatusCode);
                        _log.Error(message);
                        throw new JenkinsFailedException(message);
                    }

                    return response;
                }, TaskContinuationOptions.OnlyOnRanToCompletion);

            responseTask.ContinueWith(t => _log.Error("Failed to process Jenkins request for url '{0}'".ToFormat(url), t.Exception), TaskContinuationOptions.OnlyOnFaulted);

            if (modifyJenkinsData != null)
            {
                responseTask = responseTask.ContinueWith(t =>
                    {
                        var response = t.Result;
                        modifyJenkinsData(response.Data);
                        return response;
                    });
            }

            responseTask.ContinueWith(t =>
                {
                    var jenkinsResponse = t.Result;

                    _couchClient.Get<T>(jenkinsResponse.Data._id)
                                .ContinueWith(ct =>
                                    {
                                        bool save;
                                        var couchResponse = ct.Result;

                                        var jenkinsData = JsonConvert.DeserializeObject<JObject>(jenkinsResponse.Content);
                                        ExtractDescriptionMetaData(jenkinsData, jenkinsResponse);

                                        if (modifyRawJson != null)
                                        {
                                            modifyRawJson(jenkinsData);
                                        }

                                        jenkinsData["_id"] = jenkinsResponse.Data._id;
                                        jenkinsData["type"] = jenkinsResponse.Data.type;

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

            return responseTask.ContinueWith(t => t.Result.Data, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        void ExtractDescriptionMetaData<T>(JObject jenkinsData, IRestResponse<T> jenkinsResponse) where T : ICouchDocument
        {
            JToken descToken;
            if (!jenkinsData.TryGetValue("description", out descToken) || descToken.Value<string>() == null)
            {
                return;
            }

            var descriptionLines = descToken.Value<string>()
                                            .Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            var inComment = false;

            foreach (var line in descriptionLines)
            {
                if (line.StartsWith("<!--"))
                {
                    inComment = true;
                    continue;
                }

                if (line.StartsWith("-->"))
                {
                    inComment = false;
                    continue;
                }

                if (!inComment || !line.StartsWith("@"))
                {
                    continue;
                }

                var parts = line.TrimStart('@');
                var splitIndex = parts.IndexOf(":", StringComparison.CurrentCulture);
                var key = parts.Substring(0, splitIndex);
                var value = parts.Substring(splitIndex + 1);

                if (value.IsNullOrWhiteSpace()) continue;

                try
                {
                    var valueJson = JsonConvert.DeserializeObject<JObject>(value);
                    jenkinsData["description-{0}".ToFormat(key)] = valueJson;
                }
                catch (Exception ex)
                {
                    _log.Error("Failed to parse description meta data for {0}".ToFormat(jenkinsResponse.Data._id), ex);
                }
            }
        }
    }
}