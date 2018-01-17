using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using SpectraLogic.EscapePodClient.Runtime;

namespace SpectraLogic.EscapePodClient.Test.Mock
{
    public class MockHttpWebResponse: IHttpWebResponse
    {
        private readonly string _resourceName;

        public MockHttpWebResponse(string resourceName, HttpStatusCode statusCode, IDictionary<string, IEnumerable<string>> headers)
        {
            _resourceName = resourceName;
            StatusCode = statusCode;
            Headers = headers;
        }

        public void Dispose()
        {
        }

        public Stream GetResponseStream()
        {
            return string.IsNullOrEmpty(_resourceName) ?
                new MemoryStream(Encoding.UTF8.GetBytes("The HTTP request failed")) :
                Assembly.GetExecutingAssembly().GetManifestResourceStream(_resourceName);
        }

        public HttpStatusCode StatusCode { get; }
        public IDictionary<string, IEnumerable<string>> Headers { get; }
    }
}
