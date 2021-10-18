using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using System.IO;

using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace ProjectCS
{
    public class JsonHelper
    {
        public void Deserialize()
        {
            var myJsonString = File.ReadAllText(@"../../../Data/data.json");
            var myJsonObject = JsonConvert.DeserializeObject<List<Client>>(myJsonString);
        }

        public static T DeserializeFromJson<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static string SerializeToJson<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}