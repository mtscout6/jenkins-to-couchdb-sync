﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using RestSharp;
using FubuCore;
using RestSharp.Deserializers;

namespace sabatoast_puller
{
    public abstract class RestClientWrapper : IRestClient
    {
        private readonly RestClient _client;
        private readonly ILog _log;

        protected RestClientWrapper(RestClient client, ILog log)
        {
            _client = client;
            _log = log;
        }

        public RestRequestAsyncHandle ExecuteAsync(IRestRequest request,
                                                   Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            _log.Debug("Executing async request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteAsync(request, (response, handle) =>
                {
                    _log.Debug("Finished async request: {0} [{1}]".ToFormat(request.Resource, request.Method));
                    callback(response, handle);
                });
        }

        public RestRequestAsyncHandle ExecuteAsync<T>(IRestRequest request,
                                                      Action<IRestResponse<T>, RestRequestAsyncHandle> callback)
        {
            _log.Debug("Executing async request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteAsync(request, new Action<IRestResponse<T>, RestRequestAsyncHandle>((response, handle) =>
                {
                    _log.Debug("Finished async request: {0} [{1}]".ToFormat(request.Resource, request.Method));
                    callback(response, handle);
                }));
        }

        public IRestResponse Execute(IRestRequest request)
        {
            _log.Debug("Executing request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.Execute(request);
        }

        public IRestResponse<T> Execute<T>(IRestRequest request) where T : new()
        {
            _log.Debug("Executing request: {0} [{1}]".ToFormat(request.Resource, request.Method));
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
            _log.Debug("Executing async get request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteAsyncGet(request, (response, handle) =>
                {
                    _log.Debug("Finished async get request: {0} [{1}]".ToFormat(request.Resource, request.Method));
                    callback(response, handle);
                }, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsyncPost(IRestRequest request,
                                                       Action<IRestResponse, RestRequestAsyncHandle> callback,
                                                       string httpMethod)
        {
            _log.Debug("Executing async post request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteAsyncPost(request, (response, handle) =>
                {
                    _log.Debug("Finished async post request: {0} [{1}]".ToFormat(request.Resource, request.Method));
                    callback(response, handle);
                }, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsyncGet<T>(IRestRequest request,
                                                         Action<IRestResponse<T>, RestRequestAsyncHandle> callback,
                                                         string httpMethod)
        {
            _log.Debug("Executing async get request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteAsyncGet(request, new Action<IRestResponse<T>, RestRequestAsyncHandle>((response, handle) =>
                {
                    _log.Debug("Finished async get request: {0} [{1}]".ToFormat(request.Resource, request.Method));
                    callback(response, handle);
                }), httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsyncPost<T>(IRestRequest request,
                                                          Action<IRestResponse<T>, RestRequestAsyncHandle> callback,
                                                          string httpMethod)
        {
            _log.Debug("Executing async post request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteAsyncPost(request, new Action<IRestResponse<T>, RestRequestAsyncHandle>((response, handle) =>
                {
                    _log.Debug("Finished async post request: {0} [{1}]".ToFormat(request.Resource, request.Method));
                    callback(response, handle);
                }), httpMethod);
        }

        public IRestResponse ExecuteAsGet(IRestRequest request, string httpMethod)
        {
            _log.Debug("Executing get request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteAsGet(request, httpMethod);
        }

        public IRestResponse ExecuteAsPost(IRestRequest request, string httpMethod)
        {
            _log.Debug("Executing post request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteAsPost(request, httpMethod);
        }

        public IRestResponse<T> ExecuteAsGet<T>(IRestRequest request, string httpMethod) where T : new()
        {
            _log.Debug("Executing get request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteAsGet<T>(request, httpMethod);
        }

        public IRestResponse<T> ExecuteAsPost<T>(IRestRequest request, string httpMethod) where T : new()
        {
            _log.Debug("Executing post request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteAsPost<T>(request, httpMethod);
        }

        public Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request, CancellationToken token)
        {
            _log.Debug("Executing async task request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteTaskAsync<T>(request, token);
        }

        public Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request)
        {
            _log.Debug("Executing async task request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteTaskAsync<T>(request);
        }

        public Task<IRestResponse<T>> ExecuteGetTaskAsync<T>(IRestRequest request)
        {
            _log.Debug("Executing async task get request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteGetTaskAsync<T>(request);
        }

        public Task<IRestResponse<T>> ExecuteGetTaskAsync<T>(IRestRequest request, CancellationToken token)
        {
            _log.Debug("Executing async task get request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteGetTaskAsync<T>(request, token);
        }

        public Task<IRestResponse<T>> ExecutePostTaskAsync<T>(IRestRequest request)
        {
            _log.Debug("Executing async task post request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecutePostTaskAsync<T>(request);
        }

        public Task<IRestResponse<T>> ExecutePostTaskAsync<T>(IRestRequest request, CancellationToken token)
        {
            _log.Debug("Executing async task post request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecutePostTaskAsync<T>(request, token);
        }

        public Task<IRestResponse> ExecuteTaskAsync(IRestRequest request, CancellationToken token)
        {
            _log.Debug("Executing async task request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteTaskAsync(request, token);
        }

        public Task<IRestResponse> ExecuteTaskAsync(IRestRequest request)
        {
            _log.Debug("Executing async task request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteTaskAsync(request);
        }

        public Task<IRestResponse> ExecuteGetTaskAsync(IRestRequest request)
        {
            _log.Debug("Executing async task get request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteGetTaskAsync(request);
        }

        public Task<IRestResponse> ExecuteGetTaskAsync(IRestRequest request, CancellationToken token)
        {
            _log.Debug("Executing async task get request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecuteGetTaskAsync(request, token);
        }

        public Task<IRestResponse> ExecutePostTaskAsync(IRestRequest request)
        {
            _log.Debug("Executing async task post request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecutePostTaskAsync(request);
        }

        public Task<IRestResponse> ExecutePostTaskAsync(IRestRequest request, CancellationToken token)
        {
            _log.Debug("Executing async task post request: {0} [{1}]".ToFormat(request.Resource, request.Method));
            return _client.ExecutePostTaskAsync(request, token);
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

        public void AddHandler(string contentType, IDeserializer deserializer)
        {
            _client.AddHandler(contentType, deserializer);
        }
    }
}