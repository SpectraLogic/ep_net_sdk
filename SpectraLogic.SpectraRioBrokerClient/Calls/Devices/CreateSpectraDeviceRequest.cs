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
using SpectraLogic.SpectraRioBrokerClient.Utils;
using SpectraLogic.SpectraRioBrokerClient.Utils.JsonConverters;
using System;

namespace SpectraLogic.SpectraRioBrokerClient.Calls.Devices
{
    /// <summary>
    /// ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Calls.RestRequest"/>
    public class CreateSpectraDeviceRequest : RestRequest
    {
        #region Public Fields

        /// <summary>
        /// The device name
        /// </summary>
        [JsonProperty(PropertyName = "name")] public string DeviceName;

        /// <summary>
        /// The MGMT interface
        /// </summary>
        [JsonProperty(PropertyName = "mgmtInterface")]
        [JsonConverter(typeof(UriJsonConverter))]
        public Uri MgmtInterface;

        /// <summary>
        /// The password
        /// </summary>
        [JsonProperty(PropertyName = "password")] public string Password;

        /// <summary>
        /// The username
        /// </summary>
        [JsonProperty(PropertyName = "username")] public string Username;

        #endregion Public Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSpectraDeviceRequest"/> class.
        /// </summary>
        /// <param name="deviceName">The device name.</param>
        /// <param name="mgmtInterface">The MGMT interface.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CreateSpectraDeviceRequest(string deviceName, Uri mgmtInterface, string username, string password)
        {
            Contract.Requires<ArgumentNullException>(deviceName != null, "deviceName");
            Contract.Requires<ArgumentNullException>(mgmtInterface != null, "mgmtInterface");
            Contract.Requires<ArgumentNullException>(username != null, "username");
            Contract.Requires<ArgumentNullException>(password != null, "password");

            DeviceName = deviceName;
            MgmtInterface = mgmtInterface;
            Username = username;
            Password = password;
        }

        #endregion Public Constructors

        #region Internal Properties

        internal override string Path => "/api/devices/spectra";
        internal override HttpVerb Verb => HttpVerb.POST;

        #endregion Internal Properties

        #region Internal Methods

        internal override string GetBody()
        {
            return JsonConvert.SerializeObject(this);
        }

        #endregion Internal Methods
    }
}