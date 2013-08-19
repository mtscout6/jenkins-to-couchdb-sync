﻿using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using StructureMap;
using sabatoast_puller.Quartz.Jobs;
using sabatoast_puller.Quartz.Triggers;
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
                    x.AddRegistry<QuartzRegistry>();
                    x.AddRegistry<JenkinsRegistry>();
                    x.AddRegistry<SabatoastRegistry>();
                });
        }

        public void Start()
        {
            var scheduler = _container.GetInstance<IScheduler>();
            scheduler.Start();

            scheduler.ScheduleHourlyAndTrigger<Root>(_container);
        }

        public void Stop()
        {
            _container.GetInstance<IScheduler>().Shutdown();
        }
    }
}