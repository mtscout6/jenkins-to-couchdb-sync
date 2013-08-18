using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using RestSharp;

namespace sabatoast_puller
{
    public abstract class RestClientWrapper : IRestClient
    {
        private readonly IRestClient _client;

        protected RestClientWrapper(IRestClient client)
        {
            _client = client;
        }

        public RestRequestAsyncHandle ExecuteAsync(IRestRequest request,
                                                   Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            return _client.ExecuteAsync(request, callback);
        }

        public RestRequestAsyncHandle ExecuteAsync<T>(IRestRequest request,
                                                      Action<IRestResponse<T>, RestRequestAsyncHandle> callback)
        {
            return _client.ExecuteAsync(request, callback);
        }

        public IRestResponse Execute(IRestRequest request)
        {
            return _client.Execute(request);
        }

        public IRestResponse<T> Execute<T>(IRestRequest request) where T : new()
        {
            return _client.Execute<T>(request);
        }

        public Uri BuildUri(IRestRequest request)
        {
            return _client.BuildUri(request);
        }

        public RestRequestAsyncHandle ExecuteAsyncGet(IRestRequest request,
                                                      Action<IRestResponse, RestRequestAsyncHandle> callback,
                                                      string httpMethod)
        {
            return _client.ExecuteAsyncGet(request, callback, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsyncPost(IRestRequest request,
                                                       Action<IRestResponse, RestRequestAsyncHandle> callback,
                                                       string httpMethod)
        {
            return _client.ExecuteAsyncPost(request, callback, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsyncGet<T>(IRestRequest request,
                                                         Action<IRestResponse<T>, RestRequestAsyncHandle> callback,
                                                         string httpMethod)
        {
            return _client.ExecuteAsyncGet(request, callback, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsyncPost<T>(IRestRequest request,
                                                          Action<IRestResponse<T>, RestRequestAsyncHandle> callback,
                                                          string httpMethod)
        {
            return _client.ExecuteAsyncPost(request, callback, httpMethod);
        }

        public IRestResponse ExecuteAsGet(IRestRequest request, string httpMethod)
        {
            return _client.ExecuteAsGet(request, httpMethod);
        }

        public IRestResponse ExecuteAsPost(IRestRequest request, string httpMethod)
        {
            return _client.ExecuteAsPost(request, httpMethod);
        }

        public IRestResponse<T> ExecuteAsGet<T>(IRestRequest request, string httpMethod) where T : new()
        {
            return _client.ExecuteAsGet<T>(request, httpMethod);
        }

        public IRestResponse<T> ExecuteAsPost<T>(IRestRequest request, string httpMethod) where T : new()
        {
            return _client.ExecuteAsPost<T>(request, httpMethod);
        }

        public CookieContainer CookieContainer
        {
            get { return _client.CookieContainer; }
            set { _client.CookieContainer = value; }
        }

        public string UserAgent
        {
            get { return _client.UserAgent; }
            set { _client.UserAgent = value; }
        }

        public int Timeout
        {
            get { return _client.Timeout; }
            set { _client.Timeout = value; }
        }

        public bool UseSynchronizationContext
        {
            get { return _client.UseSynchronizationContext; }
            set { _client.UseSynchronizationContext = value; }
        }

        public IAuthenticator Authenticator
        {
            get { return _client.Authenticator; }
            set { _client.Authenticator = value; }
        }

        public string BaseUrl
        {
            get { return _client.BaseUrl; }
            set { _client.BaseUrl = value; }
        }

        public IList<Parameter> DefaultParameters
        {
            get { return _client.DefaultParameters; }
        }

        public X509CertificateCollection ClientCertificates
        {
            get { return _client.ClientCertificates; }
            set { _client.ClientCertificates = value; }
        }

        public IWebProxy Proxy
        {
            get { return _client.Proxy; }
            set { _client.Proxy = value; }
        }
    }
}