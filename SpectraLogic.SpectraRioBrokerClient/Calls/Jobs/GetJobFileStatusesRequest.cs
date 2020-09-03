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

using SpectraLogic.SpectraRioBrokerClient.Utils;
using System;

namespace SpectraLogic.SpectraRioBrokerClient.Calls.Jobs
{
    /// <summary>
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Calls.RestRequest"/>
    public class GetJobFileStatusesRequest : RestRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetJobFileStatusesRequest"/> class.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <param name="objectName">The object name.</param>
        /// <param name="page">For paginated responses, the page to return.</param>
        /// <param name="perPage">The number of jobs returned in a page.</param>
        public GetJobFileStatusesRequest(Guid jobId, string objectName, int? page = null, int? perPage = null)
        {
            Contract.Requires<ArgumentNullException>(objectName != null, "objectName");

            JobId = jobId;
            ObjectName = objectName;

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
        /// Gets the job identifier.
        /// </summary>
        /// <value>The job identifier.</value>
        public Guid JobId { get; private set; }

        /// <summary>
        /// Gets the object name.
        /// </summary>
        /// <value>The object name.</value>
        public string ObjectName { get; private set; }

        internal override string Path => $"api/jobs/{JobId}/filestatus/{Uri.EscapeDataString(ObjectName)}";
        internal override HttpVerb Verb => HttpVerb.GET;

        #endregion Properties
    }
}