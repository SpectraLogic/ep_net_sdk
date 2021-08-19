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
using log4net;
using SpectraLogic.SpectraRioBrokerClient.Calls.Authentication;
using SpectraLogic.SpectraRioBrokerClient.Calls.Broker;
using SpectraLogic.SpectraRioBrokerClient.Calls.Cluster;
using SpectraLogic.SpectraRioBrokerClient.Calls.Devices;
using SpectraLogic.SpectraRioBrokerClient.Calls.Jobs;
using SpectraLogic.SpectraRioBrokerClient.Calls.System;
using SpectraLogic.SpectraRioBrokerClient.Model;
using SpectraLogic.SpectraRioBrokerClient.ResponseParsers;
using SpectraLogic.SpectraRioBrokerClient.ResponseParsers.Authentication;
using SpectraLogic.SpectraRioBrokerClient.ResponseParsers.Broker;
using SpectraLogic.SpectraRioBrokerClient.ResponseParsers.Cluster;
using SpectraLogic.SpectraRioBrokerClient.ResponseParsers.Devices;
using SpectraLogic.SpectraRioBrokerClient.ResponseParsers.Jobs;
using SpectraLogic.SpectraRioBrokerClient.ResponseParsers.System;
using SpectraLogic.SpectraRioBrokerClient.Runtime;
using SpectraLogic.SpectraRioBrokerClient.Utils;

namespace SpectraLogic.SpectraRioBrokerClient
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.ISpectraRioBrokerClient" />
    public class SpectraRioBrokerClient : ISpectraRioBrokerClient
    {
        #region Constructors

        internal SpectraRioBrokerClient(INetwork network)
        {
            _network = network;
        }

        #endregion Constructors

        #region Fields

        private static readonly ILog Log = LogManager.GetLogger("SpectraRioBrokerClient");
        private readonly INetwork _network;

        #endregion Fields

        #region Methods

        /// <inheritdoc/>
        public IJob Archive(ArchiveRequest request)
        {
            return ExceptionDecorator.Run(() => new ArchiveResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public IJob Cancel(CancelRequest request)
        {
            return ExceptionDecorator.Run(() => new CancelResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public IBroker CreateBroker(CreateBrokerRequest request)
        {
            return ExceptionDecorator.Run(() => new CreateBrokerResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public ICluster CreateCluster(CreateClusterRequest request)
        {
            return ExceptionDecorator.Run(() => new CreateClusterResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public ISpectraDevice CreateSpectraDevice(CreateSpectraDeviceRequest request)
        {
            return ExceptionDecorator.Run(() => new CreateSpectraDeviceResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public IToken CreateToken(CreateTokenRequest request)
        {
            return ExceptionDecorator.Run(() => new CreateTokenResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public void DeleteBroker(DeleteBrokerRequest request)
        {
            ExceptionDecorator.Run(() => new DeleteBrokerResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public void DeleteCluster()
        {
            ExceptionDecorator.Run(() =>
            {
                var request = new DeleteClusterRequest();
                return new DeleteClusterResponseParser().Parse(_network.Invoke(request));
            });
        }

        /// <inheritdoc/>
        public void DeleteSpectraDevice(DeleteSpectraDeviceRequest request)
        {
            ExceptionDecorator.Run(() => new DeleteSpectraDeviceResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public void DeleteFile(DeleteFileRequest request)
        {
            ExceptionDecorator.Run(() => new DeleteFileResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public bool DoesBrokerExist(string brokerName)
        {
            return ExceptionDecorator.Run(() =>
            {
                var request = new HeadBrokerRequest(brokerName);
                return new HeadResponseParser().Parse(_network.Invoke(request));
            });
        }

        /// <inheritdoc/>
        public bool DoesBrokerObjectExist(string brokerName, string objectName)
        {
            return ExceptionDecorator.Run(() =>
            {
                var request = new HeadBrokerObjectRequest(brokerName, objectName);
                return new HeadResponseParser().Parse(_network.Invoke(request));
            });
        }

        /// <inheritdoc/>
        public bool DoesSpectraDeviceExist(string deviceName)
        {
            return ExceptionDecorator.Run(() =>
            {
                var request = new HeadSpectraDeviceRequest(deviceName);
                return new HeadResponseParser().Parse(_network.Invoke(request));
            });
        }

        /// <inheritdoc/>
        public bool DoesJobExist(Guid jobId)
        {
            return ExceptionDecorator.Run(() =>
            {
                var request = new HeadJobRequest(jobId);
                return new HeadResponseParser().Parse(_network.Invoke(request));
            });
        }

        /// <inheritdoc/>
        public IBroker GetBroker(GetBrokerRequest request)
        {
            return ExceptionDecorator.Run(() => new GetBrokerResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public IBrokerObject GetBrokerObject(GetBrokerObjectRequest request)
        {
            return ExceptionDecorator.Run(() => new GetBrokerObjectResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public IBrokerObjects GetBrokerObjects(GetBrokerObjectsRequest request)
        {
            return ExceptionDecorator.Run(() => new GetBrokerObjectsResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public IBrokers GetBrokers(GetBrokersRequest request)
        {
            return ExceptionDecorator.Run(() => new GetBrokersResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public ICluster GetCluster(GetClusterRequest request)
        {
            return ExceptionDecorator.Run(() => new GetClusterResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public ISpectraDevice GetSpectraDevice(GetSpectraDeviceRequest request)
        {
            return ExceptionDecorator.Run(() => new GetSpectraDeviceResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public ISpectraDevices GetSpectraDevices(GetSpectraDevicesRequest request)
        {
            return ExceptionDecorator.Run(() =>  new GetSpectraDevicesResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public IJob GetJob(GetJobRequest request)
        {
            return ExceptionDecorator.Run(() => new GetJobResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public IFilesStatus GetJobFilesStatus(GetJobFilesStatusRequest request)
        {
            return ExceptionDecorator.Run(() => new GetJobFilesStatusResponseParser().Parse(_network.Invoke(request)));
        }
        
        /// <inheritdoc/>
        public IFileStatuses GetJobFileStatuses(GetJobFileStatusesRequest request)
        {
            return ExceptionDecorator.Run(() => new GetJobFileStatusesResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public IJobs GetJobs(GetJobsRequest request)
        {
            return ExceptionDecorator.Run(() => new GetJobsResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public IClusterMember GetMaster()
        {
            return ExceptionDecorator.Run(() =>
            {
                var request = new GetMasterRequest();
                return new GetMasterResponseParser().Parse(_network.Invoke(request));
            });
        }

        /// <inheritdoc/>
        public IClusterMembers GetMembers()
        {
            return ExceptionDecorator.Run(() =>
            {
                var request = new GetMembersRequest();
                return new GetMembersResponseParser().Parse(_network.Invoke(request));
            });
        }

        /// <inheritdoc/>
        public IRioSystem GetSystem()
        {
            return ExceptionDecorator.Run(() =>
            {
                var request = new GetSystemRequest();
                return new GetSystemResponseParser().Parse(_network.Invoke(request));
            });
        }

        /// <inheritdoc/>
        public IJob Restore(RestoreRequest request)
        {
            return ExceptionDecorator.Run(() => new RestoreResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public IJob Retry(RetryRequest request)
        {
            return ExceptionDecorator.Run(() => new RetryResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public IBrokerObject UpdateBrokerObject(UpdateBrokerObjectRequest request)
        {
            return ExceptionDecorator.Run(() => new UpdateBrokerObjectResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc />
        public IJob UpdateJobPriority(UpdateJobPriorityRequest request)
        {
            return ExceptionDecorator.Run(() => new UpdateJobPriorityResponseParser().Parse(_network.Invoke(request)));
        }

        /// <inheritdoc/>
        public void UpdateToken(string token)
        {
            Contract.Requires<ArgumentNullException>(token != null, "token");

            _network.UpdateToken(token);
        }

        #endregion Methods
    }
}