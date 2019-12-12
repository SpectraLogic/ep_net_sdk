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

using System;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    /// ///
    /// </summary>
    public interface ISpectraDevice
    {
        #region Properties

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        string DeviceName { get; }

        /// <summary>
        /// Gets the MGMT interface.
        /// </summary>
        /// <value>The MGMT interface.</value>
        Uri MgmtInterface { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>The username.</value>
        string Username { get; }

        #endregion Properties
    }
}