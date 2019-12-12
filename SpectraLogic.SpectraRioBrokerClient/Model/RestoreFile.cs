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
    /// </summary>
    public class RestoreFile
    {
        #region Fields

        /// <summary>
        /// The byte range
        /// </summary>
        [JsonProperty(PropertyName = "byteRange", NullValueHandling = NullValueHandling.Ignore)]
        public ByteRange ByteRange;

        /// <summary>
        /// The name
        /// </summary>
        [JsonProperty(PropertyName = "name")] public string Name;

        /// <summary>
        /// The time code range
        /// </summary>
        [JsonProperty(PropertyName = "timeCodeRange", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(TimeCodeRangeConverter))]
        public TimeCodeRange TimeCodeRange;

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
        public RestoreFile(string name, Uri uri)
        {
            Name = name;
            Uri = uri.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreFile"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="uri">The destination.</param>
        /// <param name="byteRange">The byte range.</param>
        public RestoreFile(string name, Uri uri, ByteRange byteRange)
        {
            Name = name;
            Uri = uri.ToString();
            ByteRange = byteRange;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreFile"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="uri">The destination.</param>
        /// <param name="timeCodeRange">
        /// Beginning and ending frame of the desired clip separated by '-'. Format: hh:mm:ss:ff for
        /// non-drop frame rates and hh:mm:ss;ff for drop frame rates. Example:
        /// 01:00:00;12-01:00:10;13 for drop frame rate.
        /// </param>
        public RestoreFile(string name, Uri uri, TimeCodeRange timeCodeRange)
        {
            Name = name;
            Uri = uri.ToString();
            TimeCodeRange = timeCodeRange;
        }

        #endregion Constructors
    }
}