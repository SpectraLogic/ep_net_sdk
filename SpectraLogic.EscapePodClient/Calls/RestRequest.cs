using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SpectraLogic.EscapePodClient.Calls
{
    [DataContract]
    public abstract class RestRequest
    {
        internal abstract HttpVerb Verb { get; }

        internal abstract string Path { get; }

        internal Dictionary<string, string> QueryParams { get; } = new Dictionary<string, string>();

        protected void AddQueryParam(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                QueryParams.Add(key, value);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public abstract override string ToString();

        internal virtual string GetBody()
        {
            return "";
        }
    }

    internal enum HttpVerb
    {
        GET,
        PUT,
        POST,
        DELETE
    };
}
