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
    public static class SortBy
    {
        #region Public Fields

        /// <summary>Sort the results by the creation date.</summary>
        public static string CreationDate = "CREATION_DATE";

        /// <summary>Sort the results by name.</summary>
        public static string Name = "NAME";

        /// <summary>Do no sorting, and let the Metadata sort by the most relevant search.  This is the DEFAULT value.</summary>
        public static string None = "NONE";

        /// <summary>Sort the results by the size of the file.</summary>
        public static string Size = "SIZE";

        #endregion Public Fields
    }
}