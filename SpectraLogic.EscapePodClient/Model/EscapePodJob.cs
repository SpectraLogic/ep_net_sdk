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

using Newtonsoft.Json;
using System;

namespace SpectraLogic.EscapePodClient.Model
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.EscapePodClient.Model.IEscapePodJob" />
    public class EscapePodJob : IEscapePodJob
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EscapePodJob"/> class.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <param name="jobType">Type of the job.</param>
        /// <param name="numberOfFiles">The number of files.</param>
        /// <param name="totalSizeInBytes">The total size in bytes.</param>
        /// <param name="created">The created.</param>
        /// <param name="progress">The progress.</param>
        /// <param name="status">The status.</param>
        public EscapePodJob(Guid jobId, EscapePodJobType jobType, int numberOfFiles, long totalSizeInBytes, string created, double progress, EscapePodJobStatus status)
        {
            JobId = jobId;
            JobType = jobType;
            NumberOfFiles = numberOfFiles;
            TotalSizeInBytes = totalSizeInBytes;
            Created = created;
            Progress = progress;
            Status = status;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty(Order = 1, PropertyName = "id")] public Guid JobId { get; }

        /// <summary>
        /// Gets the type of the job.
        /// </summary>
        /// <value>
        /// The type of the job.
        /// </value>
        [JsonProperty(Order = 2, PropertyName = "jobType")] public EscapePodJobType JobType { get; }

        /// <summary>
        /// Gets the number of files.
        /// </summary>
        /// <value>
        /// The number of files.
        /// </value>
        [JsonProperty(Order = 3, PropertyName = "numberOfFiles")] public int NumberOfFiles { get; }

        /// <summary>
        /// Gets the files transferred.
        /// </summary>
        /// <value>
        /// The files transferred.
        /// </value>
        [JsonProperty(Order = 4, PropertyName = "filesTransferred")] public int FilesTransferred { get; }

        /// <summary>
        /// Gets the total size in bytes.
        /// </summary>
        /// <value>
        /// The total size in bytes.
        /// </value>
        [JsonProperty(Order = 5, PropertyName = "totalSizeInBytes")] public long TotalSizeInBytes { get; }

        /// <summary>
        /// Gets the bytes transferred.
        /// </summary>
        /// <value>
        /// The bytes transferred.
        /// </value>
        [JsonProperty(Order = 6, PropertyName = "bytesTransferred")] public long BytesTransferred { get; }

        /// <summary>
        /// Gets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        [JsonProperty(Order = 7, PropertyName = "created")] public string Created { get; }

        /// <summary>
        /// Gets the progress.
        /// </summary>
        /// <value>
        /// The progress.
        /// </value>
        [JsonProperty(Order = 8, PropertyName = "progress")] public double Progress { get; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonProperty(Order = 9, PropertyName = "status")] public EscapePodJobStatus Status { get; }

        #endregion Properties
    }
}