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

namespace SpectraLogic.SpectraStorageBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraStorageBrokerClient.Model.IBroker" />
    public class Broker : IBroker
    {
        #region Constructors

        [JsonConstructor]
        private Broker(string brokerName, string creationDate)
        {
            BrokerName = brokerName;
            CreationDate = creationDate;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the name of the broker.
        /// </summary>
        /// <value>
        /// The name of the broker.
        /// </value>
        [JsonProperty(Order = 1, PropertyName = "name")] public string BrokerName { get; }

        /// <summary>
        /// Gets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        [JsonProperty(Order = 2, PropertyName = "creationDate")] public string CreationDate { get; }

        #endregion Properties
    }
}