using SpectraLogic.EscapePodClient.Runtime;

namespace SpectraLogic.EscapePodClient.ResponseParsers
{
    internal interface IResponseParser<out TResponse>
    {
        TResponse Parse(IHttpWebResponse response);
    }
}
