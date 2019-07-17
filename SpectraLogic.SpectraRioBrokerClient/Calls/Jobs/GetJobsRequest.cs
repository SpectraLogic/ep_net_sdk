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

using System.Collections.Generic;

namespace SpectraLogic.SpectraRioBrokerClient.Calls
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Calls.RestRequest" />
    public class GetJobsRequest : RestRequest
    {
        #region Public Constructors

        /// <summary>Initializes a new instance of the <see cref="GetJobsRequest"/> class.</summary>
        /// <param name="brokers">The brokers.</param>
        /// <param name="jobTypes">Types of the job.</param>
        /// <param name="jobStatuses">The job statuses.</param>
        public GetJobsRequest(IList<string> brokers = null, IList<string> jobTypes = null, IList<string> jobStatuses = null)
        {
            if (brokers != null)
            {
                foreach (string broker in brokers)
                {
                    QueryParams.Add("broker", broker);
                }
            }

            if (jobTypes != null)
            {
                foreach (string jobType in jobTypes)
                {
                    QueryParams.Add("job_type", jobType);
                }
            }

            if (jobStatuses != null)
            {
                foreach(string jobStatus in jobStatuses)
                {
                    QueryParams.Add("status", jobStatus);
                }
            }
        }

        #endregion Public Constructors

        #region Internal Properties

        internal override string Path => $"api/jobs";
        internal override HttpVerb Verb => HttpVerb.GET;

        #endregion Internal Properties
    }
}