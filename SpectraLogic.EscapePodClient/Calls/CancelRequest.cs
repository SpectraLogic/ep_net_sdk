using System.Linq;

namespace SpectraLogic.EscapePodClient.Calls
{
    public class CancelRequest : RestRequest
    {
        public CancelRequest(string escapePodJobId)
        {
            AddQueryParam("id", escapePodJobId);
        }

        public CancelRequest(long escapePodJobId)
        {
            AddQueryParam("id", escapePodJobId.ToString());
        }

        internal override HttpVerb Verb => HttpVerb.PUT;
        internal override string Path => "api/cancel"; //TODO use the right path
        public override string ToString()
        {
            return $"{Path}?{string.Join(";", QueryParams.Select(q => q.Key + "=" + q.Value))}\n{Verb}";
        }
    }
}
