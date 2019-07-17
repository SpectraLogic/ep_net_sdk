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

using Newtonsoft.Json;
using System.Collections.Generic;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary></summary>
    public class Jobs : IJobs
    {
        #region Public Constructors

        /// <summary>Initializes a new instance of the <see cref="Jobs"/> class.</summary>
        /// <param name="jobsList">The jobs list.</param>
        public Jobs(IList<Job> jobsList)
        {
            JobsList = jobsList;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>Gets the jobs.</summary>
        /// <value>The jobs.</value>
        [JsonProperty(PropertyName = "jobs")] public IList<Job> JobsList { get; }

        #endregion Public Properties
    }
}