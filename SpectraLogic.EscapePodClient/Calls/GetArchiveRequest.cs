using System.Linq;

namespace SpectraLogic.EscapePodClient.Calls
{
    public class GetArchiveRequest : RestRequest
    {
        public GetArchiveRequest(string archiveName)
        {
            QueryParams.Add("name", archiveName);
        }

        internal override HttpVerb Verb => HttpVerb.GET;
        internal override string Path => "api/getarchive"; //TODO use the right path
        public override string ToString()
        {
            return $"{Path}?{string.Join(";", QueryParams.Select(q => q.Key + "=" + q.Value))}\n{Verb}";
        }
    }
}
