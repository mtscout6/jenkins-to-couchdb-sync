using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace sabatoast_puller.Utils.Json
{
    public class JsonCamelCaseReader
    {
        static readonly JsonSerializerSettings Settings;
        private readonly Stream _data;

        static JsonCamelCaseReader()
        {
            Settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }

        public JsonCamelCaseReader(Stream data)
        {
            _data = data;
        }

        public T Read<T>()
        {
            var serializer = JsonSerializer.Create(Settings);

            var result = serializer.Deserialize<T>(new JsonTextReader(new StreamReader(_data)));

            return result;
        }

        public static T FromJson<T>(string data)
        {
            return FromJson<T>(Encoding.UTF8.GetBytes(data));
        }

        public static T FromJson<T>(byte[] data)
        {
            var serializer = JsonSerializer.Create(Settings);

            var result = serializer.Deserialize<T>(new JsonTextReader(new StreamReader(new MemoryStream(data))));

            return result;
        }
    }
}