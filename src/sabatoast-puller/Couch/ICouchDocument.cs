using System;

namespace sabatoast_puller.Couch
{
    public interface ICouchDocument
    {
        string _id { get; set; }
        string _rev { get; set; }
        string type { get; set; }
    }

    public abstract class CouchDocument<T> : ICouchDocument
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
    }
}