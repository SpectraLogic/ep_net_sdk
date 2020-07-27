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

namespace SpectraLogic.SpectraRioBrokerClient.Calls.Broker
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Calls.RestRequest" />
    public class GetBrokersRequest : RestRequest
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBrokersRequest"/> class.
        /// <param name="page">The page.</param>
        /// <param name="perPage">The per page.</param>
        /// </summary>
        public GetBrokersRequest(int? page = null, int? perPage = null)
        {
            if (page != null)
            {
                AddQueryParam("page", page.ToString());
            }

            if (perPage != null)
            {
                AddQueryParam("per_page", perPage.ToString());
            }
        }

        #endregion Public Constructors

        #region Internal Properties

        internal override string Path => "/api/brokers";
        internal override HttpVerb Verb => HttpVerb.GET;

        #endregion Internal Properties
    }
}