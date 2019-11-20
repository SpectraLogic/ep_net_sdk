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

using SpectraLogic.SpectraRioBrokerClient.Exceptions;
using SpectraLogic.SpectraRioBrokerClient.Model;
using System;
using System.Net;

namespace SpectraLogic.SpectraRioBrokerClient.Utils
{
    internal static class ExceptionDecorator
    {
        #region Public Methods

        public static T Run<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (ErrorResponseException ex)
            {
                if (ex.ErrorResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new AuthenticationFailureException(ex.ErrorResponse.ErrorMessage, ex);
                }

                if (ex.ErrorResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    var notFoundErrorResponse = ex.ErrorResponse as NotFoundErrorResponse;

                    switch (notFoundErrorResponse.ResourceType)
                    {
                        case ResourceType.BROKER:
                            throw new BrokerNotFoundException(ex.ErrorResponse.ErrorMessage, ex);

                        case ResourceType.JOB:
                            throw new JobNotFoundException(ex.ErrorResponse.ErrorMessage, ex);

                        case ResourceType.SPECTRA_DEVICE:
                            throw new DeviceNotFoundException(ex.ErrorResponse.ErrorMessage, ex);

                        case ResourceType.OBJECT:
                            throw new BrokerObjectNotFoundException(ex.ErrorResponse.ErrorMessage, ex);
                    }
                }

                if (ex.ErrorResponse.StatusCode == HttpStatusCode.Conflict)
                {
                    var conflictErrorResponse = ex.ErrorResponse as ConflictErrorResponse;

                    switch (conflictErrorResponse.ResourceType)
                    {
                        case ResourceType.SPECTRA_DEVICE:
                            throw new DeviceAlreadyExistsException(ex.ErrorResponse.ErrorMessage, ex);

                        case ResourceType.BROKER:
                            throw new BrokerAlreadyExistsException(ex.ErrorResponse.ErrorMessage, ex);

                        case ResourceType.CLUSTER:
                            throw new AlreadyAClusterMemberException(ex.ErrorResponse.ErrorMessage, ex);
                    }
                }

                if (ex.ErrorResponse.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    switch (ex.ErrorResponse.ErrorMessage)
                    {
                        case "The service is unavailable":
                        case "The node is not a member of a cluster":
                            throw new NodeIsNotAClusterMemberException(ex.ErrorResponse.ErrorMessage, ex);
                    }
                }

                if (ex.ErrorResponse.StatusCode == (HttpStatusCode)422)
                {
                    throw new ValidationException(ex.ErrorResponse.ErrorMessage, ex);
                }

                if (ex.ErrorResponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    switch (ex.ErrorResponse.ErrorMessage)
                    {
                        case "Missing Authorization Header":
                            throw new MissingAuthorizationHeaderException(ex.ErrorResponse.ErrorMessage, ex);
                    }
                }

                throw;
            }
        }

        #endregion Public Methods
    }
}