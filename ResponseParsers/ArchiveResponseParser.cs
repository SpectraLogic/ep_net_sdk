using System;
using System.Net;
using SpectraLogic.EscapePodClient.Model;
using SpectraLogic.EscapePodClient.Runtime;
using SpectraLogic.EscapePodClient.Utils;

namespace SpectraLogic.EscapePodClient.ResponseParsers
{
    internal class ArchiveResponseParser : IResponseParser<IEscapePodJob>
    {
        public IEscapePodJob Parse(IHttpWebResponse response)
        {
            using (response)
            {
                ResponseParseUtils.HandleStatusCode(response, HttpStatusCode.OK); //TODO use the right status code
                using (var stream = response.GetResponseStream())
                {
                    throw new NotImplementedException("Need to parse into IEscapePodJob");
                }
            }
        }
    }
}
