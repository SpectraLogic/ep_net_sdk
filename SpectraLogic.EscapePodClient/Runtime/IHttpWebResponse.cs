using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SpectraLogic.EscapePodClient.Runtime
{
    internal interface IHttpWebResponse : IDisposable
    {
        Stream GetResponseStream();
        HttpStatusCode StatusCode { get; }
        IDictionary<string, IEnumerable<string>> Headers { get; }
    }
}
