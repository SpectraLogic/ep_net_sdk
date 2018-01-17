using log4net;
using SpectraLogic.EscapePodClient.Runtime;

namespace SpectraLogic.EscapePodClient
{
    public class EscapePodClientBuilder : IEscapePodClientBuilder
    {
        private static readonly ILog Log = LogManager.GetLogger("EscapePodClientBuilder");

        private readonly string _serverName;
        private readonly int _serverPort;
        private readonly string _username;
        private readonly string _password;
        private string _proxy;

        public EscapePodClientBuilder(string serverName, int serverPort, string username, string password)
        {
            _serverName = serverName;
            _serverPort = serverPort;
            _username = username;
            _password = password;
        }

        public EscapePodClientBuilder WithProxy(string proxy)
        {
            _proxy = proxy;
            return this;
        }

        public IEscapePodClient Build()
        {
            var networkBuilder = new NetworkBuilder(_serverName, _serverPort, _username, _password);
            if (_proxy != null)
            {
                networkBuilder.WithProxy(_proxy);
            }

            return new EscapePodClient(networkBuilder.Build());
        }
    }
}
