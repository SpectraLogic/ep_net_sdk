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
using SpectraLogic.SpectraRioBrokerClient.Utils;
using System;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Model.IBroker" />
    public class Broker : IBroker
    {
        #region Private Constructors

        [JsonConstructor]
        private Broker(string brokerName, string creationDate, long objectCount)
        {
            BrokerName = brokerName;
            CreationDate = creationDate.ToDateTime();
            ObjectCount = objectCount;
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>Gets the name of the broker.</summary>
        /// <value>The name of the broker.</value>
        [JsonProperty(PropertyName = "name")] public string BrokerName { get; }

        /// <summary>Gets the creation date.</summary>
        /// <value>The creation date.</value>
        [JsonProperty(PropertyName = "creationDate")] public DateTime CreationDate { get; }

        /// <summary>Gets the object count.</summary>
        /// <value>The object count.</value>
        [JsonProperty(PropertyName = "objectCount")] public long ObjectCount { get; }

        #endregion Public Properties
    }
}