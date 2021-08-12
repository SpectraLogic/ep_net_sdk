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

using System;
using SpectraLogic.SpectraRioBrokerClient.Calls.Authentication;
using SpectraLogic.SpectraRioBrokerClient.Calls.Broker;
using SpectraLogic.SpectraRioBrokerClient.Calls.Cluster;
using SpectraLogic.SpectraRioBrokerClient.Calls.Devices;
using SpectraLogic.SpectraRioBrokerClient.Calls.Jobs;
using SpectraLogic.SpectraRioBrokerClient.Model;

namespace SpectraLogic.SpectraRioBrokerClient
{
    /// <summary>
    ///
    /// </summary>
    public interface ISpectraRioBrokerClient
    {
        #region Methods

        /// <summary>Archives the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ValidationException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerNotFoundException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IJob Archive(ArchiveRequest request);

        /// <summary>Cancels the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IJob Cancel(CancelRequest request);

        /// <summary>Creates a broker.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerAlreadyExistsException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ValidationException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IBroker CreateBroker(CreateBrokerRequest request);

        /// <summary>Creates the cluster.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AlreadyAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        ICluster CreateCluster(CreateClusterRequest request);

        /// <summary>Creates a spectra device.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.InvalidDeviceCredentialsException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.DeviceAlreadyExistsException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ValidationException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        ISpectraDevice CreateSpectraDevice(CreateSpectraDeviceRequest request);

        /// <summary>Creates the token.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ValidationException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IToken CreateToken(CreateTokenRequest request);

        /// <summary>Deletes the broker.</summary>
        /// <param name="request">The request.</param>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerNotFoundException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        void DeleteBroker(DeleteBrokerRequest request);

        /// <summary>Deletes the cluster.</summary>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        void DeleteCluster();

        /// <summary>Deletes the spectra device.</summary>
        /// <param name="request">The request.</param>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.DeviceNotFoundException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        void DeleteSpectraDevice(DeleteSpectraDeviceRequest request);
        
        /// <summary>Deletes the specified file.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerNotFoundException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerObjectNotFoundException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        void DeleteFile(DeleteFileRequest request);

        /// <summary>Determines whether brokerName exist.</summary>
        /// <param name="brokerName">Name of the broker.</param>
        /// <returns>
        ///   <c>true</c> if specified brokerName exist; otherwise, <c>false</c>.</returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        bool DoesBrokerExist(string brokerName);

        /// <summary>Determines whether the broker object exist.</summary>
        /// <param name="brokerName">Name of the broker.</param>
        /// <param name="objectName">Name of the object.</param>
        /// <returns>
        ///   <c>true</c> if specified objectName exist in brokerName; otherwise, <c>false</c>.</returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        bool DoesBrokerObjectExist(string brokerName, string objectName);

        /// <summary>Determines whether deviceName exist.</summary>
        /// <param name="deviceName">Name of the device.</param>
        /// <returns>
        ///   <c>true</c> if specified device exist; otherwise, <c>false</c>.</returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        bool DoesSpectraDeviceExist(string deviceName);

        /// <summary>Determines whether jobId exist.</summary>
        /// <param name="jobId">The job id.</param>
        /// <returns>
        ///   <c>true</c> if specified job exist; otherwise, <c>false</c>.</returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        bool DoesJobExist(Guid jobId);

        /// <summary>Gets the broker.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerNotFoundException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IBroker GetBroker(GetBrokerRequest request);

        /// <summary>Gets the object.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerNotFoundException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerObjectNotFoundException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IBrokerObject GetBrokerObject(GetBrokerObjectRequest request);

        /// <summary>Gets the broker objects.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerNotFoundException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IBrokerObjects GetBrokerObjects(GetBrokerObjectsRequest request);

        /// <summary>Gets the brokers.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IBrokers GetBrokers(GetBrokersRequest request);

        /// <summary>Gets the cluster.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        ICluster GetCluster(GetClusterRequest request);

        /// <summary>Gets the spectra device.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.DeviceNotFoundException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        ISpectraDevice GetSpectraDevice(GetSpectraDeviceRequest request);

        /// <summary>Gets the spectra devices.</summary>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        ISpectraDevices GetSpectraDevices(GetSpectraDevicesRequest request);
        
        /// <summary>Gets the job.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.JobNotFoundException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IJob GetJob(GetJobRequest request);

        /// <summary>Gets the job file status log.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.JobNotFoundException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IFilesStatus GetJobFilesStatus(GetJobFilesStatusRequest request);
        
        /// <summary>Gets the job file status log.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.JobNotFoundException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IFileStatuses GetJobFileStatuses(GetJobFileStatusesRequest request);
        
        /// <summary>Gets the jobs.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IJobs GetJobs(GetJobsRequest request);

        /// <summary>Gets details about the master node in the cluster</summary>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IClusterMember GetMaster();

        /// <summary>Gets a list of all the members of the cluster</summary>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IClusterMembers GetMembers();

        /// <summary>Gets the system info.</summary>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IRioSystem GetSystem();

        /// <summary>Restores the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ValidationException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IJob Restore(RestoreRequest request);

        /// <summary>Retries the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IJob Retry(RetryRequest request);

        /// <summary>Updates the broker object.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerNotFoundException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.BrokerObjectNotFoundException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IBrokerObject UpdateBrokerObject(UpdateBrokerObjectRequest request);

        /// <summary>Updates the job priority.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.MissingAuthorizationHeaderException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.AuthenticationFailureException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.NodeIsNotAClusterMemberException"></exception>
        /// <exception cref="SpectraLogic.SpectraRioBrokerClient.Exceptions.ErrorResponseException"></exception>
        IJob UpdateJobPriority(UpdateJobPriorityRequest request);
        
        /// <summary>Updates the token.</summary>
        /// <param name="token">The token.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        void UpdateToken(string token);

        #endregion Methods
    }
}