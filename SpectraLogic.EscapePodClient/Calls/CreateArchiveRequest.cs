using System.Runtime.Serialization;
using SpectraLogic.EscapePodClient.Utils;

namespace SpectraLogic.EscapePodClient.Calls
{
    [DataContract]
    public class CreateArchiveRequest : RestRequest
    {
        [DataMember] public string Name;

        public CreateArchiveRequest() { }

        public CreateArchiveRequest(string name)
        {
            Name = name;
        }

        internal override HttpVerb Verb => HttpVerb.POST;
        internal override string Path => "api/createarchive"; //TODO use the right path
        public override string ToString()
        {
            return $"{Path}\n{Verb}\n{GetBody()}";
        }

        internal override string GetBody()
        {
            return HttpUtils<CreateArchiveRequest>.ObjectToJson(this);
        }
    }
}
