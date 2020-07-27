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
using SpectraLogic.SpectraRioBrokerClient.Utils;

namespace SpectraLogic.SpectraRioBrokerClient.Calls.Broker
{
    /// <summary></summary>
    public class DeleteBrokerRequest : RestRequest
    {
        #region Fields

        /// <summary>The broker name</summary>
        public readonly string BrokerName;

        #endregion Fields

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="DeleteBrokerRequest"/> class.</summary>
        /// <param name="brokerName">Name of the broker.</param>
        /// <param name="force">Force broker delete even if it has associated files. Default=false</param>
        public DeleteBrokerRequest(string brokerName, bool force = false)
        {
            Contract.Requires<ArgumentNullException>(brokerName != null, "brokerName");

            BrokerName = brokerName;
            AddQueryParam("force", force.ToString());
        }

        #endregion Constructors

        #region Properties

        internal override string Path => $"/api/brokers/{BrokerName}";
        internal override HttpVerb Verb => HttpVerb.DELETE;

        #endregion Properties
    }
}