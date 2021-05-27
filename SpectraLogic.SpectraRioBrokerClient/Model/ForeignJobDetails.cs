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

using Newtonsoft.Json;
using System;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    /// </summary>
    public enum ForeignJobType
    {
        /// <summary>
        /// The black pearl
        /// </summary>
        BLACK_PEARL,

        /// <summary>
        /// The SGL
        /// </summary>
        SGL,

        /// <summary>
        /// The flashnet
        /// </summary>
        FLASHNET,
        
        /// <summary>
        /// The diva
        /// </summary>
        DIVA,

        /// <summary>
        /// The unknown
        /// </summary>
        UNKNOWN
    }

    /// <summary>
    /// </summary>
    public class ForeignJobDetails
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignJobDetails"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="type">The type.</param>
        public ForeignJobDetails(string id, ForeignJobType type)
        {
            Id = id;
            Type = type;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty(PropertyName = "id")] public string Id { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        [JsonProperty(PropertyName = "type")] public ForeignJobType Type { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance;
        /// otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var x = obj as ForeignJobDetails;
            return Id.Equals(x.Id) && Type.Equals(x.Type);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data
        /// structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return (Id.GetHashCode() * 397) ^ (int)Type;
        }

        #endregion Methods
    }
}