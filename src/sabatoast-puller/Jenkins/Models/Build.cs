﻿using System.Collections.Generic;
using sabatoast_puller.Couch;
using FubuCore;

namespace sabatoast_puller.Jenkins.Models
{
    public class Build : CouchDocument<Build>
    {
        public override string _id
        {
            get { return "job[{0}][{1}]".ToFormat(Job, Number); }
            set
            {
                // NO-OP
            }
        }

        public string Job { get; set; }
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