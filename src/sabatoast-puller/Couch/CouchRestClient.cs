using Common.Logging;
using RestSharp;

namespace sabatoast_puller.Couch
{
    public class CouchRestClient : RestClientWrapper, ICouchRestClient
    {
        public CouchRestClient(CouchSettings settings, ILog log)
            : base(new RestClient(settings.Url), log)
        {
        }
    }
}