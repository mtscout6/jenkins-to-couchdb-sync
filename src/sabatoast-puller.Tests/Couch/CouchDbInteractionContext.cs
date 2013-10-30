using System;
using System.Net;
using NUnit.Framework;
using RestSharp;
using StructureMap;
using sabatoast_puller.Couch;
using sabatoast_puller.Registries;
using Should;

namespace sabatoast_puller.Tests.Couch
{
    public abstract class CouchDbInteractionContext
    {
        protected IContainer Container { get; private set; }

        [SetUp]
        public void Setup()
        {
            Container = new Container(x =>
                {
                    x.AddRegistry<LoggingRegistry>();
                    x.AddRegistry<CouchRegistry>();
                });

            Container.EjectAllInstancesOf<CouchSettings>();

            Container.Configure(x => x.For<CouchSettings>().Use(new CouchSettings
                {
                    Uri = new Uri("http://127.0.0.1:5984/test-jenkins-to-couch")
                }));

            var dropDbRequest = new RestRequest {Method = Method.DELETE};
            var delResponse = Container.GetInstance<ICouchRestClient>().Execute(dropDbRequest);
            delResponse.ResponseStatus.ShouldEqual(ResponseStatus.Completed, "Failed to drop existing CouchDB database");

            var createDbRequest = new RestRequest {Method = Method.PUT};
            var createResponse = Container.GetInstance<ICouchRestClient>().Execute(createDbRequest);
            createResponse.ResponseStatus.ShouldEqual(ResponseStatus.Completed, "Failed to create CouchDB database [Request]");
            createResponse.StatusCode.ShouldEqual(HttpStatusCode.Created, "Failed to create CouchDB database [StatusCode]");

            BeforeEach();
        }

        protected virtual void BeforeEach() { }
    }
}