namespace sabatoast_puller.Quartz.Triggers
{
    public class HalfMinuteTriggerBuilder : IHalfMinuteTriggerBuilder
    {
        private int _second;

        public IHalfMinuteTrigger Build()
        {
            var trigger = new HalfMinuteTrigger(_second);

            _second++;

            if (_second >= 30)
            {
                _second = 0;
            }

            return trigger;
        }
    }
}