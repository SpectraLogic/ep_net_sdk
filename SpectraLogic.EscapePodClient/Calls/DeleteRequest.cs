using System.Collections.Generic;
using System.Runtime.Serialization;
using SpectraLogic.EscapePodClient.Model;
using SpectraLogic.EscapePodClient.Utils;

namespace SpectraLogic.EscapePodClient.Calls
{
    [DataContract]
    public class DeleteRequest : RestRequest
    {
        [DataMember] public IEnumerable<DeleteFile> Files;

        public DeleteRequest() { }

        public DeleteRequest(IEnumerable<DeleteFile> files)
        {
            Files = files;
        }

        internal override HttpVerb Verb => HttpVerb.DELETE;
        internal override string Path => "api/delete"; //TODO use the right path
        public override string ToString()
        {
            return $"{Path}\n{Verb}\n{GetBody()}";
        }

        internal override string GetBody()
        {
            return HttpUtils<DeleteRequest>.ObjectToJson(this);
        }
    }
}