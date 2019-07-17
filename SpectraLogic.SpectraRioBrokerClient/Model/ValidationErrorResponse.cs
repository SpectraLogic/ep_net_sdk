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

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Model.ErrorResponse" />
    public class ValidationErrorResponse : ErrorResponse
    {
        #region Constructors

        [JsonConstructor]
        private ValidationErrorResponse(string errorMessage, HttpStatusCode statusCode, IEnumerable<ValidationError> errors)
            : base(errorMessage, statusCode)
        {
            Errors = errors;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        [JsonProperty(PropertyName = "errors")] public IEnumerable<ValidationError> Errors { get; }

        #endregion Properties
    }
}