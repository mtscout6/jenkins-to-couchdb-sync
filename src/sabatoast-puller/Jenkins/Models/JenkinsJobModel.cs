using System.Collections.Generic;
using sabatoast_puller.Couch;
using FubuCore;

namespace sabatoast_puller.Jenkins.Models
{
    public class JenkinsJobModel : CouchDocument<JenkinsJobModel>
    {
        public override string _id
        {
            get
            {
                return "job[{0}]".ToFormat(Name);
            }
            set
            {
                // NO-OP
            }
        }

        public string Name { get; set; }

        public List<JobBuild> Builds { get; set; }
        public JobBuild LastBuild { get; set; }
    }

    public class JobBuild
    {
        public int Number { get; set; }
        public string Url { get; set; }
    }
}