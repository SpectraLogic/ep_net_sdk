using SpectraLogic.EscapePodClient.Calls;

namespace SpectraLogic.EscapePodClient.Runtime
{
    internal interface INetwork
    {
        IHttpWebResponse Invoke(RestRequest request);
    }
}
