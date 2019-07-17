/*
 * ******************************************************************************
 *   Copyright 2014-2019 Spectra Logic Corporation. All Rights Reserved.
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

using Newtonsoft.Json;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public class ClusterMember : IClusterMember
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="ClusterMember"/> class.</summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="clusterPort">The cluster port.</param>
        /// <param name="httpPort">The HTTP port.</param>
        /// <param name="role">The role.</param>
        public ClusterMember(string ipAddress, int clusterPort, int httpPort, string role)
        {
            IpAddress = ipAddress;
            ClusterPort = clusterPort;
            HttpPort = httpPort;
            Role = role;
        }

        #endregion Constructors

        #region Properties

        /// <summary>Gets the cluster port.</summary>
        /// <value>The cluster port.</value>
        [JsonProperty(PropertyName = "clusterPort")] public int ClusterPort { get; private set; }

        /// <summary>Gets the HTTP port.</summary>
        /// <value>The HTTP port.</value>
        [JsonProperty(PropertyName = "httpPort")] public int HttpPort { get; private set; }

        /// <summary>Gets the ip address.</summary>
        /// <value>The ip address.</value>
        [JsonProperty(PropertyName = "ipAddress")] public string IpAddress { get; private set; }

        /// <summary>Gets the role.</summary>
        /// <value>The role.</value>
        [JsonProperty(PropertyName = "role")] public string Role { get; private set; }

        #endregion Properties
    }
}