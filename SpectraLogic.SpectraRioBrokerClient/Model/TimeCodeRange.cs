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

using SpectraLogic.SpectraRioBrokerClient.Exceptions;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary></summary>
    public class TimeCodeRange
    {
        #region Fields

        private readonly TimeCode Start;
        private readonly TimeCode End;

        #endregion Fields

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="TimeCodeRange"/> class.</summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        public TimeCodeRange(TimeCode start, TimeCode end)
        {
            if (start.IsDropFrame != end.IsDropFrame) throw new MixTimeCodeException("Both start and end timecodes should be drop-frame or non-drop-frame.");

            Start = start;
            End = end;
        }

        #endregion Constructors

        #region Methods

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{Start}-{End}";
        }

        #endregion Methods
    }
}