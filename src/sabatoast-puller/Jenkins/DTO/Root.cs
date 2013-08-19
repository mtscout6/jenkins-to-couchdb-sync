using System.Collections.Generic;

namespace sabatoast_puller.Jenkins.DTO
{
    public class Root
    {
        public List<RootJob> Jobs { get; set; }
    }

    public class RootJob
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}