using Common.Logging;
using RestSharp;
using sabatoast_puller.Utils.Json;

namespace sabatoast_puller.Couch
{
    public class CouchRestClient : RestClientWrapper, ICouchRestClient
    {
        public CouchRestClient(CouchSettings settings, ILog log)
            : base(new RestClient(settings.Uri.AbsoluteUri), log)
        {
            AddHandler("application/json", new NewtonsoftJsonDeserializer());
        }
    }
}