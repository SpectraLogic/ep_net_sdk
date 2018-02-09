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

namespace SpectraLogic.EscapePodClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public class ArchiveFile
    {
        #region Fields

        /// <summary>
        /// The name
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "name")] public string Name;

        /// <summary>
        /// The URI
        /// </summary>
        [JsonProperty(Order = 2, PropertyName = "uri")] public string Uri;

        /// <summary>
        /// The size
        /// </summary>
        [JsonProperty(Order = 3, PropertyName = "size")] public long Size;

        /// <summary>
        /// The metadata
        /// </summary>
        [JsonProperty(Order = 4, PropertyName = "metadata")] public IDictionary<string, string> Metadata;

        /// <summary>
        /// The index media
        /// </summary>
        [JsonProperty(Order = 5, PropertyName = "indexMedia")] public bool IndexMedia;

        /// <summary>
        /// The store file properties
        /// </summary>
        [JsonProperty(Order = 6, PropertyName = "storeFileProperties")] public bool StoreFileProperties;

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
        /// <param name="storeFileProperties">if set to <c>true</c> [store file properties].</param>
        public ArchiveFile(string name, string uri, long size, IDictionary<string, string> metadata, bool indexMedia, bool storeFileProperties)
        {
            Name = name;
            Uri = uri;
            Size = size;
            Metadata = metadata;
            IndexMedia = indexMedia;
            StoreFileProperties = storeFileProperties;
        }

        #endregion Constructors
    }
}