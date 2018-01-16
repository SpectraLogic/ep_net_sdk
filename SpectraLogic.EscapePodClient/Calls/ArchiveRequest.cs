using System.Collections.Generic;
using System.Runtime.Serialization;
using SpectraLogic.EscapePodClient.Model;
using SpectraLogic.EscapePodClient.Utils;

namespace SpectraLogic.EscapePodClient.Calls
{
    [DataContract]
    public class ArchiveRequest: RestRequest
    {
        [DataMember] public IEnumerable<ArchiveFile> Files;

        public ArchiveRequest(){}

        public ArchiveRequest(IEnumerable<ArchiveFile> files)
        {
            Files = files;
        }

        internal override HttpVerb Verb => HttpVerb.POST;
        internal override string Path => "api/archive"; //TODO use the right path
        public override string ToString()
        {
            return $"{Path}\n{Verb}\n{GetBody()}";
        }

        internal override string GetBody()
        {
            return HttpUtils<ArchiveRequest>.ObjectToJson(this);
        }
    }
}
