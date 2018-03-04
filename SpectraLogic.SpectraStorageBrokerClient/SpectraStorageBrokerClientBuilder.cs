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
using SpectraLogic.SpectraStorageBrokerClient.Runtime;
using System;

namespace SpectraLogic.SpectraStorageBrokerClient
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraStorageBrokerClient.ISpectraStorageBrokerClientBuilder" />
    public class SpectraStorageBrokerClientBuilder : ISpectraStorageBrokerClientBuilder
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger("SpectraStorageBrokerClientBuilder");

        private readonly string _password;
        private readonly string _serverName;
        private readonly int _serverPort;
        private readonly string _username;
        private Uri _proxy;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectraStorageBrokerClientBuilder"/> class.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="serverPort">The server port.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public SpectraStorageBrokerClientBuilder(string serverName, int serverPort, string username, string password)
        {
            _serverName = serverName;
            _serverPort = serverPort;
            _username = username;
            _password = password;
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        public ISpectraStorageBrokerClient Build()
        {
            var network = new Network(_serverName, _serverPort, _username, _password, _proxy);
            return new SpectraStorageBrokerClient(network);
        }

        /// <summary>
        /// Withes the proxy.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <returns></returns>
        public SpectraStorageBrokerClientBuilder WithProxy(string proxy)
        {
            _proxy = new Uri(proxy);
            return this;
        }

        /// <summary>
        /// Withes the proxy.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <returns></returns>
        public SpectraStorageBrokerClientBuilder WithProxy(Uri proxy)
        {
            _proxy = proxy;
            return this;
        }

        #endregion Methods
    }
}