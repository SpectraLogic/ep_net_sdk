using System;

namespace SpectraLogic.EscapePodClient.Runtime
{
    internal class NetworkBuilder
    {
        private readonly string _serverName;
        private readonly int _serverPort;
        private readonly string _username;
        private readonly string _password;
        private string _proxy;

        public NetworkBuilder(string serverName, int serverPort, string username, string password)
        {
            _serverName = serverName;
            _serverPort = serverPort;
            _username = username;
            _password = password;
        }

        public NetworkBuilder WithProxy(string proxy)
        {
            _proxy = proxy;
            return this;
        }

        public INetwork Build()
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

            return network;
        }
    }
}
