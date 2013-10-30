using System;
using System.Net;
using System.Threading.Tasks;
using RestSharp;
using Quartz.Util;
using FubuCore;
using sabatoast_puller.Utils.Json;

namespace sabatoast_puller.Couch
{
    public interface ICouchClient
    {
        Task Save<T>(T document) where T : ICouchDocument;
        Task<T> Get<T>(string id) where T : ICouchDocument;
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

        public Task Save<T>(T document) where T : ICouchDocument
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

                                  if ((int) response.StatusCode >= 200 && (int) response.StatusCode < 300)
                                  {
                                      document._rev = response.Data.Rev;
                                      return;
                                  }

                                  if (response.ResponseStatus == ResponseStatus.Completed)
                                  {
                                      throw new Exception("Failed to save document {0} [Error: {1}, Reason: {2}]".ToFormat(document._id, response.Data.Error, response.Data.Reason));
                                  }

                                  throw new Exception("Failed to save document {0}".ToFormat(document._id));
                              });
        }

        public Task<T> Get<T>(string id) where T : ICouchDocument
        {
            if (id.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("id", "Must provide a document id to retrieve");
            }

            var request = BuildRequest(id);
            request.Method = Method.GET;

            return _client.ExecuteTaskAsync<T>(request)
                          .ContinueWith(responseTask =>
                              {
                                  var response = responseTask.Result;

                                  if (response.StatusCode == HttpStatusCode.OK)
                                  {
                                      return response.Data;
                                  }

                                  throw new Exception("Failed to get document {0}".ToFormat(id));
                              });
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