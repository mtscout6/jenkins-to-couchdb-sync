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

    public class CouchDocument<T> : ICouchDocument<T>
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

    public static class CouchDocumentExtensions
    {
        public static ICouchDocument<T> ToCouchDoc<T>(this T obj)
        {
            var doc = new CouchDocument<T>
                {
                    _data = obj
                };

            // TODO: Figure out id gen

            return doc;
        }
    }
}