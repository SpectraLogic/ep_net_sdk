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

using System;
using Newtonsoft.Json;
using SpectraLogic.SpectraRioBrokerClient.Utils;
using SpectraLogic.SpectraRioBrokerClient.Utils.JsonConverters;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public class JobFileStatus
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JobFileStatus"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="status">The status.</param>
        /// <param name="statusMessage">The status message.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="sizeInBytes">The size in bytes.</param>
        /// <param name="lastUpdated">The last updated date.</param>
        public JobFileStatus(string name, string status, string statusMessage, Uri uri, long sizeInBytes, string lastUpdated)
        {
            Name = name;
            Status = status;
            StatusMessage = statusMessage;
            Uri = uri;
            SizeInBytes = sizeInBytes;
            LastUpdated = lastUpdated.ToDateTime();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty(PropertyName = "name")] public string Name { get; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonProperty(PropertyName = "status")] public string Status { get; }

        /// <summary>
        /// Gets the status message.
        /// </summary>
        /// <value>
        /// The status message.
        /// </value>
        [JsonProperty(PropertyName = "statusMessage")] public string StatusMessage { get; }

        /// <summary>
        /// Gets the Uri.
        /// </summary>
        /// <value>
        /// The Uri.
        /// </value>
        [JsonProperty(PropertyName = "uri")]
        [JsonConverter(typeof(UriJsonConverter))]
        public Uri Uri;
        
        /// <summary>
        /// Gets the size in bytes.
        /// </summary>
        /// <value>
        /// The size in bytes.
        /// </value>
        [JsonProperty(PropertyName = "sizeInBytes")] public long SizeInBytes { get; }
        
        /// <summary>
        /// Gets the last updated date.
        /// </summary>
        /// <value>The last updated date.</value>
        [JsonProperty(PropertyName = "lastUpdated")] public DateTime LastUpdated { get; }
        
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
            return $"<name: {Name}, status: {Status}, statusMessage: {StatusMessage}>";
        }

        #endregion Methods
    }
}