using RestSharp.Serializers;

namespace sabatoast_puller.Utils.Json
{
    public class RawJsonSerializer : ISerializer
    {
        public string Serialize(object obj)
        {
            return obj.ToString();
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
        public string ContentType { get; set; }
    }
}