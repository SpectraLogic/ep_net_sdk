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

using SpectraLogic.SpectraStorageBrokerClient.Calls;
using SpectraLogic.SpectraStorageBrokerClient.Model;

namespace SpectraLogic.SpectraStorageBrokerClient
{
    /// <summary>
    ///
    /// </summary>
    public interface ISpectraStorageBrokerClient
    {
        #region Public Methods

        /// <summary>
        /// Archives the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.InvalidServerCredentialsException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.ValidationException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.ErrorResponseException" />
        IJob Archive(ArchiveRequest request);

        /// <summary>
        /// Cancels the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.InvalidServerCredentialsException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.ErrorResponseException" />
        IJob Cancel(CancelRequest request);

        /// <summary>
        /// Creates a broker.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.InvalidServerCredentialsException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.BrokerAlreadyExistsException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.ValidationException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.ErrorResponseException" />
        IBroker CreateBroker(CreateBrokerRequest request);

        /// <summary>
        /// Creates the cluster.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.InvalidServerCredentialsException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.AlreadyAClusterMemberException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.ErrorResponseException" />
        ICluster CreateCluster(CreateClusterRequest request);

        /// <summary>
        /// Creates a device.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.InvalidServerCredentialsException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.InvalidDeviceCredentialsException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.DeviceAlreadyExistsException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.ValidationException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.ErrorResponseException" />
        IDevice CreateDevice(CreateDeviceRequest request);

        /// <summary>
        /// Deletes the cluster.
        /// </summary>
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.InvalidServerCredentialsException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.ErrorResponseException" />
        void DeleteCluster();

        /// <summary>
        /// Deletes the specified files.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.InvalidServerCredentialsException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.ErrorResponseException" />
        IJob DeleteFiles(DeleteFilesRequest request);

        /// <summary>
        /// Determines whether brokerName exist.
        /// </summary>
        /// <param name="brokerName">Name of the broker.</param>
        /// <returns>
        ///   <c>true</c> if specified brokerName exist; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.InvalidServerCredentialsException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.ErrorResponseException" />
        bool DoesBrokerExist(string brokerName);

        /// <summary>
        /// Determines whether deviceName exist.
        /// </summary>
        /// <param name="deviceName">Name of the device.</param>
        /// <returns>
        ///   <c>true</c> if specified device exist; otherwise, <c>false</c>.
        /// </returns>
        bool DoesDeviceExist(string deviceName);

        /// <summary>
        /// Gets the broker.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.BrokerNotFoundException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.InvalidServerCredentialsException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.ErrorResponseException" />
        /// <returns></returns>
        IBroker GetBroker(GetBrokerRequest request);

        /// <summary>
        /// Gets the cluster.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.InvalidServerCredentialsException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.ErrorResponseException" />
        ICluster GetCluster(GetClusterRequest request);

        /// <summary>
        /// Gets the device.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.InvalidServerCredentialsException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.DeviceNotFoundException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.ErrorResponseException" />
        IDevice GetDevice(GetDeviceRequest request);

        /// <summary>
        /// Gets the job.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.InvalidServerCredentialsException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.JobNotFoundException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.ErrorResponseException" />
        IJob GetJob(GetJobRequest request);

        /// <summary>
        /// Restores the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.InvalidServerCredentialsException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.ValidationException" />
        /// <exception cref="SpectraLogic.SpectraStorageBrokerClient.Exceptions.ErrorResponseException" />
        IJob Restore(RestoreRequest request);

        #endregion Public Methods
    }
}