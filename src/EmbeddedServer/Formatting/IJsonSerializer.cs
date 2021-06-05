using System;
using System.Collections.Generic;
using System.Text;

namespace EmbeddedServer.Formatting
{
    public interface IJsonSerializer
    {
        string Serialize(object obj);
        object Deserialize(string json);
        object Deserialize(string json, Type type);
        T Deserialize<T>(string json);
    }
}
