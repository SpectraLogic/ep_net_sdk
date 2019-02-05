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

using SpectraLogic.SpectraRioBrokerClient.Calls;
using SpectraLogic.SpectraRioBrokerClient.Model;
using System;

namespace SpectraLogic.SpectraRioBrokerClient
{
    /// <summary>
    ///
    /// </summary>
    public interface ISpectraRioBrokerClient
    {
        #region Public Methods

        /// <summary>
        /// Archives the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ValidationException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerNotFoundException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        IJob Archive(ArchiveRequest request);

        /// <summary>
        /// Cancels the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        IJob Cancel(CancelRequest request);

        /// <summary>
        /// Creates a broker.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerAlreadyExistsException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ValidationException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        IBroker CreateBroker(CreateBrokerRequest request);

        /// <summary>
        /// Creates the cluster.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AlreadyAClusterMemberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        ICluster CreateCluster(CreateClusterRequest request);

        /// <summary>
        /// Creates a device.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.InvalidDeviceCredentialsException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.DeviceAlreadyExistsException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ValidationException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        IDevice CreateDevice(CreateDeviceRequest request);

        /// <summary>
        /// Creates the token.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ValidationException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        IToken CreateToken(CreateTokenRequest request);

        /// <summary>
        /// Deletes the cluster.
        /// </summary>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        void DeleteCluster();

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerNotFoundException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerObjectNotFoundException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        void DeleteFile(DeleteFileRequest request);

        /// <summary>
        /// Determines whether brokerName exist.
        /// </summary>
        /// <param name="brokerName">Name of the broker.</param>
        /// <returns>
        ///   <c>true</c> if specified brokerName exist; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        bool DoesBrokerExist(string brokerName);

        /// <summary>Determines whether the broker object exist.</summary>
        /// <param name="brokerName">Name of the broker.</param>
        /// <param name="objectName">Name of the object.</param>
        /// <returns><c>true</c> if specified objectName exist in brokerName; otherwise, <c>false</c>.</returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        bool DoesBrokerObjectExist(string brokerName, string objectName);

        /// <summary>
        /// Determines whether deviceName exist.
        /// </summary>
        /// <param name="deviceName">Name of the device.</param>
        /// <returns>
        ///   <c>true</c> if specified device exist; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        bool DoesDeviceExist(string deviceName);

        /// <summary>
        /// Determines whether jobId exist.
        /// </summary>
        /// <param name="jobId">The job id.</param>
        /// <returns>
        ///   <c>true</c> if specified job exist; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        bool DoesJobExist(Guid jobId);

        /// <summary>
        /// Gets the broker.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerNotFoundException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        /// <returns></returns>
        IBroker GetBroker(GetBrokerRequest request);

        /// <summary>Gets the object.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerNotFoundException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerObjectNotFoundException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        IBrokerObject GetBrokerObject(GetBrokerObjectRequest request);

        /// <summary>Gets the broker objects.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerNotFoundException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        IBrokerObjects GetBrokerObjects(GetBrokerObjectsRequest request);

        /// <summary>
        /// Gets the relationship objects.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerNotFoundException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        IBrokerObjects GetBrokerRelationship(GetBrokerRelationshipRequest request);

        /// <summary>
        /// Gets the brokers.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        /// <returns></returns>
        IBrokers GetBrokers(GetBrokersRequest request);

        /// <summary>
        /// Gets the cluster.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        ICluster GetCluster(GetClusterRequest request);

        /// <summary>
        /// Gets the device.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.DeviceNotFoundException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        IDevice GetDevice(GetDeviceRequest request);

        /// <summary>
        /// Gets the job.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.JobNotFoundException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        IJob GetJob(GetJobRequest request);

        /// <summary>
        /// Gets the jobs.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        IJobs GetJobs(GetJobsRequest request);

        /// <summary>
        /// Gets details about the master node in the cluster
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        IClusterMember GetMaster();

        /// <summary>
        /// Gets a list of all the members of the cluster
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        IClusterMembers GetMembers();

        /// <summary>
        /// Gets the system info.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IRioSystem GetSystem(GetSystemRequest request);

        /// <summary>
        /// Restores the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ValidationException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        IJob Restore(RestoreRequest request);

        /// <summary>
        /// Retries the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException" />
        IJob Retry(RetryRequest request);

        /// <summary>
        /// Updates the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        void UpdateToken(string token);

        #endregion Public Methods
    }
}