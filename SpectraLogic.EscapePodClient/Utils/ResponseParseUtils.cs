using System;
using System.IO;
using System.Linq;
using System.Net;
using SpectraLogic.EscapePodClient.Runtime;

namespace SpectraLogic.EscapePodClient.Utils
{
    internal static class ResponseParseUtils
    {
        internal static void HandleStatusCode(IHttpWebResponse response, params HttpStatusCode[] expectedStatusCodes)
        {
            var actualStatusCode = response.StatusCode;
            if (expectedStatusCodes.Contains(actualStatusCode))
            {
                return;
            }

            var responseContent = GetResponseContent(response);
            throw new Exception(responseContent);
        }

        private static string GetResponseContent(IHttpWebResponse response)
        {
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}
