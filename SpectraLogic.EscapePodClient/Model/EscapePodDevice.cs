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

using Newtonsoft.Json;

namespace SpectraLogic.EscapePodClient.Model
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.EscapePodClient.Model.IEscapePodDevice" />
    public class EscapePodDevice : IEscapePodDevice
    {
        #region Constructors

        [JsonConstructor]
        private EscapePodDevice(string deviceName, string mgmtInterface, string username)
        {
            DeviceName = deviceName;
            MgmtInterface = mgmtInterface;
            Username = username;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the name of the archive.
        /// </summary>
        /// <value>
        /// The name of the archive.
        /// </value>
        [JsonProperty(Order = 1, PropertyName = "name")] public string DeviceName { get; }

        /// <summary>
        /// Gets the MGMT interface.
        /// </summary>
        /// <value>
        /// The MGMT interface.
        /// </value>
        [JsonProperty(Order = 2, PropertyName = "mgmtInterface")] public string MgmtInterface { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [JsonProperty(Order = 3, PropertyName = "username")] public string Username { get; }

        #endregion Properties
    }
}