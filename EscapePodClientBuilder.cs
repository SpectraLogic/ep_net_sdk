using System;
using log4net;
using SpectraLogic.EscapePodClient.Runtime;

namespace SpectraLogic.EscapePodClient
{
    public class EscapePodClientBuilder
    {
        private readonly ILog _log = LogManager.GetLogger("EscapePodClientBuilder");

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
            var network = new Network(
                _serverName,
                _serverPort,
                _username,
                _password
                );

            if (_proxy != null)
            {
                network.WithProxy(new Uri(_proxy));
            }

            return new EscapePodClient(network);
        }
    }
}
