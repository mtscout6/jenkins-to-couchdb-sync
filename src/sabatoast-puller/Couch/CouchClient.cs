using System;
using System.Net;
using RestSharp;
using Quartz.Util;
using FubuCore;

namespace sabatoast_puller.Couch
{
    public interface ICouchClient
    {
        void Save<T>(ICouchDocument<T> document);
    }

    public class CouchClient : ICouchClient
    {
        private readonly IRestClient _client;
        private readonly string _database;

        public CouchClient(IRestClient client, string database)
        {
            _client = client;
            _database = database;
        }

        public void Save<T>(ICouchDocument<T> document)
        {
            if (document._id.IsNullOrWhiteSpace())
            {
                document._id = Guid.NewGuid().ToString();
            }

            var request = new RestRequest(document._id, Method.PUT);

            _client.ExecuteAsync<CouchResponse>(request, response =>
                {
                    if (response.StatusCode == HttpStatusCode.Accepted)
                    {
                        document._rev = response.Data.Rev;
                        return;
                    }

                    throw new Exception("Failed to save document {0}".ToFormat(document._id));
                });
        }

        public ICouchDocument<T> Get<T>(string id)
        {
            throw new NotImplementedException();
        }
    }

    public class CouchResponse
    {
        public string Rev;
    }
}