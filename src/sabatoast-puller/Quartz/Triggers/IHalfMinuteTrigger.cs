using Quartz;

namespace sabatoast_puller.Quartz.Triggers
{
    public interface IHalfMinuteTrigger : ITrigger
    {
        void OnSecond(int second);
    }
}