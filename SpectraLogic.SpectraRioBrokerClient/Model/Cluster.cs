﻿/*
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
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Model.ICluster" />
    public class Cluster : ICluster
    {
        #region Constructors

        [JsonConstructor]
        private Cluster(string name)
        {
            Name = name;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the cluster name.
        /// </summary>
        /// <value>
        /// The cluster name.
        /// </value>
        [JsonProperty(PropertyName = "name")] public string Name { get; private set; }

        #endregion Properties
    }
}