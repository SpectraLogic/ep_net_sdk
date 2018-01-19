using System.Linq;

namespace SpectraLogic.EscapePodClient.Calls
{
    public class GetEscapePodJobStatus : RestRequest
    {
        public GetEscapePodJobStatus(string escapePodJobId)
        {
            AddQueryParam("id", escapePodJobId);
        }

        public GetEscapePodJobStatus(long escapePodJobId)
        {
            AddQueryParam("id", escapePodJobId.ToString());
        }

        internal override HttpVerb Verb => HttpVerb.GET;
        internal override string Path => "api/jobstatus"; //TODO use the right path
        public override string ToString()
        {
            return $"{Path}?{string.Join(";", QueryParams.Select(q => q.Key + "=" + q.Value))}\n{Verb}";
        }
    }
}
