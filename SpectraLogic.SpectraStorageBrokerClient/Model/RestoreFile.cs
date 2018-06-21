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

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public class RestoreFile
    {
        #region Fields

        /// <summary>
        /// The byte range
        /// </summary>
        [JsonProperty(PropertyName = "byteRange", NullValueHandling = NullValueHandling.Ignore)] public ByteRange ByteRange;

        /// <summary>
        /// The name
        /// </summary>
        [JsonProperty(PropertyName = "name")] public string Name;

        /// <summary>
        /// The restore file attributes
        /// </summary>
        [JsonProperty(PropertyName = "restoreFileAttributes", NullValueHandling = NullValueHandling.Ignore)] public bool? RestoreFileAttributes;

        /// <summary>
        /// The time code range
        /// </summary>
        [JsonProperty(PropertyName = "timeCodeRange", NullValueHandling = NullValueHandling.Ignore)] public TimecodeRange TimeCodeRange;

        /// <summary>
        /// The URI
        /// </summary>
        [JsonProperty(PropertyName = "uri")] public string Uri;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreFile"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="uri">The destination.</param>
        public RestoreFile(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreFile"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="uri">The destination.</param>
        /// <param name="restoreFileAttributes">if set to <c>true</c> [restore file attributes].</param>
        public RestoreFile(string name, string uri, bool restoreFileAttributes)
        {
            Name = name;
            Uri = uri;
            RestoreFileAttributes = restoreFileAttributes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreFile"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="uri">The destination.</param>
        /// <param name="byteRange">The byte range.</param>
        public RestoreFile(string name, string uri, ByteRange byteRange)
        {
            Name = name;
            Uri = uri;
            ByteRange = byteRange;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreFile"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="uri">The destination.</param>
        /// <param name="timeCodeRange">The time code range.</param>
        public RestoreFile(string name, string uri, TimecodeRange timeCodeRange)
        {
            Name = name;
            Uri = uri;
            TimeCodeRange = timeCodeRange;
        }

        #endregion Constructors
    }
}