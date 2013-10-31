using System;
using System.Collections.Generic;
using sabatoast_puller.Couch;
using FubuCore;

namespace sabatoast_puller.Jenkins.Models
{
    public class RootModel : CouchDocument<RootModel>
    {
        private const string Id = "ROOT";

        public override string _id
        {
            get { return Id; }
            set
            {
                if (value != Id)
                {
                    throw new InvalidOperationException("The id for this must be {0}".ToFormat(Id));
                }
            }
        }

        public List<RootJob> Jobs { get; set; }
    }

    public class RootJob
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}