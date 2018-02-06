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

using log4net;
using SpectraLogic.EscapePodClient.Calls;
using SpectraLogic.EscapePodClient.Exceptions;
using SpectraLogic.EscapePodClient.Model;
using SpectraLogic.EscapePodClient.ResponseParsers;
using SpectraLogic.EscapePodClient.Runtime;
using System.Net;

namespace SpectraLogic.EscapePodClient
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="SpectraLogic.EscapePodClient.IEscapePodClient" />
    public class EscapePodClient : IEscapePodClient
    {
        private static readonly ILog Log = LogManager.GetLogger("EscapePodClient");
        private readonly INetwork _network;

        internal EscapePodClient(INetwork network)
        {
            _network = network;
        }

        /// <summary>
        /// Gets the archive.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ArchiveNotFoundException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        /// <returns></returns>
        public IEscapePodArchive GetArchive(GetArchiveRequest request)
        {
            try
            {
                Log.Debug($"GetArchive info\n{request}");
                return new GetArchiveResponseParser().Parse(_network.Invoke(request));
            }
            catch (ErrorResponseException ex)
            {
                if (ex.ErrorResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new ArchiveNotFoundException(ex.ErrorResponse.ErrorMessage);
                }

                if (ex.ErrorResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new InvalidEscapePodServerCredentialsException(ex.ErrorResponse.ErrorMessage);
                }

                throw ex;
            }
        }

        /// <summary>
        /// Gets the job.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        /// <returns></returns>
        public IEscapePodJob GetJob(GetEscapePodJobRequest request)
        {
            try
            {
                Log.Debug($"GetJob info\n{request}");
                return new GetEscapePodJobParser().Parse(_network.Invoke(request));
            }
            catch (ErrorResponseException ex)
            {
                //TODO parse by status code and throw the right expection
                if (ex.ErrorResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new InvalidEscapePodServerCredentialsException(ex.ErrorResponse.ErrorMessage);
                }

                throw ex;
            }
        }

        /// <summary>
        /// Deletes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        /// <returns></returns>
        public IEscapePodJob Delete(DeleteRequest request)
        {
            try
            {
                Log.Debug($"Delete info\n{request}");
                return new DeleteResponseParser().Parse(_network.Invoke(request));
            }
            catch (ErrorResponseException ex)
            {
                //TODO parse by status code and throw the right expection
                if (ex.ErrorResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new InvalidEscapePodServerCredentialsException(ex.ErrorResponse.ErrorMessage);
                }

                throw ex;
            }
        }

        /// <summary>
        /// Restores the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        /// <returns></returns>
        public IEscapePodJob Restore(RestoreRequest request)
        {
            try
            {
                Log.Debug($"Retore info\n{request}");
                return new RestoreResponseParser().Parse(_network.Invoke(request));
            }
            catch (ErrorResponseException ex)
            {
                //TODO parse by status code and throw the right expection
                if (ex.ErrorResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new InvalidEscapePodServerCredentialsException(ex.ErrorResponse.ErrorMessage);
                }

                throw ex;
            }
        }

        /// <summary>
        /// Archives the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        /// <returns></returns>
        public IEscapePodJob Archive(ArchiveRequest request)
        {
            try
            {
                Log.Debug($"Archive info\n{request}");
                return new ArchiveResponseParser().Parse(_network.Invoke(request));
            }
            catch (ErrorResponseException ex)
            {
                //TODO parse by status code and throw the right expection
                if (ex.ErrorResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new InvalidEscapePodServerCredentialsException(ex.ErrorResponse.ErrorMessage);
                }

                throw ex;
            }
        }

        /// <summary>
        /// Cancels the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        /// <returns></returns>
        public IEscapePodJob Cancel(CancelRequest request)
        {
            try
            {
                Log.Debug($"Cancel info\n{request}");
                return new CancelResponseParser().Parse(_network.Invoke(request));
            }
            catch (ErrorResponseException ex)
            {
                //TODO parse by status code and throw the right expection
                if (ex.ErrorResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new InvalidEscapePodServerCredentialsException(ex.ErrorResponse.ErrorMessage);
                }

                throw ex;
            }
        }

        /// <summary>
        /// Creates the archive.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ArchiveAlreadyExistsException" />
        /// /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.BucketDoesNotExistException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        /// <returns></returns>
        public IEscapePodArchive CreateArchive(CreateArchiveRequest request)
        {
            try
            {
                Log.Debug($"CreateArchive info\n{request}");
                return new CreateArchiveResponseParser().Parse(_network.Invoke(request));
            }
            catch (ErrorResponseException ex)
            {
                //TODO parse by status code and throw the right expection
                if (ex.ErrorResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new InvalidEscapePodServerCredentialsException(ex.ErrorResponse.ErrorMessage);
                }

                if (ex.ErrorResponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    if (ex.ErrorResponse.ErrorMessage.Equals($"Archive {request.Name} already exists"))
                    {
                        throw new ArchiveAlreadyExistsException(ex.ErrorResponse.ErrorMessage);
                    }
                }

                if (ex.ErrorResponse.StatusCode == HttpStatusCode.NotFound)
                { 

                    if (ex.ErrorResponse.ErrorMessage.Equals($"Bucket {request.ResolverConfig.Bucket} does not exist"))
                    {
                        throw new BucketDoesNotExistException(ex.ErrorResponse.ErrorMessage);
                    }
                }

                throw ex;
            }
        }
    }
}
