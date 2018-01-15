using System;
using SpectraLogic.EscapePodClient.Calls;

namespace SpectraLogic.EscapePodClient.Runtime
{
    internal interface INetwork
    {
        INetwork WithProxy(Uri proxy);
        IHttpWebResponse Invoke(RestRequest request);
    }
}
