using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SpectraLogic.EscapePodClient.Utils
{
    internal static class HttpUtils<T> where T : new()
    {
        internal static string ObjectToJson(T request)
        {
            var ms = new MemoryStream();
            var ser = new DataContractJsonSerializer(typeof(T));
            ser.WriteObject(ms, request);
            var json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        internal static T JsonToObject(string json)
        {
            var deserializedObject = new T();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var ser = new DataContractJsonSerializer(deserializedObject.GetType());
            deserializedObject = (T) ser.ReadObject(ms);
            ms.Close();
            return deserializedObject;
        }
    }
}
