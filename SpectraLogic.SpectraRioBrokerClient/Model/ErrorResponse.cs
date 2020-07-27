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
using System.Net;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public class ErrorResponse
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="statusCode">The status code.</param>
        [JsonConstructor]
        protected ErrorResponse(string errorMessage, HttpStatusCode statusCode)
        {
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        [JsonProperty(PropertyName = "message")] public string ErrorMessage { get; }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        [JsonProperty(PropertyName = "statusCode")] public HttpStatusCode StatusCode { get; }

        #endregion Properties
    }
}