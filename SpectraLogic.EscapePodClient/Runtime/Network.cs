using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using log4net;
using SpectraLogic.EscapePodClient.Calls;

namespace SpectraLogic.EscapePodClient.Runtime
{
    internal class Network : INetwork
    {
        private static readonly ILog Log = LogManager.GetLogger("Network");

        private string HostServerName { get; }
        private int HostServerPort { get; }
        private string Username { get; }
        private string Password { get; }
        private Uri Proxy { get; }

        public Network(string hostServerName, int hostServerPort, string username, string password, Uri proxy = null)
        {
            HostServerName = hostServerName;
            HostServerPort = hostServerPort;
            Username = username;
            Password = password;
            Proxy = proxy;
        }

        public IHttpWebResponse Invoke(RestRequest request)
        {
            var httpWebRequest = CreateHttpWebRequest(request);

            try
            {
                return httpWebRequest.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Response == null)
                {
                    throw;
                }
                return new EscapePodHttpWebResponse((HttpWebResponse)e.Response);
            }
        }

        private IHttpWebRequest CreateHttpWebRequest(RestRequest request)
        {
            var uriBuilder = new UriBuilder(HostServerName)
            {
                Path = Uri.EscapeDataString(request.Path),
                Query = BuildQueryParams(request.QueryParams),
                Port = HostServerPort
            };


            var httpRequest = (HttpWebRequest) WebRequest.Create(uriBuilder.Uri);
            httpRequest.Method = request.Verb.ToString();
            httpRequest.Headers.Add("Authorization", GetBasicAuth());
            httpRequest.ContentType = "application/json";

            if (Proxy != null)
            {
                var webProxy = new WebProxy
                {
                    Address = Proxy
                };
                httpRequest.Proxy = webProxy;
            }

            httpRequest.Date = DateTime.UtcNow;
            httpRequest.UserAgent = "Spectra Avid Plugin - EscapePod Client";

            if (request.Verb == HttpVerb.PUT || request.Verb == HttpVerb.POST)
            {
                var body = Encoding.UTF8.GetBytes(request.GetBody());
                httpRequest.ContentLength = body.Length;

                using (var requestStream = httpRequest.GetRequestStream())
                {
                    requestStream.Write(body, 0, body.Length);
                }
            }

            return new EscapePodHttpWebRequest(httpRequest);
        }

        private string GetBasicAuth()
        {
            var credentials = $"{Username}:{Password}";
            var bytes = Encoding.ASCII.GetBytes(credentials);
            var base64 = Convert.ToBase64String(bytes);
            var authorization = string.Concat("Basic ", base64);
            return authorization;
        }

        private static string BuildQueryParams(Dictionary<string, string> queryParams)
        {
            return string.Join(
                "&",
                from kvp in queryParams
                orderby kvp.Key
                let encodedKey = Uri.EscapeDataString(kvp.Key)
                select !string.IsNullOrEmpty(kvp.Value)
                    ? encodedKey + "=" + Uri.EscapeDataString(kvp.Value)
                    : encodedKey
            );
        }
    }
}
