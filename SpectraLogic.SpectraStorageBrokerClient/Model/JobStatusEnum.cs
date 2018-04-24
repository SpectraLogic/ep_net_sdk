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


namespace SpectraLogic.SpectraStorageBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public enum JobStatusEnum
    {
        /// <summary>
        /// The job is active
        /// </summary>
        ACTIVE,

        /// <summary>
        /// The job is completed
        /// </summary>
        COMPLETED,

        /// <summary>
        /// The job is paused
        /// </summary>
        PAUSED,

        /// <summary>
        /// The job is canceled
        /// </summary>
        CANCELED,

        /// <summary>
        /// The job has error
        /// </summary>
        ERROR
    }
}