﻿using System;
using StructureMap.Configuration.DSL;
using sabatoast_puller.Couch;

namespace sabatoast_puller.Registries
{
    public class CouchRegistry : Registry
    {
        public CouchRegistry()
        {
            For<ICouchRestClient>().Use<CouchRestClient>();
            For<ICouchClient>().Use<CouchClient>();

            For<CouchSettings>().Use(new CouchSettings
                {
                    Uri = new Uri("http://127.0.0.1:5984/sabatoast")
                });
        }
    }
}
