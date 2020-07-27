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
using SpectraLogic.SpectraRioBrokerClient.Utils;
using System;
using System.Collections.Generic;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public class BrokerObject : IBrokerObject
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BrokerObject"/> class.
        /// </summary>
        /// <param name="broker">The broker.</param>
        /// <param name="checksum">The checksum.</param>
        /// <param name="creationDate">The creation date.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="name">The name.</param>
        /// <param name="size">The size.</param>
        public BrokerObject(string broker, Checksum checksum, string creationDate, IDictionary<string, string> metadata, string name, long size)
        {
            Broker = broker;
            Checksum = checksum;
            CreationDate = creationDate.ToDateTime();
            Metadata = metadata;
            Name = name;
            Size = size;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the broker.
        /// </summary>
        /// <value>
        /// The broker.
        /// </value>
        [JsonProperty(PropertyName = "broker")] public string Broker { get; }

        /// <summary>
        /// Gets the checksum.
        /// </summary>
        /// <value>
        /// The checksum.
        /// </value>
        [JsonProperty(PropertyName = "checksum")] public Checksum Checksum { get; }

        /// <summary>
        /// Gets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        [JsonProperty(PropertyName = "creationDate")] public DateTime CreationDate { get; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        [JsonProperty(PropertyName = "metadata")] public IDictionary<string, string> Metadata { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty(PropertyName = "name")] public string Name { get; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        [JsonProperty(PropertyName = "size")] public long Size { get; }

        #endregion Public Properties
    }

    /// <summary>
    ///
    /// </summary>
    public class Checksum
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Checksum"/> class.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <param name="type">The type.</param>
        public Checksum(string hash, string type)
        {
            Hash = hash;
            Type = type;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the hash.
        /// </summary>
        /// <value>
        /// The hash.
        /// </value>
        [JsonProperty(PropertyName = "hash")] public string Hash { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [JsonProperty(PropertyName = "type")] public string Type { get; }

        #endregion Public Properties
    }
}