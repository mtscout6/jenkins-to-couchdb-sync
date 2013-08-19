using System.Collections.Generic;

namespace sabatoast_puller.Jenkins.DTO
{
    public class Build
    {
        public List<IBuildActions> Actions { get; set; }

        public bool Building { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int EstimatedDuration { get; set; }
        public string Executor { get; set; }
        public string Id { get; set; }
        public int Number { get; set; }
        public string Result { get; set; }
        public string Url { get; set; }
    }

    public interface IBuildActions{}

    public class BuildParameters : IBuildActions
    {
        public List<BuildParameter> Parameters { get; set; }
    }

    public class BuildParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}