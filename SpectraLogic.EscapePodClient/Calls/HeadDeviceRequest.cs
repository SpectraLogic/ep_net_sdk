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

using SpectraLogic.EscapePodClient.Utils;
using System;

namespace SpectraLogic.EscapePodClient.Calls
{
    internal class HeadDeviceRequest : RestRequest
    {
        #region Fields

        private string DeviceName;

        #endregion Fields

        #region Constructors

        public HeadDeviceRequest(string deviceName)
        {
            Contract.Requires<ArgumentNullException>(deviceName != null, "deviceName");

            DeviceName = deviceName;
        }

        #endregion Constructors

        #region Properties

        internal override HttpVerb Verb => HttpVerb.HEAD;

        internal override string Path => $"/api/devices/spectra/{DeviceName}";

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return $"{Path}\n{Verb}";
        }

        #endregion Methods
    }
}