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

using log4net;
using SpectraLogic.SpectraRioBrokerClient.Runtime;
using System;

namespace SpectraLogic.SpectraRioBrokerClient
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.ISpectraRioBrokerClientBuilder" />
    public class SpectraRioBrokerClientBuilder : ISpectraRioBrokerClientBuilder
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger("SpectraRioBrokerClientBuilder");

        private readonly string _serverName;
        private readonly int _serverPort;
        private readonly string _token = null;
        private Uri _proxy;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectraRioBrokerClientBuilder" /> class.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="serverPort">The server port.</param>
        /// <param name="token">The token.</param>
        public SpectraRioBrokerClientBuilder(string serverName, int serverPort, string token)
        {
            _serverName = serverName;
            _serverPort = serverPort;
            _token = token;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectraRioBrokerClientBuilder"/> class.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="serverPort">The server port.</param>
        public SpectraRioBrokerClientBuilder(string serverName, int serverPort)
        {
            _serverName = serverName;
            _serverPort = serverPort;
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        public ISpectraRioBrokerClient Build()
        {
            var network = new Network(_serverName, _serverPort, _token, _proxy);
            return new SpectraRioBrokerClient(network);
        }

        /// <summary>
        /// Withes the proxy.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <returns></returns>
        public SpectraRioBrokerClientBuilder WithProxy(string proxy)
        {
            _proxy = new Uri(proxy);
            return this;
        }

        /// <summary>
        /// Withes the proxy.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <returns></returns>
        public SpectraRioBrokerClientBuilder WithProxy(Uri proxy)
        {
            _proxy = proxy;
            return this;
        }

        #endregion Methods
    }
}