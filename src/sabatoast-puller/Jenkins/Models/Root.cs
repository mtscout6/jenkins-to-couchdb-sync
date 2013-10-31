using System;
using System.Collections.Generic;
using sabatoast_puller.Couch;
using FubuCore;
using sabatoast_puller.Utils;

namespace sabatoast_puller.Jenkins.Models
{
    public class Root : CouchDocument<Root>, IEquatable<Root>
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

        public override bool Equals(CouchDocument<Root> obj)
        {
            var equals = base.Equals(obj);
            return equals && EqualsInternal(obj as Root);
        }

        public bool Equals(Root obj)
        {
            return Equals(obj as CouchDocument<Root>);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Root);
        }

        bool EqualsInternal(Root obj)
        {
            if (obj == null) return false;

            return Jobs.ListEquals(obj.Jobs);
        }
    }

    public class RootJob : IEquatable<RootJob>
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public bool Equals(RootJob obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Name == obj.Name && Url == obj.Url;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RootJob);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (Url != null ? Url.GetHashCode() : 0);
            }
        }
    }
}