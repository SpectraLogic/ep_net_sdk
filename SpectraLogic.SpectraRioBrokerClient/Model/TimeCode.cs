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

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary></summary>
    public class TimeCode
    {
        #region Fields

        private readonly string _divider;
        private readonly string _ff;
        private readonly string _hh;
        private readonly string _mm;
        private readonly string _ss;

        #endregion Fields

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="TimeCode"/> class.</summary>
        /// <param name="HH">The Hours.</param>
        /// <param name="MM">The Minutes.</param>
        /// <param name="SS">The Seconds.</param>
        /// <param name="FF">The Frames.</param>
        /// <param name="dropFrame">if set to <c>true</c> [divider].</param>
        public TimeCode(int HH, int MM, int SS, int FF, bool dropFrame)
        {
            this._hh = HH.ToString("D2");
            this._mm = MM.ToString("D2");
            this._ss = SS.ToString("D2");
            this._ff = FF.ToString("D2");
            _divider = dropFrame ? ";" : ":";
        }

        #endregion Constructors

        #region Methods

        /// <summary>Determines whether [is drop frame].</summary>
        /// <returns>
        ///   <c>true</c> if [is drop frame]; otherwise, <c>false</c>.</returns>
        public bool IsDropFrame => _divider.Equals(";");

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString() => $"{_hh}:{_mm}:{_ss}{_divider}{_ff}";

        #endregion Methods
    }
}