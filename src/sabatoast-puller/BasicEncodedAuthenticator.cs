using System;
using System.Linq;
using FubuCore;
using RestSharp;

namespace sabatoast_puller
{
    public class BasicEncodedAuthenticator : IAuthenticator
    {
        private readonly string _auth;

        public BasicEncodedAuthenticator(string auth)
        {
            _auth = "Basic {0}".ToFormat(auth);
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            if (request.Parameters.Any(x => x.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            request.AddParameter("Authorization", _auth, ParameterType.HttpHeader);

        }
    }
}