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
using System.Collections.Generic;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public class ArchiveFile
    {
        #region Fields

        /// <summary>
        /// The index media
        /// </summary>
        [JsonProperty(PropertyName = "indexMedia")] public bool IndexMedia;

        /// <summary>
        /// The metadata
        /// </summary>
        [JsonProperty(PropertyName = "metadata")] public IDictionary<string, string> Metadata;

        /// <summary>
        /// The name
        /// </summary>
        [JsonProperty(PropertyName = "name")] public string Name;

        /// <summary>
        /// The relationships
        /// </summary>
        [JsonProperty(PropertyName = "relationships")] public ISet<string> Relationships = new HashSet<string>();

        /// <summary>
        /// The size
        /// </summary>
        [JsonProperty(PropertyName = "size")] public long Size;

        /// <summary>
        /// The URI
        /// </summary>
        [JsonProperty(PropertyName = "uri")] public string Uri;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveFile"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="size">The size.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="indexMedia">if set to <c>true</c> [index media].</param>
        public ArchiveFile(string name, string uri, long size, IDictionary<string, string> metadata, bool indexMedia)
        {
            Name = name;
            Uri = uri;
            Size = size;
            Metadata = metadata;
            IndexMedia = indexMedia;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveFile"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="size">The size.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="indexMedia">if set to <c>true</c> [index media].</param>
        /// <param name="relationships">The relationships.</param>
        public ArchiveFile(string name, string uri, long size, IDictionary<string, string> metadata, bool indexMedia, ISet<string> relationships)
        {
            Name = name;
            Uri = uri;
            Size = size;
            Metadata = metadata;
            IndexMedia = indexMedia;
            Relationships = relationships;
        }

        #endregion Constructors
    }
}