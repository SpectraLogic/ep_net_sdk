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

using System.Collections.Generic;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary></summary>
    public interface IJobs
    {
        #region Public Properties

        /// <summary>Gets the page.</summary>
        /// <value>The page.</value>
        PageResult Page { get; }

        /// <summary>Gets the jobs.</summary>
        /// <value>The jobs.</value>
        IList<Job> JobsList { get; }

        #endregion Public Properties
    }
}