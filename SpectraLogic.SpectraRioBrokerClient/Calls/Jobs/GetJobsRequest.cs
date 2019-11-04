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

using System.Collections.Generic;
using SpectraLogic.SpectraRioBrokerClient.Model;

namespace SpectraLogic.SpectraRioBrokerClient.Calls.Jobs
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Calls.RestRequest" />
    public class GetJobsRequest : RestRequest
    {
        #region Public Constructors

        /// <summary>Initializes a new instance of the <see cref="GetJobsRequest"/> class.</summary>
        /// <param name="brokers">Filter jobs to only include those pertaining to the specified brokers.</param>
        /// <param name="jobTypes">The type of job to include in the search.</param>
        /// <param name="jobStatuses">The status of job to include in the search.</param>
        /// <param name="sortBy">Specify the field by which to sort.</param>
        /// <param name="sortOrder">Specify the sort direction.</param>
        /// <param name="page">For paginated responses, the page to return.</param>
        /// <param name="perPage">The number of jobs returned in a page.</param>
        public GetJobsRequest(IList<string> brokers = null, IList<JobTypeEnum> jobTypes = null,
            IList<JobStatusEnum> jobStatuses = null, JobsSortByEnum? sortBy = null, SortOrderEnum? sortOrder = null,
            int? page = null,
            int? perPage = null)
        {
            if (brokers != null)
            {
                foreach (var broker in brokers)
                {
                    QueryParams.Add("broker", broker);
                }
            }

            if (jobTypes != null)
            {
                foreach (var jobType in jobTypes)
                {
                    QueryParams.Add("job_type", jobType.ToString());
                }
            }

            if (jobStatuses != null)
            {
                foreach (var jobStatus in jobStatuses)
                {
                    QueryParams.Add("status", jobStatus.ToString());
                }
            }

            if (sortBy != null)
            {
                QueryParams.Add("sort_by", sortBy.ToString());
            }

            if (sortOrder != null)
            {
                QueryParams.Add("sort_oder", sortOrder.ToString());
            }

            if (page != null)
            {
                QueryParams.Add("page", page.ToString());
            }

            if (perPage != null)
            {
                QueryParams.Add("per_page", perPage.ToString());
            }
        }

        #endregion Public Constructors

        #region Internal Properties

        internal override string Path => "api/jobs";
        internal override HttpVerb Verb => HttpVerb.GET;

        #endregion Internal Properties
    }
}