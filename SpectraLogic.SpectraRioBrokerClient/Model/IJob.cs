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

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public interface IJob
    {
        #region Properties

        /// <summary>
        /// Gets the bytes transferred.
        /// </summary>
        /// <value>
        /// The bytes transferred.
        /// </value>
        long BytesTransferred { get; }

        /// <summary>
        /// Gets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        string Created { get; }

        /// <summary>
        /// Gets the files transferred.
        /// </summary>
        /// <value>
        /// The files transferred.
        /// </value>
        int FilesTransferred { get; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        Guid JobId { get; }

        /// <summary>
        /// Gets the type of the job.
        /// </summary>
        /// <value>
        /// The type of the job.
        /// </value>
        JobType JobType { get; }

        /// <summary>
        /// Gets the number of files.
        /// </summary>
        /// <value>
        /// The number of files.
        /// </value>
        int NumberOfFiles { get; }

        /// <summary>
        /// Gets the progress.
        /// </summary>
        /// <value>
        /// The progress.
        /// </value>
        double Progress { get; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        JobStatus Status { get; }

        /// <summary>
        /// Gets the total size in bytes.
        /// </summary>
        /// <value>
        /// The total size in bytes.
        /// </value>
        long TotalSizeInBytes { get; }

        #endregion Properties
    }
}