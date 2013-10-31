using System;
using FubuCore;
using Quartz.Impl.Triggers;

namespace sabatoast_puller.Quartz.Triggers
{
    public class HalfMinuteTrigger : CronTriggerImpl, IHalfMinuteTrigger
    {
        public HalfMinuteTrigger() : this(0) { }

        public HalfMinuteTrigger(int onSecond) : base("HalfMinuteTrigger." + Guid.NewGuid())
        {
            OnSecond(onSecond);
        }

        public void OnSecond(int second)
        {
            if (0 > second || second >= 30)
                throw new ArgumentException("Must be between 0-29", "second");

            CronExpressionString = "{0}/30 * * * * ?".ToFormat(second);
        }
    }
}