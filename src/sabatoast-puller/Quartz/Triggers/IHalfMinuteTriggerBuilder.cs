namespace sabatoast_puller.Quartz.Triggers
{
    public interface IHalfMinuteTriggerBuilder
    {
        IHalfMinuteTrigger Build(string group);
    }
}