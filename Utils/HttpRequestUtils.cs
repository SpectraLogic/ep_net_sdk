using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SpectraLogic.EscapePodClient.Utils
{
    internal static class HttpRequestUtils<T>
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
    }
}
