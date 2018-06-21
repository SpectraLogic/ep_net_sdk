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

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public class AgentConfig
    {
        #region Fields

        /// <summary>
        /// The black pearl name
        /// </summary>
        [JsonProperty(PropertyName = "blackPearlName")] public string BlackPearlName;

        /// <summary>
        /// The bucket
        /// </summary>
        [JsonProperty(PropertyName = "bucket")] public string Bucket;

        /// <summary>
        /// The HTTPS
        /// </summary>
        [JsonProperty(PropertyName = "https")] public bool Https;

        /// <summary>
        /// The name
        /// </summary>
        [JsonProperty(PropertyName = "name")] public string Name;

        /// <summary>
        /// The user name
        /// </summary>
        [JsonProperty(PropertyName = "username")] public string UserName;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentConfig"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="blackPearlName">Name of the black pearl.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="bucket">The bucket.</param>
        /// <param name="https">if set to <c>true</c> [HTTPS].</param>
        public AgentConfig(string name, string blackPearlName, string userName, string bucket, bool https)
        {
            Name = name;
            BlackPearlName = blackPearlName;
            UserName = userName;
            Bucket = bucket;
            Https = https;
        }

        #endregion Constructors
    }
}