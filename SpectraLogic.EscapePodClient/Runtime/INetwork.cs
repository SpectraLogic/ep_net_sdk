using System;
using SpectraLogic.EscapePodClient.Calls;

namespace SpectraLogic.EscapePodClient.Runtime
{
    public interface INetwork
    {
        INetwork WithProxy(Uri proxy);
        IHttpWebResponse Invoke(RestRequest request);
    }
}
