﻿using System.Collections.Generic;

namespace sabatoast_puller.Jenkins.DTO
{
    public class Job
    {
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Buildable { get; set; }

        public List<JobBuild> Builds { get; set; }
        public JobBuild LastBuild { get; set; }

        public bool InQueue { get; set; }

        public List<JobDownstreamJob> DownstreamProjects { get; set; }
    }

    public class JobBuild
    {
        public int Number { get; set; }
        public string Url { get; set; }
    }

    public class JobDownstreamJob
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}