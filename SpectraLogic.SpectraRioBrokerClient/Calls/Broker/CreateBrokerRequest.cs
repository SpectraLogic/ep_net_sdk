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
using Newtonsoft.Json;
using SpectraLogic.SpectraRioBrokerClient.Model;
using SpectraLogic.SpectraRioBrokerClient.Utils;

namespace SpectraLogic.SpectraRioBrokerClient.Calls.Broker
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Calls.RestRequest" />
    public class CreateBrokerRequest : RestRequest
    {
        #region Fields

        /// <summary>
        /// The agent configuration
        /// </summary>
        [JsonProperty(PropertyName = "agentConfig")] public AgentConfig AgentConfig;

        /// <summary>
        /// The agent name
        /// </summary>
        [JsonProperty(PropertyName = "agentName")] public string AgentName;

        /// <summary>
        /// The broker name
        /// </summary>
        [JsonProperty(PropertyName = "name")] public string BrokerName;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateBrokerRequest" /> class.
        /// </summary>
        /// <param name="brokerName">The broker name.</param>
        /// <param name="agentName">Name of the agent.</param>
        /// <param name="agentConfig">The agent configuration.</param>
        public CreateBrokerRequest(string brokerName, string agentName, AgentConfig agentConfig)
        {
            Contract.Requires<ArgumentNullException>(brokerName != null, "brokerName");
            Contract.Requires<ArgumentNullException>(agentConfig != null, "agent");
            Contract.Requires<ArgumentNullException>(agentName != null, "agentName");

            BrokerName = brokerName;
            AgentName = agentName;
            AgentConfig = agentConfig;
        }

        #endregion Constructors

        #region Properties

        internal override string Path => "/api/brokers";
        internal override HttpVerb Verb => HttpVerb.POST;

        #endregion Properties

        #region Methods

        internal override string GetBody()
        {
            return JsonConvert.SerializeObject(this);
        }

        #endregion Methods
    }
}