using FubuCore;
using Quartz.Impl.Triggers;

namespace sabatoast_puller.Quartz.Triggers
{
    public class HourlyTrigger : CronTriggerImpl, IHourlyTrigger
    {
        public HourlyTrigger() : this(0) { }

        public HourlyTrigger(int onMinute) : base("Hourly Trigger")
        {
            CronExpressionString = "0 {0} * * * ?".ToFormat(onMinute);
        }
    }
}