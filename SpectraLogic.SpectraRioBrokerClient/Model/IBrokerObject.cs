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

using System;
using System.Collections.Generic;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    /// </summary>
    public interface IBrokerObject
    {
        #region Properties

        /// <summary>
        /// Gets the broker.
        /// </summary>
        /// <value>The broker.</value>
        string Broker { get; }

        /// <summary>
        /// Gets the checksum.
        /// </summary>
        /// <value>The checksum.</value>
        Checksum Checksum { get; }

        /// <summary>
        /// Gets the creation date.
        /// </summary>
        /// <value>The creation date.</value>
        DateTime CreationDate { get; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        IDictionary<string, string> Metadata { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Gets the object locations.
        /// </summary>
        /// <value>The object locations.</value>
        IList<ObjectLocation> ObjectLocations { get; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        long Size { get; }

        #endregion Properties
    }
}