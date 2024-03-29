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
using SpectraLogic.SpectraRioBrokerClient.Model;

namespace SpectraLogic.SpectraRioBrokerClient.Calls.Jobs
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Calls.RestRequest" />
    public class UpdateJobPriorityRequest : RestRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateJobPriorityRequest"/> class.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <param name="priority">The new job priority</param>
        public UpdateJobPriorityRequest(Guid jobId, JobPriority priority)
        {
            JobId = jobId;
            AddQueryParam("priority", priority.ToString().ToUpper());
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets the job identifier.
        /// </summary>
        /// <value>
        /// The job identifier.
        /// </value>
        public Guid JobId { get; private set; }

        #endregion Public Properties

        #region Internal Properties

        internal override string Path => $"/api/jobs/{JobId}";
        internal override HttpVerb Verb => HttpVerb.PUT;

        #endregion Internal Properties
    }
}