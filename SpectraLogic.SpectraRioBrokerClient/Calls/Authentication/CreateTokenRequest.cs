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

using System;
using Newtonsoft.Json;
using SpectraLogic.SpectraRioBrokerClient.Utils;

namespace SpectraLogic.SpectraRioBrokerClient.Calls.Authentication
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Calls.RestRequest" />
    public class CreateTokenRequest : RestRequest
    {
        #region Public Fields

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
        /// Initializes a new instance of the <see cref="CreateTokenRequest"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public CreateTokenRequest(string username, string password)
        {
            Contract.Requires<ArgumentNullException>(username != null, "username");
            Contract.Requires<ArgumentNullException>(password != null, "password");

            Username = username;
            Password = password;
        }

        #endregion Public Constructors

        #region Internal Properties

        internal override string Path => "/api/tokens";
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