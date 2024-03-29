﻿using RestSharp;
using StructureMap.Configuration.DSL;
using sabatoast_puller.Jenkins;

namespace sabatoast_puller.Registries
{
    public class JenkinsRegistry : Registry
    {
        public JenkinsRegistry()
        {
            For<IJenkinsRestClient>().Use<JenkinsRestClient>();
            For<IJenkinsClient>().Use<JenkinsClient>();
        }
    }
}
