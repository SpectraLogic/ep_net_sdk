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

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public interface IClusterMember
    {
        #region Properties

        /// <summary>Gets the cluster port.</summary>
        /// <value>The cluster port.</value>
        int ClusterPort { get; }

        /// <summary>Gets the HTTP port.</summary>
        /// <value>The HTTP port.</value>
        int HttpPort { get; }

        /// <summary>Gets the ip address.</summary>
        /// <value>The ip address.</value>
        string IpAddress { get; }

        /// <summary>Gets the role.</summary>
        /// <value>The role.</value>
        string Role { get; }

        #endregion Properties
    }
}