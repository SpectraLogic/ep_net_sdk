/*
 * ******************************************************************************
 *   Copyright 2014-2018 Spectra Logic Corporation. All Rights Reserved.
 *   Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *   this file except in compliance with the License. A copy of the License is located at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 *   or in the "license" file accompanying this file.
 *   This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *   CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *   specific language governing permissions and limitations under the License.
 * ****************************************************************************
 */

using System;
using log4net;
using SpectraLogic.EscapePodClient.Runtime;

namespace SpectraLogic.EscapePodClient
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="SpectraLogic.EscapePodClient.IEscapePodClientBuilder" />
    public class EscapePodClientBuilder : IEscapePodClientBuilder
    {
        private static readonly ILog Log = LogManager.GetLogger("EscapePodClientBuilder");

        private readonly string _serverName;
        private readonly int _serverPort;
        private readonly string _username;
        private readonly string _password;
        private Uri _proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="EscapePodClientBuilder"/> class.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="serverPort">The server port.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public EscapePodClientBuilder(string serverName, int serverPort, string username, string password)
        {
            _serverName = serverName;
            _serverPort = serverPort;
            _username = username;
            _password = password;
        }

        /// <summary>
        /// Withes the proxy.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <returns></returns>
        public EscapePodClientBuilder WithProxy(string proxy)
        {
            _proxy = new Uri(proxy);
            return this;
        }

        /// <summary>
        /// Withes the proxy.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <returns></returns>
        public EscapePodClientBuilder WithProxy(Uri proxy)
        {
            _proxy = proxy;
            return this;
        }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns></returns>
        public IEscapePodClient Build()
        {
            var network = new Network(_serverName, _serverPort, _username, _password, _proxy);
            return new EscapePodClient(network);
        }
    }
}
