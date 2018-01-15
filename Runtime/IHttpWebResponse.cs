using System;
using System.IO;
using System.Net;

namespace SpectraLogic.EscapePodClient.Runtime
{
    internal interface IHttpWebResponse : IDisposable
    {
        Stream GetResponseStream();
        HttpStatusCode StatusCode { get; }
    }
}
