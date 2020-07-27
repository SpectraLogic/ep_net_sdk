/*
 * ******************************************************************************
 *   Copyright 2014-2020 Spectra Logic Corporation. All Rights Reserved.
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
using SpectraLogic.SpectraRioBrokerClient.Utils;

namespace SpectraLogic.SpectraRioBrokerClient.Calls.Devices
{
    /// <summary></summary>
    public class DeleteSpectraDeviceRequest : RestRequest
    {
        #region Fields

        /// <summary>The device name</summary>
        public readonly string DeviceName;

        #endregion Fields

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="DeleteSpectraDeviceRequest"/> class.</summary>
        /// <param name="deviceName">Name of the device.</param>
        public DeleteSpectraDeviceRequest(string deviceName)
        {
            Contract.Requires<ArgumentNullException>(deviceName != null, "deviceName");

            DeviceName = deviceName;
        }

        #endregion Constructors

        #region Properties

        internal override string Path => $"/api/devices/spectra/{DeviceName}";
        internal override HttpVerb Verb => HttpVerb.DELETE;

        #endregion Properties
    }
}