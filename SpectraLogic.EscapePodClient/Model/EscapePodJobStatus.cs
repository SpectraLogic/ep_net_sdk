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

using System;
using System.Runtime.Serialization;

namespace SpectraLogic.EscapePodClient.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="SpectraLogic.EscapePodClient.Model.IEscapePodJobStatus" />
    [DataContract]
    public class EscapePodJobStatus : IEscapePodJobStatus
    {
        [DataMember(Name="Status")] private string StatusString { get; set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public Status Status => Enum.TryParse(StatusString, true, out Status ret) ? ret : Status.UNKNOWN;
    }

    /// <summary>
    /// 
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Job is in progress
        /// </summary>
        IN_PROGRESS,
        /// <summary>
        /// The job is canceled
        /// </summary>
        CANCELED,
        /// <summary>
        /// The job is done
        /// </summary>
        DONE,
        /// <summary>
        /// Unknown status
        /// </summary>
        UNKNOWN
    }
}
