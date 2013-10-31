using Quartz;
using StructureMap;
using sabatoast_puller.Couch;
using sabatoast_puller.Quartz.Schedulers;
using sabatoast_puller.Registries;

namespace sabatoast_puller
{
    public class Puller
    {
        private readonly Container _container;

        public Puller()
        {
            _container = new Container(x =>
                {
                    x.AddRegistry<LoggingRegistry>();
                    x.AddRegistry<QuartzRegistry>();
                    x.AddRegistry<JenkinsRegistry>();
                    x.AddRegistry<CouchRegistry>();
                });
        }

        public void Start()
        {
            var scheduler = _container.GetInstance<IScheduler>();
            scheduler.Start();

            _container.GetInstance<IRootJobScheduler>().Schedule(scheduler);
        }

        public void Stop()
        {
            _container.GetInstance<IScheduler>().Shutdown(true);
        }
    }
}