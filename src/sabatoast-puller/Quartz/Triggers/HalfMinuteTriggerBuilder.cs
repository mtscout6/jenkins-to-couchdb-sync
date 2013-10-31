namespace sabatoast_puller.Quartz.Triggers
{
    public class HalfMinuteTriggerBuilder : IHalfMinuteTriggerBuilder
    {
        private int _second;

        public IHalfMinuteTrigger Build(string group)
        {
            var trigger = new HalfMinuteTrigger(group, _second);

            _second++;

            if (_second >= 30)
            {
                _second = 0;
            }

            return trigger;
        }
    }
}