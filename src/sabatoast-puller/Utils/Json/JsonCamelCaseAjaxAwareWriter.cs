using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace sabatoast_puller.Utils.Json
{
    public class JsonCamelCaseAjaxAwareWriter
    {
        public static string ToJSON(object o)
        {
            var settings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};
            return JsonConvert.SerializeObject(o, Formatting.None, settings);
        }
    }
}