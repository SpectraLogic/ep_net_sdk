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

        public MockHttpWebResponse(string resourceName, HttpStatusCode statusCode)
        {
            _resourceName = resourceName;
            StatusCode = statusCode;
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
    }
}
