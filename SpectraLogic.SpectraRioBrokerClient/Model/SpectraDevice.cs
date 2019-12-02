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
    /// <seealso cref="ISpectraDevice" />
    public class SpectraDevice : ISpectraDevice
    {
        #region Constructors

        [JsonConstructor]
        private SpectraDevice(string deviceName, string mgmtInterface, string username)
        {
            DeviceName = deviceName;
            MgmtInterface = mgmtInterface;
            Username = username;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        /// <value>
        /// The name of the device.
        /// </value>
        [JsonProperty(PropertyName = "name")] public string DeviceName { get; }

        /// <summary>
        /// Gets the MGMT interface.
        /// </summary>
        /// <value>
        /// The MGMT interface.
        /// </value>
        [JsonProperty(PropertyName = "mgmtInterface")] public string MgmtInterface { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [JsonProperty(PropertyName = "username")] public string Username { get; }

        #endregion Properties
    }
}