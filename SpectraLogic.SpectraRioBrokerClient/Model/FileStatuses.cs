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
using Newtonsoft.Json;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary></summary>
    public class FileStatuses : IFileStatuses
    {
        #region Public Constructors

        /// <summary>Initializes a new instance of the <see cref="FileStatuses"/> class.</summary>
        /// <param name="fileStatusesList">The job list of file statuses.</param>
        public FileStatuses(IList<FileStatus> fileStatusesList)
        {
            FileStatusesList = fileStatusesList;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>Gets the jobs file status.</summary>
        /// <value>The jobs file status.</value>
        [JsonProperty(PropertyName = "fileStatus")]
        public IList<FileStatus> FileStatusesList { get; }

        #endregion Public Properties
    }
}