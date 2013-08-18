using RestSharp;

namespace sabatoast_puller.Sabatoast
{
    public interface ISabatoastRestClient : IRestClient { }

    public class SabatoastRestClient : RestClientWrapper, ISabatoastRestClient
    {
        public SabatoastRestClient(SabatoastSettings settings) : base(new RestClient(settings.Url)) { }
    }
}