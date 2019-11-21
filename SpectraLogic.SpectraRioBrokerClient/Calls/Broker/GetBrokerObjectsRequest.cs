﻿/*
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
using System.Collections.Generic;
using SpectraLogic.SpectraRioBrokerClient.Model;
using SpectraLogic.SpectraRioBrokerClient.Utils;

namespace SpectraLogic.SpectraRioBrokerClient.Calls.Broker
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Calls.RestRequest" />
    public class GetBrokerObjectsRequest : RestRequest
    {
        #region Public Constructors

        /// <summary>Initializes a new instance of the <see cref="GetBrokerObjectsRequest"/> class.</summary>
        /// <param name="brokerName">Name of the broker.</param>
        /// <param name="sortBy">The sort by.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="page">The page.</param>
        /// <param name="perPage">The per page.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="relationships">The relationships.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public GetBrokerObjectsRequest(
            string brokerName,
            ObjectsSortByEnum? sortBy = null,
            SortOrderEnum? sortOrder = null,
            int? page = null,
            int? perPage = null,
            string prefix = null,
            string filename = null,
            List<KeyValuePair<string, string>> metadata = null,
            List<string> relationships = null)
        {
            Contract.Requires<ArgumentNullException>(brokerName != null, "brokerName");

            BrokerName = brokerName;

            if (sortBy != null)
            {
                AddQueryParam("sort_by", sortBy.ToString());
            }

            if (sortOrder != null)
            {
                AddQueryParam("sort_order", sortOrder.ToString());
            }

            if (page != null)
            {
                AddQueryParam("page", page.ToString());
            }

            if (perPage != null)
            {
                AddQueryParam("per_page", perPage.ToString());
            }

            if (prefix != null)
            {
                AddQueryParam("prefix", prefix);
            }

            if (filename != null)
            {
                AddQueryParam("filename", filename);
            }

            metadata?.ForEach(pair => { AddQueryParam("metadata", $"{pair.Key},{pair.Value}"); });

            relationships?.ForEach(relationship => { AddQueryParam("relationships", relationship); });
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>Gets the name of the broker.</summary>
        /// <value>The name of the broker.</value>
        public string BrokerName { get; private set; }

        #endregion Public Properties

        #region Internal Properties

        internal override string Path => $"/api/brokers/{BrokerName}/objects";
        internal override HttpVerb Verb => HttpVerb.GET;

        #endregion Internal Properties
    }
}