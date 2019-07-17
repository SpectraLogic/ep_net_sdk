﻿/*
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
using SpectraLogic.SpectraRioBrokerClient.Utils;
using System;
using System.Collections.Generic;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Model.IJob" />
    public class Job : IJob
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Job" /> class.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <param name="jobType">Type of the job.</param>
        /// <param name="numberOfFiles">The number of files.</param>
        /// <param name="filesTransferred">The files transferred.</param>
        /// <param name="totalSizeInBytes">The total size in bytes.</param>
        /// <param name="creationDate">The created.</param>
        /// <param name="lastUpdated">The last updated.</param>
        /// <param name="progress">The progress.</param>
        /// <param name="status">The status.</param>
        /// <param name="files">The files.</param>
        public Job(Guid jobId, JobType jobType, int numberOfFiles, int filesTransferred, long totalSizeInBytes, string creationDate, string lastUpdated, double progress, JobStatus status, IList<JobFileStatus> files)
        {
            JobId = jobId;
            JobType = jobType;
            NumberOfFiles = numberOfFiles;
            FilesTransferred = filesTransferred;
            TotalSizeInBytes = totalSizeInBytes;
            CreationDate = creationDate.ToDateTime();
            LastUpdated = lastUpdated.ToDateTime();
            Progress = progress;
            Status = status;
            Files = files;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        [JsonProperty(PropertyName = "creationDate")] public DateTime CreationDate { get; }

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <value>
        /// The files.
        /// </value>
        [JsonProperty(PropertyName = "files")] public IList<JobFileStatus> Files { get; }

        /// <summary>
        /// Gets the files transferred.
        /// </summary>
        /// <value>
        /// The files transferred.
        /// </value>
        [JsonProperty(PropertyName = "filesTransferred")] public int FilesTransferred { get; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty(PropertyName = "id")] public Guid JobId { get; }

        /// <summary>
        /// Gets the type of the job.
        /// </summary>
        /// <value>
        /// The type of the job.
        /// </value>
        [JsonProperty(PropertyName = "jobType")] public JobType JobType { get; }

        /// <summary>
        /// Gets the last updated.
        /// </summary>
        /// <value>
        /// The last updated.
        /// </value>
        [JsonProperty(PropertyName = "lastUpdated")] public DateTime LastUpdated { get; }

        /// <summary>
        /// Gets the number of files.
        /// </summary>
        /// <value>
        /// The number of files.
        /// </value>
        [JsonProperty(PropertyName = "numberOfFiles")] public int NumberOfFiles { get; }

        /// <summary>
        /// Gets the progress.
        /// </summary>
        /// <value>
        /// The progress.
        /// </value>
        [JsonProperty(PropertyName = "progress")] public double Progress { get; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonProperty(PropertyName = "status")] public JobStatus Status { get; }

        /// <summary>
        /// Gets the total size in bytes.
        /// </summary>
        /// <value>
        /// The total size in bytes.
        /// </value>
        [JsonProperty(PropertyName = "totalSizeInBytes")] public long TotalSizeInBytes { get; }

        #endregion Properties
    }
}