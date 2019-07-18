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

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary></summary>
    public class TimeCode
    {
        #region Fields

        private readonly string Divider;
        private readonly string FF;
        private readonly string HH;
        private readonly string MM;
        private readonly string SS;

        #endregion Fields

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="TimeCode"/> class.</summary>
        /// <param name="HH">The hh.</param>
        /// <param name="MM">The mm.</param>
        /// <param name="SS">The ss.</param>
        /// <param name="FF">The ff.</param>
        /// <param name="dropFrame">if set to <c>true</c> [divider].</param>
        public TimeCode(int HH, int MM, int SS, int FF, bool dropFrame)
        {
            this.HH = HH.ToString("D2");
            this.MM = MM.ToString("D2");
            this.SS = SS.ToString("D2");
            this.FF = FF.ToString("D2");
            Divider = dropFrame ? ";" : ":";
        }

        #endregion Constructors

        #region Methods

        /// <summary>Determines whether [is drop frame].</summary>
        /// <returns>
        ///   <c>true</c> if [is drop frame]; otherwise, <c>false</c>.</returns>
        public bool IsDropFrame => Divider.Equals(";");

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString() => $"{HH}:{MM}:{SS}{Divider}{FF}";

        #endregion Methods
    }
}