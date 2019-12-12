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
using SpectraLogic.SpectraRioBrokerClient.Utils.JsonConverters;
using System;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public class FileStatus
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JobFileStatus"/> class.
        /// </summary>
        /// <param name="name">The name of the file.</param>
        /// <param name="uri">The URI of the file.</param>
        /// <param name="status">The current status of the file.</param>
        /// <param name="statusMessage">A longer message explaining the current status of the file.</param>
        /// <param name="lastUpdated">The datetime of when the file status was last updated.</param>
        public FileStatus(string name, Uri uri, string status, string statusMessage, string lastUpdated)
        {
            Name = name;
            Uri = uri;
            Status = status;
            StatusMessage = statusMessage;
            LastUpdated = lastUpdated;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [JsonProperty(PropertyName = "name")] public string Name { get; }

        /// <summary>
        /// Gets the URI of the file.
        /// </summary>
        /// <value>
        /// The URI of the file.
        /// </value>
        [JsonProperty(PropertyName = "uri")]
        [JsonConverter(typeof(UriJsonConverter))]
        public Uri Uri { get; }
        
        /// <summary>
        /// Gets the current status of the file.
        /// </summary>
        /// <value>
        /// The current status of the file.
        /// </value>
        [JsonProperty(PropertyName = "status")] public string Status { get; }

        /// <summary>
        /// Gets the a longer message explaining the current status of the file.
        /// </summary>
        /// <value>
        /// The longer message explaining the current status of the file.
        /// </value>
        [JsonProperty(PropertyName = "statusMessage")] public string StatusMessage { get; }

        /// <summary>
        /// Gets the datetime of when the file status was last updated.
        /// </summary>
        /// <value>
        /// The datetime of when the file status was last updated.
        /// </value>
        [JsonProperty(PropertyName = "lastUpdated")] public string LastUpdated { get; }
        
        #endregion Properties
    }
}