using RestSharp;
using RestSharp.Deserializers;

namespace sabatoast_puller.Utils.Json
{
    public class NewtonsoftJsonDeserializer : IDeserializer
    {
        public T Deserialize<T>(IRestResponse response)
        {
            return JsonCamelCaseReader.FromJson<T>(response.RawBytes);
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
    }
}