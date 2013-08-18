using StructureMap.Configuration.DSL;
using sabatoast_puller.Sabatoast;

namespace sabatoast_puller.Registries
{
    public class SabatoastRegistry : Registry
    {
        public SabatoastRegistry()
        {
            For<ISabatoastRestClient>().Singleton().Use<SabatoastRestClient>();
        }
    }
}