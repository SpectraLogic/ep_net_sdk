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

using SpectraLogic.SpectraRioBrokerClient.Model;
using System;
using System.Collections.Generic;

namespace SpectraLogic.SpectraRioBrokerClient.Exceptions
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ValidationException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
            ValidationErrors = ExtractValidationErrors(innerException);
        }

        #endregion Constructors

        #region Properties

        /// <summary>Gets the validation errors.</summary>
        /// <value>The validation errors.</value>
        public IEnumerable<ValidationError> ValidationErrors { get; private set; }

        #endregion Properties

        #region Methods

        private IEnumerable<ValidationError> ExtractValidationErrors(Exception ex)
        {
            var errorResponseException = ex as ErrorResponseException;
            var validationErrorResponse = errorResponseException.ErrorResponse as ValidationErrorResponse;

            return validationErrorResponse.Errors;
        }

        #endregion Methods
    }
}