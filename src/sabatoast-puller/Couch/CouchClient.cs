using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using Quartz.Util;
using sabatoast_puller.Utils.Json;
using FubuCore;

namespace sabatoast_puller.Couch
{
    public interface ICouchClient
    {
        Task<IRestResponse<CouchResponse>> Save<T>(T document) where T : ICouchDocument;
        Task<IRestResponse<CouchResponse>> Save(JObject obj);
        Task<IRestResponse<T>> Get<T>(string id) where T : ICouchDocument;
        Task<HashSet<int>> GetSavedCompleteBuildsFor(string job);
    }

    public class CouchClient : ICouchClient
    {
        private readonly ICouchRestClient _client;
        private readonly CouchSettings _settings;
        private readonly ILog _log;

        public CouchClient(ICouchRestClient client, CouchSettings settings, ILog log)
        {
            _client = client;
            _settings = settings;
            _log = log;
        }

        public Task<IRestResponse<CouchResponse>> Save<T>(T document) where T : ICouchDocument
        {
            if (document._id.IsNullOrWhiteSpace())
            {
                document._id = Guid.NewGuid().ToString();
            }

            var request = BuildRequest(document._id);
            request.Method = Method.PUT;
            request.JsonSerializer = new NewtonsoftJsonSerializer();
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddBody(document);

            var couchRequestTask = _client.ExecuteTaskAsync<CouchResponse>(request);
            couchRequestTask.ContinueWith(t => _log.Error("Failed to save document {0}".ToFormat(document._id), t.Exception), TaskContinuationOptions.OnlyOnFaulted);

            return couchRequestTask.ContinueWith(responseTask =>
                              {
                                  var response = responseTask.Result;

                                  if (200 <= (int) response.StatusCode && (int) response.StatusCode < 300)
                                  {
                                      document._rev = response.Data.Rev;
                                      return response;
                                  }

                                  return response;
                              }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public Task<IRestResponse<CouchResponse>> Save(JObject obj)
        {
            var request = BuildRequest(obj["_id"].Value<string>());
            request.Method = Method.PUT;
            request.AddHeader("Content-Type", "application/json");
            request.JsonSerializer = new NewtonsoftJsonSerializer();
            request.RequestFormat = DataFormat.Json;
            request.AddBody(obj);

            var couchRequestTask = _client.ExecuteTaskAsync<CouchResponse>(request);
            couchRequestTask.ContinueWith(t => _log.Error("Failed to save document {0}".ToFormat(obj["_id"]), t.Exception), TaskContinuationOptions.OnlyOnFaulted);
            return couchRequestTask;
        }

        public Task<IRestResponse<T>> Get<T>(string id) where T : ICouchDocument
        {
            if (id.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("id", "Must provide a document id to retrieve");
            }

            var request = BuildRequest(id);
            request.Method = Method.GET;

            var couchRequestTask = _client.ExecuteTaskAsync<T>(request);
            couchRequestTask.ContinueWith(t => _log.Error("Failed to retrieve document {0}".ToFormat(id), t.Exception), TaskContinuationOptions.OnlyOnFaulted);
            return couchRequestTask;
        }

        public Task<HashSet<int>> GetSavedCompleteBuildsFor(string job)
        {
            var request = ConfigureRequest(new RestRequest("/_design/builds/_view/complete-builds"));
            request.Method = Method.GET;
            request.Parameters.Add(new Parameter
                {
                    Type = ParameterType.QueryString,
                    Name = "key",
                    Value = "\"{0}\"".ToFormat(job)
                });

            var couchRequestTask = _client.ExecuteTaskAsync<CouchQuery<Value<int>>>(request);
            couchRequestTask.ContinueWith(t => _log.Error("Failed to retrieve list of completed builds for {0}".ToFormat(job), t.Exception), TaskContinuationOptions.OnlyOnFaulted);

            return couchRequestTask
                .ContinueWith(t =>
                    {
                        var data = t.Result.Data.rows;
                        return new HashSet<int>(data.Select(x => x.value));
                    }, TaskContinuationOptions.OnlyOnRanToCompletion);

/*
function(doc) {
  if (doc.type === 'Build' && doc.building === false) {
    emit(doc.job, doc.number);
  }
}
*/
        }

        RestRequest BuildRequest(string id)
        {
            var request = new RestRequest(id);
            return ConfigureRequest(request);
        }

        RestRequest ConfigureRequest(RestRequest request)
        {
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Referer", _settings.Uri.Host);
            request.RequestFormat = DataFormat.Json;

            return request;
        }

        public class Value<T>
        {
            public T value { get; set; }
        }

        public class CouchQuery<T>
        {
            public int total_rows { get; set; }
            public int offset { get; set; }
            public List<T> rows { get; set; }
        }
    }
}