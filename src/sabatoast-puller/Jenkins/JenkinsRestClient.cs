using Common.Logging;
using RestSharp;

namespace sabatoast_puller.Jenkins
{
    public class JenkinsRestClient : RestClientWrapper, IJenkinsRestClient
    {
        public JenkinsRestClient(JenkinsSettings settings, ILog log)
            : base(new RestClient(settings.Url){Authenticator = new BasicEncodedAuthenticator(settings.Auth)}, log) { }
    }
}