using System;
using System.IO;
using System.Net;

namespace SpectraLogic.EscapePodClient.Runtime
{
    public interface IHttpWebResponse : IDisposable
    {
        Stream GetResponseStream();
        HttpStatusCode StatusCode { get; }
    }
}
