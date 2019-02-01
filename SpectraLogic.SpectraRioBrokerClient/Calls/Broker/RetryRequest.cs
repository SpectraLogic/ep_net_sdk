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

using SpectraLogic.SpectraRioBrokerClient.Model;
using SpectraLogic.SpectraRioBrokerClient.Utils;
using System;

namespace SpectraLogic.SpectraRioBrokerClient.Calls
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Calls.RestRequest" />
    public class RetryRequest : RestRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryRequest"/> class.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        public RetryRequest(String brokerName, Guid jobId, JobType retryJobType)
        {
            Contract.Requires<ArgumentNullException>(brokerName != null, "brokerName");
            Contract.Requires<InvalidOperationException>(retryJobType == JobType.ARCHIVE || retryJobType == JobType.RESTORE, "retryJobType");

            BrokerName = brokerName;
            RetryJobType = retryJobType;

            QueryParams.Add("retry", jobId.ToString());
        }

        #endregion Constructors

        #region Properties

        /// <summary>Gets the name of the broker.</summary>
        /// <value>The name of the broker.</value>
        public String BrokerName { get; private set; }

        /// <summary>Gets the type of the retry job.</summary>
        /// <value>The type of the retry job.</value>
        public JobType RetryJobType { get; private set; }
        
        internal override string Path => $"/api/brokers/{BrokerName}/{RetryJobType.ToString().ToLower()}";
        internal override HttpVerb Verb => HttpVerb.POST;

        #endregion Properties
    }
}