using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using Quartz.Spi;
using StructureMap.Configuration.DSL;
using sabatoast_puller.Quartz;
using sabatoast_puller.Quartz.Schedulers;
using sabatoast_puller.Quartz.Triggers;

namespace sabatoast_puller.Registries
{
    public class QuartzRegistry : Registry
    {
        public QuartzRegistry()
        {
            For<IJobFactory>().Use<StructureMapSchedulerJobFactory>();
            For<IThreadPool>().Singleton().Use(new SimpleThreadPool());
            For<IJobStore>().Singleton().Use<RAMJobStore>();

            For<ISchedulerFactory>().Singleton().Use(context =>
                {
                    var factory = DirectSchedulerFactory.Instance;

                    var threadPool = context.GetInstance<IThreadPool>();
                    var jobStore = context.GetInstance<IJobStore>();

                    factory.CreateScheduler(threadPool, jobStore);
                    return factory;
                });

            For<IScheduler>().Singleton().Use(context =>
                {
                    var scheduler = context.GetInstance<ISchedulerFactory>().GetScheduler();
                    scheduler.JobFactory = context.GetInstance<IJobFactory>();
                    return scheduler;
                });

            For<IRootJobScheduler>().Use<RootJobScheduler>();
            For<IJenkinsJobScheduler>().Use<JenkinsJobScheduler>();

            For<IHourlyTrigger>().AlwaysUnique().Use(ctx => new HourlyTrigger());
            For<IHalfMinuteTriggerBuilder>().Use<HalfMinuteTriggerBuilder>();
        }
    }
}