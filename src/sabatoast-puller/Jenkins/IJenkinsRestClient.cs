using RestSharp;

namespace sabatoast_puller.Jenkins
{
    public interface IJenkinsRestClient : IRestClient { }

    public class JenkinsRestClient : RestClientWrapper, IJenkinsRestClient
    {
        public JenkinsRestClient(JenkinsSettings settings)
            : base(new RestClient(settings.Url){Authenticator = new BasicEncodedAuthenticator(settings.Auth)}) { }
    }
}