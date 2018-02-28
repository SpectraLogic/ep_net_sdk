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
using SpectraLogic.SpectraStorageBrokerClient.Utils;
using System;

namespace SpectraLogic.SpectraStorageBrokerClient.Calls
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraStorageBrokerClient.Calls.RestRequest" />
    public class GetDeviceRequest : RestRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetDeviceRequest"/> class.
        /// </summary>
        /// <param name="deviceName">Name of the device.</param>
        public GetDeviceRequest(string deviceName)
        {
            Contract.Requires<ArgumentNullException>(deviceName != null, "deviceName");

            DeviceName = deviceName;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        /// <value>
        /// The name of the device.
        /// </value>
        [JsonProperty(PropertyName = "name")] public string DeviceName { get; private set; }

        internal override string Path => $"/api/devices/spectra/{DeviceName}";
        internal override HttpVerb Verb => HttpVerb.GET;

        #endregion Properties
    }
}