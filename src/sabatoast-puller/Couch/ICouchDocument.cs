using System;

namespace sabatoast_puller.Couch
{
    public interface ICouchDocument
    {
        string _id { get; set; }
        string _rev { get; set; }
        string type { get; set; }
    }

    public abstract class CouchDocument<T> : ICouchDocument, IEquatable<CouchDocument<T>> where T : class
    {
        public virtual string _id { get; set; }

        public string _rev { get; set; }

        public string type
        {
            get { return typeof (T).Name; }
            set
            {
                if (value != typeof(T).Name)
                    throw new InvalidOperationException();
            }
        }

        public bool ShouldSerialize_rev()
        {
            return _rev != null;
        }

        public virtual bool Equals(CouchDocument<T> obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return _id == obj._id &&
                _rev == obj._rev &&
                type == obj.type;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CouchDocument<T>);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_id != null ? _id.GetHashCode() : 0)*397) ^ (_rev != null ? _rev.GetHashCode() : 0);
            }
        }
    }
}