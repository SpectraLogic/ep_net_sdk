using System.Collections.Generic;
using System.Runtime.Serialization;
using SpectraLogic.EscapePodClient.Model;
using SpectraLogic.EscapePodClient.Utils;

namespace SpectraLogic.EscapePodClient.Calls
{
    [DataContract]
    public class RestoreRequest : RestRequest
    {
        [DataMember] public IEnumerable<RestoreFile> Files;

        public RestoreRequest(){ }

        public RestoreRequest(IEnumerable<RestoreFile> files)
        {
            Files = files;
        }

        internal override HttpVerb Verb => HttpVerb.GET;
        internal override string Path => "api/restore"; //TODO use the right path

        public override string ToString()
        {
            return $"{Path}\n{Verb}\n{GetBody()}";
        }

        internal override string GetBody()
        {
            return HttpUtils<RestoreRequest>.ObjectToJson(this);
        }
    }
}