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

using System;

namespace SpectraLogic.SpectraRioBrokerClient.Calls
{
    internal class HeadJobRequest : RestRequest
    {
        #region Private Fields

        private Guid JobId;

        #endregion Private Fields

        #region Public Constructors

        public HeadJobRequest(Guid jobId)
        {
            JobId = jobId;
        }

        #endregion Public Constructors

        #region Internal Properties

        internal override string Path => $"/api/jobs/{JobId}";
        internal override HttpVerb Verb => HttpVerb.HEAD;

        #endregion Internal Properties
    }
}