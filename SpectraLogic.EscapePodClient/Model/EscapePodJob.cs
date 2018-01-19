﻿/*
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

using System;
using System.Runtime.Serialization;

namespace SpectraLogic.EscapePodClient.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="SpectraLogic.EscapePodClient.Model.IEscapePodJob" />
    [DataContract]
    public class EscapePodJob : IEscapePodJob
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember(IsRequired = true)] public string Id { get; set; }
        [DataMember(Name = "Status", IsRequired = true)] public string StatusString { get; set; }

        public Status Status => Enum.TryParse(StatusString, true, out Status ret) ? ret : Status.UNKNOWN;
    }

    public enum Status
    {
        IN_PROGRESS,
        CANCELED,
        DONE,
        UNKNOWN
    }
}
