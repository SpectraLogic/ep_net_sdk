using System;
using System.IO;
using System.Net;

namespace SpectraLogic.EscapePodClient.Runtime
{
    internal class EscapePodHttpWebResponse : IHttpWebResponse
    {
        private HttpWebResponse Response { get; }

        internal EscapePodHttpWebResponse(HttpWebResponse httpWebResponse)
        {
            Response = httpWebResponse;
        }
        public Stream GetResponseStream()
        {
            return Response.GetResponseStream();
        }

        public HttpStatusCode StatusCode => Response.StatusCode;

        public void Dispose()
        {
            Dispose(true);
            Response.Close();
            GC.SuppressFinalize(this);
        }

        private static void Dispose(bool disposing)
        {
        }
    }
}
