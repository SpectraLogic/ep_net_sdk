using System.Net;

namespace SpectraLogic.EscapePodClient.Runtime
{
    internal class EscapePodHttpWebRequest : IHttpWebRequest
    {
        private readonly HttpWebRequest _httpWebRequest;

        public EscapePodHttpWebRequest(HttpWebRequest httpWebRequest)
        {
            _httpWebRequest = httpWebRequest;
        }

        public IHttpWebResponse GetResponse()
        {
            return new EscapePodHttpWebResponse((HttpWebResponse)_httpWebRequest.GetResponse());
        }
    }
}
