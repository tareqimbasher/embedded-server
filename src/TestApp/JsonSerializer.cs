using EmbeddedServer.Formatting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp
{
    public class JsonSerializer : IJsonSerializer
    {
        public object Deserialize(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public object Deserialize(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
