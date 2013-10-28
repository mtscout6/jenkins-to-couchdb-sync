using System;

namespace sabatoast_puller.Couch
{
    public interface ICouchDocument<T>
    {
        string _id { get; set; }
        string _rev { get; set; }
        string _type { get; set; }
        T _data { get; set; }
    }

    public abstract class CouchDocument<T> : ICouchDocument<T>
    {
        public virtual string _id { get; set; }
        public string _rev { get; set; }
        public string _type
        {
            get { return typeof (T).Name; }
            set
            {
                if (value != typeof(T).Name)
                    throw new InvalidOperationException();
            }
        }

        public T _data { get; set; }
    }
}