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
using SpectraLogic.SpectraStorageBrokerClient.Calls;
using SpectraLogic.SpectraStorageBrokerClient.Model;
using SpectraLogic.SpectraStorageBrokerClient.ResponseParsers;
using SpectraLogic.SpectraStorageBrokerClient.Runtime;
using SpectraLogic.SpectraStorageBrokerClient.Utils;

namespace SpectraLogic.SpectraStorageBrokerClient
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraStorageBrokerClient.ISpectraStorageBrokerClient" />
    public class SpectraStorageBrokerClient : ISpectraStorageBrokerClient
    {
        #region Private Fields

        private static readonly ILog Log = LogManager.GetLogger("SpectraStorageBrokerClient");
        private readonly INetwork _network;

        #endregion Private Fields

        #region Internal Constructors

        internal SpectraStorageBrokerClient(INetwork network)
        {
            _network = network;
        }

        #endregion Internal Constructors

        #region Public Methods

        public IJob Archive(ArchiveRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"Archive info\n{request}");
                return new ArchiveResponseParser().Parse(_network.Invoke(request));
            });
        }

        public IJob Cancel(CancelRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"Cancel info\n{request}");
                return new CancelResponseParser().Parse(_network.Invoke(request));
            });
        }

        public IBroker CreateBroker(CreateBrokerRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"CreateBroker info\n{request}");
                return new CreateBrokerResponseParser().Parse(_network.Invoke(request));
            });
        }

        public ICluster CreateCluster(CreateClusterRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"CreateCluster info\n{request}");
                return new CreateClusterResponseParser().Parse(_network.Invoke(request));
            });
        }

        public IDevice CreateDevice(CreateDeviceRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"CreateDevice info\n{request}");
                return new CreateDeviceResponseParser().Parse(_network.Invoke(request));
            });
        }

        public IJob Delete(DeleteRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"Delete info\n{request}");
                return new DeleteResponseParser().Parse(_network.Invoke(request));
            });
        }

        public void DeleteCluster()
        {
            ExceptionDecorator.Run(() =>
            {
                var request = new DeleteClusterRequest();
                Log.Debug($"DeleteCluster info\n{request}");
                return new DeleteClusterResponseParser().Parse(_network.Invoke(request));
            });
        }

        public bool DoesBrokerExist(string brokerName)
        {
            return ExceptionDecorator.Run(() =>
            {
                var request = new HeadBrokerRequest(brokerName);
                Log.Debug($"DoesBrokerExist info\n{request}");
                return new HeadResponseParser().Parse(_network.Invoke(request));
            });
        }

        public bool DoesDeviceExist(string deviceName)
        {
            return ExceptionDecorator.Run(() =>
            {
                var request = new HeadDeviceRequest(deviceName);
                Log.Debug($"DoesDeviceExist info\n{request}");
                return new HeadResponseParser().Parse(_network.Invoke(request));
            });
        }

        public IBroker GetBroker(GetBrokerRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"GetBroker info\n{request}");
                return new GetBrokerResponseParser().Parse(_network.Invoke(request));
            });
        }

        public ICluster GetCluster(GetClusterRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"GetCluster info\n{request}");
                return new GetClusterResponseParser().Parse(_network.Invoke(request));
            });
        }

        public IDevice GetDevice(GetDeviceRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"GetDevice info\n{request}");
                return new GetDeviceResponseParser().Parse(_network.Invoke(request));
            });
        }

        public IJob GetJob(GetJobRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"GetJob info\n{request}");
                return new GetJobParser().Parse(_network.Invoke(request));
            });
        }

        public IJob Restore(RestoreRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"Retore info\n{request}");
                return new RestoreResponseParser().Parse(_network.Invoke(request));
            });
        }

        #endregion Public Methods
    }
}