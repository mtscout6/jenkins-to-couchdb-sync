using System.Collections.Generic;

namespace sabatoast_puller.Jenkins.DTO
{
    public class Root
    {
        public Root() { }

        public List<RootJob> jobs { get; set; }
    }

    public class RootJob
    {
        public RootJob() { }

        public string name { get; set; }
        public string url { get; set; }
    }
}