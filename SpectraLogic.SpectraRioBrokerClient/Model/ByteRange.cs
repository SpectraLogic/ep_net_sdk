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

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public class ByteRange
    {
        #region Fields

        [JsonProperty(PropertyName = "endingIndex")] private long _endingIndex;
        [JsonProperty(PropertyName = "startingIndex")] private long _startingIndex;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteRange" /> class.
        /// </summary>
        /// <param name="startingIndex">Index of the starting.</param>
        /// <param name="endingIndex">Index of the ending.</param>
        public ByteRange(long startingIndex, long endingIndex)
        {
            _startingIndex = startingIndex;
            _endingIndex = endingIndex;
        }

        #endregion Constructors
    }
}