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
using SpectraLogic.EscapePodClient.Utils;
using System;

namespace SpectraLogic.EscapePodClient.Calls
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.EscapePodClient.Calls.RestRequest" />
    public class CreateDeviceRequest : RestRequest
    {
        #region Fields

        /// <summary>
        /// The MGMT interface
        /// </summary>
        [JsonProperty(Order = 2, PropertyName = "mgmtInterface")] public string MgmtInterface;

        /// <summary>
        /// The name
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "name")] public string Name;

        /// <summary>
        /// The password
        /// </summary>
        [JsonProperty(Order = 4, PropertyName = "password")] public string Password;

        /// <summary>
        /// The username
        /// </summary>
        [JsonProperty(Order = 3, PropertyName = "username")] public string Username;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDeviceRequest" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="mgmtInterface">The MGMT interface.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public CreateDeviceRequest(string name, string mgmtInterface, string username, string password)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(mgmtInterface != null, "mgmtInterface");
            Contract.Requires<ArgumentNullException>(username != null, "username");
            Contract.Requires<ArgumentNullException>(password != null, "password");

            Name = name;
            MgmtInterface = mgmtInterface;
            Username = username;
            Password = password;
        }

        #endregion Constructors

        #region Properties

        internal override string Path => $"/api/devices/spectra";
        internal override HttpVerb Verb => HttpVerb.POST;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{Path}\n{Verb}\n{GetBody()}";
        }

        internal override string GetBody()
        {
            return JsonConvert.SerializeObject(this);
        }

        #endregion Methods
    }
}