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
using SpectraLogic.SpectraRioBrokerClient.Utils;

namespace SpectraLogic.SpectraRioBrokerClient.Calls.Broker
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Calls.RestRequest" />
    public class GetBrokerRelationshipsRequest : RestRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBrokerRelationshipsRequest"/> class.
        /// </summary>
        /// <param name="brokerName">Name of the broker.</param>
        /// <param name="page">The page.</param>
        /// <param name="perPage">The per page.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public GetBrokerRelationshipsRequest(string brokerName, int? page = null, int? perPage = null)
        {
            Contract.Requires<ArgumentNullException>(brokerName != null, "brokerName");

            BrokerName = brokerName;
            
            if (page != null)
            {
                AddQueryParam("page", page.ToString());
            }

            if (perPage != null)
            {
                AddQueryParam("per_page", perPage.ToString());
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the name of the broker.
        /// </summary>
        /// <value>
        /// The name of the broker.
        /// </value>
        public string BrokerName { get; private set; }

        internal override string Path => $"/api/brokers/{BrokerName}/relationships";
        internal override HttpVerb Verb => HttpVerb.GET;

        #endregion Properties
    }
}