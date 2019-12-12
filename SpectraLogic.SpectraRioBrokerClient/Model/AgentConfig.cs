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
    public class AgentConfig
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentConfig"/> class.
        /// </summary>
        /// <param name="blackPearlName">Name of the black pearl.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="bucket">The bucket.</param>
        /// <param name="https">if set to <c>true</c> [HTTPS].</param>
        /// <param name="createBucket">if set to <c>true</c> creates the bucket</param>
        /// <param name="dataPolicyUuid">The data policy uuid</param>
        /// <param name="proxy">The proxy</param>
        /// <param name="maxRetries">The max retires</param>
        /// <param name="delay">The delay</param>
        public AgentConfig(string blackPearlName, string userName, string bucket, bool? https = null,
            bool? createBucket = null, string dataPolicyUuid = null, string proxy = null, int? maxRetries = null,
            long? delay = null)
        {
            BlackPearlName = blackPearlName;
            UserName = userName;
            Bucket = bucket;
            Https = https;
            CreateBucket = createBucket;
            DataPolicyUuid = dataPolicyUuid;
            Proxy = proxy;
            MaxRetries = maxRetries;
            Delay = delay;
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        /// The black pearl name
        /// </summary>
        [JsonProperty(PropertyName = "blackPearlName")]
        public string BlackPearlName;

        /// <summary>
        /// The bucket
        /// </summary>
        [JsonProperty(PropertyName = "bucket")]
        public string Bucket;

        /// <summary>
        /// Create the bucket if doesn't exists
        /// </summary>
        [JsonProperty(PropertyName = "createBucket", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CreateBucket;

        /// <summary>
        /// The data policy uuid
        /// </summary>
        [JsonProperty(PropertyName = "dataPolicyUUID", NullValueHandling = NullValueHandling.Ignore)]
        public string DataPolicyUuid;

        /// <summary>
        /// The delay
        /// </summary>
        [JsonProperty(PropertyName = "delay", NullValueHandling = NullValueHandling.Ignore)]
        public long? Delay;

        /// <summary>
        /// The HTTPS
        /// </summary>
        [JsonProperty(PropertyName = "https", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Https;

        /// <summary>
        /// The max retries
        /// </summary>
        [JsonProperty(PropertyName = "maxRetries", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxRetries;

        /// <summary>
        /// The proxy
        /// </summary>
        [JsonProperty(PropertyName = "proxy", NullValueHandling = NullValueHandling.Ignore)]
        public string Proxy;

        /// <summary>
        /// The user name
        /// </summary>
        [JsonProperty(PropertyName = "username")]
        public string UserName;

        #endregion Fields
    }
}