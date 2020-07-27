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
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Model.IClusterMembers" />
    public class ClusterMembers : IClusterMembers
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="ClusterMembers"/> class.</summary>
        /// <param name="members">The members.</param>
        public ClusterMembers(IList<ClusterMember> members)
        {
            Members = members;
        }

        #endregion Constructors

        #region Properties

        /// <summary>Gets a list of all the members of the cluster.</summary>
        /// <value>The members.</value>
        [JsonProperty(PropertyName = "members")] public IList<ClusterMember> Members { get; }

        #endregion Properties
    }
}