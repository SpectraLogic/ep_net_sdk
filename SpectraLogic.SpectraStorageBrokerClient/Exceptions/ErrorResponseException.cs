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

using SpectraLogic.SpectraStorageBrokerClient.Model;
using System;

namespace SpectraLogic.SpectraStorageBrokerClient.Exceptions
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ErrorResponseException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseException"/> class.
        /// </summary>
        /// <param name="errorResponse">The error response.</param>
        public ErrorResponseException(ErrorResponse errorResponse)
        {
            ErrorResponse = errorResponse;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the error response.
        /// </summary>
        /// <value>
        /// The error response.
        /// </value>
        public ErrorResponse ErrorResponse { get; private set; }

        #endregion Properties
    }
}