using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;
using Quartz.Util;
using sabatoast_puller.Utils.Json;

namespace sabatoast_puller.Couch
{
    public interface ICouchClient
    {
        Task<IRestResponse<CouchResponse>> Save<T>(T document) where T : ICouchDocument;
        Task<IRestResponse<CouchResponse>> Save(JObject obj);
        Task<IRestResponse<T>> Get<T>(string id) where T : ICouchDocument;
    }

    public class CouchClient : ICouchClient
    {
        private readonly ICouchRestClient _client;
        private readonly CouchSettings _settings;

        public CouchClient(ICouchRestClient client, CouchSettings settings)
        {
            _client = client;
            _settings = settings;
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

            return _client.ExecuteTaskAsync<CouchResponse>(request)
                          .ContinueWith(responseTask =>
                              {
                                  var response = responseTask.Result;

                                  if (200 <= (int) response.StatusCode && (int) response.StatusCode < 300)
                                  {
                                      document._rev = response.Data.Rev;
                                      return response;
                                  }

                                  return response;
                              });
        }

        public Task<IRestResponse<CouchResponse>> Save(JObject obj)
        {
            var request = BuildRequest(obj["_id"].Value<string>());
            request.Method = Method.PUT;
            request.AddHeader("Content-Type", "application/json");
            request.JsonSerializer = new NewtonsoftJsonSerializer();
            request.RequestFormat = DataFormat.Json;
            request.AddBody(obj);

            return _client.ExecuteTaskAsync<CouchResponse>(request);
        }

        public Task<IRestResponse<T>> Get<T>(string id) where T : ICouchDocument
        {
            if (id.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("id", "Must provide a document id to retrieve");
            }

            var request = BuildRequest(id);
            request.Method = Method.GET;

            return _client.ExecuteTaskAsync<T>(request);
        }

        RestRequest BuildRequest(string id)
        {
            var request = new RestRequest(id);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Referer", _settings.Uri.Host);
            request.RequestFormat = DataFormat.Json;

            return request;
        }
    }
}