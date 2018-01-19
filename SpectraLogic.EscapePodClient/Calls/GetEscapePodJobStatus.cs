﻿/*
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

using System.Linq;

namespace SpectraLogic.EscapePodClient.Calls
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="SpectraLogic.EscapePodClient.Calls.RestRequest" />
    public class GetEscapePodJobStatus : RestRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetEscapePodJobStatus"/> class.
        /// </summary>
        /// <param name="escapePodJobId">The escape pod job identifier.</param>
        public GetEscapePodJobStatus(string escapePodJobId)
        {
            AddQueryParam("id", escapePodJobId);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetEscapePodJobStatus"/> class.
        /// </summary>
        /// <param name="escapePodJobId">The escape pod job identifier.</param>
        public GetEscapePodJobStatus(long escapePodJobId)
        {
            AddQueryParam("id", escapePodJobId.ToString());
        }

        internal override HttpVerb Verb => HttpVerb.GET;
        internal override string Path => "api/jobstatus"; //TODO use the right path

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{Path}?{string.Join(";", QueryParams.Select(q => q.Key + "=" + q.Value))}\n{Verb}";
        }
    }
}
