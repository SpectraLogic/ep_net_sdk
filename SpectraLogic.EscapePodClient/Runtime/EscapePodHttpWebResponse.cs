using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace SpectraLogic.EscapePodClient.Runtime
{
    internal class EscapePodHttpWebResponse : IHttpWebResponse
    {
        private HttpWebResponse Response { get; }

        public EscapePodHttpWebResponse(HttpWebResponse httpWebResponse)
        {
            Response = httpWebResponse;
        }
        public Stream GetResponseStream()
        {
            return Response.GetResponseStream();
        }

        public HttpStatusCode StatusCode => Response.StatusCode;

        public IDictionary<string, IEnumerable<string>> Headers => Response.Headers.AllKeys.ToDictionary<string, string, IEnumerable<string>>(key => key, key => Response.Headers.GetValues(key));

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
