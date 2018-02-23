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

using SpectraLogic.EscapePodClient.Calls;
using SpectraLogic.EscapePodClient.Model;

namespace SpectraLogic.EscapePodClient
{
    /// <summary>
    ///
    /// </summary>
    public interface IEscapePodClient
    {
        #region Public Methods

        /// <summary>
        /// Gets the archive.
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ArchiveNotFoundException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.NodeIsNotAClusterMemeberException" />
        IEscapePodArchive GetArchive(GetArchiveRequest request);
        /// Gets the job.
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        IEscapePodJob GetJob(GetEscapePodJobRequest request);
        /// Deletes the specified request.
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ArchiveAlreadyExistsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ValidationException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        IEscapePodJob Delete(DeleteRequest request);
        /// Restores the specified request.
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.AlreadyAClusterMemberException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        IEscapePodJob Restore(RestoreRequest request);
        /// Creates the device.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidDeviceCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.DeviceAlreadyExistsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ValidationException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        IEscapePodDevice CreateDevice(CreateDeviceRequest request);

        /// <summary>
        /// Deletes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        IEscapePodJob Delete(DeleteRequest request);

        /// <summary>
        /// Deletes the cluster.
        /// </summary>
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        void DeleteCluster();

        /// <summary>
        /// Creates the cluster.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        IEscapePodCluster CreateCluster(CreateClusterRequest request);

        /// <summary>
        /// Determines whether [does archive exist] [the specified archive name].
        /// </summary>
        /// <param name="archiveName">Name of the archive.</param>
        /// <returns>
        ///   <c>true</c> if [does device exist] [the specified device name]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        bool DoesArchiveExist(string archiveName);

        /// <summary>
        /// Gets the device.
        /// </summary>
        /// <param name="deviceName">Name of the device.</param>
        /// <returns>
        ///   <c>true</c> if [does device exist] [the specified device name]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        IEscapePodDevice GetDevice(GetDeviceRequest request);

        /// <summary>
        /// Creates the cluster.
        /// </summary>
        void DeleteCluster();

        /// <summary>
        /// Determines whether [does archive exist] [the specified archive name].
        /// </summary>
        /// <param name="archiveName">Name of the archive.</param>
        /// <returns>
        ///   <c>true</c> if [does archive exist] [the specified archive name]; otherwise, <c>false</c>.
        /// </returns>
        bool DoesArchiveExist(string archiveName);

        /// <summary>
        /// Determines whether [does device exist] [the specified device name].
        /// </summary>
        /// <param name="deviceName">Name of the device.</param>
        /// <returns>
        ///   <c>true</c> if [does device exist] [the specified device name]; otherwise, <c>false</c>.
        /// </returns>
        bool DoesDeviceExist(string deviceName);

        /// <summary>
        /// Gets the archive.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ArchiveNotFoundException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        /// <returns></returns>
        IEscapePodArchive GetArchive(GetArchiveRequest request);

        /// <summary>
        /// Gets the cluster.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        IEscapePodCluster GetCluster(GetClusterRequest request);

        /// <summary>
        /// Gets the device.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.DeviceNotFoundException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        IEscapePodDevice GetDevice(GetDeviceRequest request);

        /// <summary>
        /// Gets the job.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        bool IsArchiveExist(string archiveName);
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.EscapePodJobNotFoundException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        IEscapePodJob GetJob(GetEscapePodJobRequest request);

        /// <summary>
        /// Restores the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.InvalidEscapePodServerCredentialsException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.NodeIsNotAClusterMemeberException" />
        /// <exception cref="SpectraLogic.EscapePodClient.Exceptions.ErrorResponseException" />
        IEscapePodJob Restore(RestoreRequest request);

        #endregion Public Methods
    }
}