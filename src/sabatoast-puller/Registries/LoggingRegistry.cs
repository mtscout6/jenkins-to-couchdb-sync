using Common.Logging;
using StructureMap.Configuration.DSL;

namespace sabatoast_puller.Registries
{
    public class LoggingRegistry : Registry
    {
        public LoggingRegistry()
        {
            For<ILog>()
                .AlwaysUnique()
                .Use(ctx => LogManager.GetLogger(ctx.ParentType ?? ctx.BuildStack.Current.ConcreteType));
        }
    }
}