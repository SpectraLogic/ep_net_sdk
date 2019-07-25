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

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public interface IRioSystem
    {
        #region Public Properties

        /// <summary>
        /// Gets the API version.
        /// </summary>
        /// <value>
        /// The API version.
        /// </value>
        string ApiVersion { get; }

        /// <summary>
        /// Gets the build date.
        /// </summary>
        /// <value>
        /// The build date.
        /// </value>
        DateTime BuildDate { get; }

        /// <summary>
        /// Gets the git commit hash.
        /// </summary>
        /// <value>
        /// The git commit hash.
        /// </value>
        string GitCommitHash { get; }

        /// <summary>
        /// Gets the runtime stats.
        /// </summary>
        /// <value>
        /// The runtime stats.
        /// </value>
        RuntimeStats RuntimeStats { get; }

        /// <summary>
        /// Gets the server.
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        Server Server { get; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        string Version { get; }

        #endregion Public Properties
    }
}