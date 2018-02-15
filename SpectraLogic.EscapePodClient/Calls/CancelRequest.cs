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

using System;
using System.Linq;

namespace SpectraLogic.EscapePodClient.Calls
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.EscapePodClient.Calls.RestRequest" />
    public class CancelRequest : RestRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelRequest"/> class.
        /// </summary>
        /// <param name="escapePodJobId">The escape pod job identifier.</param>
        public CancelRequest(Guid escapePodJobId)
        {
            AddQueryParam("id", escapePodJobId.ToString());
        }

        #endregion Constructors

        #region Properties

        internal override HttpVerb Verb => HttpVerb.PUT;
        internal override string Path => "api/cancel"; //TODO use the right path

        #endregion Properties

        #region Methods

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

        #endregion Methods
    }
}