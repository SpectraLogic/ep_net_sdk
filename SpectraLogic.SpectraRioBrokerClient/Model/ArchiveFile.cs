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
using System.Collections.Generic;
using Newtonsoft.Json;
using SpectraLogic.SpectraRioBrokerClient.Utils.JsonConverters;

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
        [JsonProperty(PropertyName = "indexMedia", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IndexMedia;

        /// <summary>
        /// The metadata
        /// </summary>
        [JsonProperty(PropertyName = "metadata", NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, string> Metadata;

        /// <summary>
        /// The name
        /// </summary>
        [JsonProperty(PropertyName = "name")] public string Name;

        /// <summary>
        /// The size
        /// </summary>
        [JsonProperty(PropertyName = "size", NullValueHandling = NullValueHandling.Ignore)]
        public long? Size;

        /// <summary>
        /// The URI
        /// </summary>
        [JsonProperty(PropertyName = "uri")]
        [JsonConverter(typeof(UriJsonConverter))]
        public Uri Uri;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveFile"/> class.
        /// </summary>
        /// <param name="name">The name of the file that will appear in RIO Broker.</param>
        /// <param name="uri">The source URI where the file can be retrieved.</param>
        /// <param name="size">The size of the file to be archived. This is optional</param>
        /// <param name="metadata">Key value pairs of metadata to associate with the file. This is optional</param>
        /// <param name="indexMedia">If enabled, if the file is a media file, it will be indexed to allow for time based partial file restores. This is optional and defaults to false</param>
        public ArchiveFile(string name, Uri uri, long? size = null, IDictionary<string, string> metadata = null,
            bool? indexMedia = null)
        {
            Name = name;
            Uri = uri;
            Size = size;
            Metadata = metadata;
            IndexMedia = indexMedia;
        }

        #endregion Constructors
    }
}