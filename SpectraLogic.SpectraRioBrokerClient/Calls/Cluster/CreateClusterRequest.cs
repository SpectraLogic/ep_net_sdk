﻿/*
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

namespace SpectraLogic.SpectraRioBrokerClient.Calls.Cluster
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Calls.RestRequest" />
    public class CreateClusterRequest : RestRequest
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateClusterRequest"/> class.
        /// </summary>
        /// <param name="clusterName">The cluster name.</param>
        public CreateClusterRequest(string clusterName)
        {
            Contract.Requires<ArgumentNullException>(clusterName != null, "clusterName");

            AddQueryParam("name", clusterName);
        }

        #endregion Public Constructors

        #region Internal Properties

        internal override string Path => "/api/cluster";
        internal override HttpVerb Verb => HttpVerb.POST;

        #endregion Internal Properties
    }
}