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

namespace SpectraLogic.EscapePodClient.Calls
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="SpectraLogic.EscapePodClient.Calls.RestRequest" />
    public class CreateDeviceRequest : RestRequest
    {
        /// <summary>
        /// The name
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "name")] public string Name;

        /// <summary>
        /// The endpoint
        /// </summary>
        [JsonProperty(Order = 2, PropertyName = "endpoint")] public string Endpoint;

        /// <summary>
        /// The username
        /// </summary>
        [JsonProperty(Order = 3, PropertyName = "username")] public string Username;

        /// <summary>
        /// The password
        /// </summary>
        [JsonProperty(Order = 4, PropertyName = "password")] public string Password;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDeviceRequest"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public CreateDeviceRequest(string name, string endpoint, string username, string password)
        {
            Name = name;
            Endpoint = endpoint;
            Username = username;
            Password = password;
        }

        internal override HttpVerb Verb => HttpVerb.POST;

        internal override string Path => $"/api/devices/spectra";

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
    }
}
