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
using System.Collections.Generic;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    /// </summary>
    public class ObjectLocation
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectLocation"/> class.
        /// </summary>
        /// <param name="locationType">Type of the location.</param>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="barcode">The barcode.</param>
        /// <param name="partitionId">The partition identifier.</param>
        /// <param name="ejected">The ejected.</param>
        /// <param name="inCache">The in cache.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="state">The state.</param>
        /// <param name="metadata">The metadata.</param>
        public ObjectLocation(
            string locationType, string name = null, string type = null, string barcode = null, string partitionId = null,
            bool? ejected = null, bool? inCache = null, string id = null, string state = null, IDictionary<string, string> metadata = null)
        {
            LocationType = locationType;
            Name = name;
            Type = type;
            Barcode = barcode;
            PartitionId = partitionId;
            Ejected = ejected;
            InCache = inCache;
            Id = id;
            State = state;
            Metadata = metadata;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the barcode.
        /// </summary>
        /// <value>The barcode.</value>
        [JsonProperty(PropertyName = "barcode", NullValueHandling = NullValueHandling.Ignore)] public string Barcode { get; }

        /// <summary>
        /// Gets the ejected.
        /// </summary>
        /// <value>The ejected.</value>
        [JsonProperty(PropertyName = "ejected", NullValueHandling = NullValueHandling.Ignore)] public bool? Ejected { get; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)] public string Id { get; }

        /// <summary>
        /// Gets the in cache.
        /// </summary>
        /// <value>The in cache.</value>
        [JsonProperty(PropertyName = "inCache", NullValueHandling = NullValueHandling.Ignore)] public bool? InCache { get; }

        /// <summary>
        /// Gets the type of the location.
        /// </summary>
        /// <value>The type of the location.</value>
        [JsonProperty(PropertyName = "locationType")] public string LocationType { get; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        [JsonProperty(PropertyName = "metadata", NullValueHandling = NullValueHandling.Ignore)] public IDictionary<string, string> Metadata { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)] public string Name { get; }

        /// <summary>
        /// Gets the partition identifier.
        /// </summary>
        /// <value>The partition identifier.</value>
        [JsonProperty(PropertyName = "partitionId", NullValueHandling = NullValueHandling.Ignore)] public string PartitionId { get; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        [JsonProperty(PropertyName = "state", NullValueHandling = NullValueHandling.Ignore)] public string State { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)] public string Type { get; }

        #endregion Properties
    }
}