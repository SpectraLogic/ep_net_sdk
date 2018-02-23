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
using SpectraLogic.EscapePodClient.Model;
using SpectraLogic.EscapePodClient.ResponseParsers;
using SpectraLogic.EscapePodClient.Runtime;
using SpectraLogic.EscapePodClient.Utils;

namespace SpectraLogic.EscapePodClient
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.EscapePodClient.IEscapePodClient" />
    public class EscapePodClient : IEscapePodClient
    {
        #region Private Fields

        private static readonly ILog Log = LogManager.GetLogger("EscapePodClient");
        private readonly INetwork _network;

        #endregion Private Fields

        #region Internal Constructors

        internal EscapePodClient(INetwork network)
        {
            _network = network;
        }

        #endregion Internal Constructors

        #region Public Methods

        public IEscapePodJob Archive(ArchiveRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"Archive info\n{request}");
                return new ArchiveResponseParser().Parse(_network.Invoke(request));
            });
        }

        public IEscapePodJob Cancel(CancelRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"Cancel info\n{request}");
                return new CancelResponseParser().Parse(_network.Invoke(request));
            });
        }

        public IEscapePodArchive CreateArchive(CreateArchiveRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"CreateArchive info\n{request}");
                return new CreateArchiveResponseParser().Parse(_network.Invoke(request));
            });
        }

        public IEscapePodCluster CreateCluster(CreateClusterRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"CreateCluster info\n{request}");
                return new CreateClusterResponseParser().Parse(_network.Invoke(request));
            });
        }

        public IEscapePodDevice CreateDevice(CreateDeviceRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"CreateDevice info\n{request}");
                return new CreateDeviceResponseParser().Parse(_network.Invoke(request));
            });
        }

        public IEscapePodJob Delete(DeleteRequest request)
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

        public bool DoesArchiveExist(string archiveName)
        {
            return ExceptionDecorator.Run(() =>
            {
                var request = new HeadArchiveRequest(archiveName);
                Log.Debug($"IsArchiveExist info\n{request}");
                return new HeadResponseParser().Parse(_network.Invoke(request));
            });
        }

        public bool DoesDeviceExist(string deviceName)
        {
            return ExceptionDecorator.Run(() =>
            {
                var request = new HeadDeviceRequest(deviceName);
                Log.Debug($"IsDeviceExist info\n{request}");
                return new HeadResponseParser().Parse(_network.Invoke(request));
            });
        }

        public IEscapePodArchive GetArchive(GetArchiveRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"GetArchive info\n{request}");
                return new GetArchiveResponseParser().Parse(_network.Invoke(request));
            });
        }

        public IEscapePodCluster GetCluster(GetClusterRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"GetCluster info\n{request}");
                return new GetClusterResponseParser().Parse(_network.Invoke(request));
            });
        }

        public IEscapePodDevice GetDevice(GetDeviceRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"GetDevice info\n{request}");
                return new GetDeviceResponseParser().Parse(_network.Invoke(request));
            });
        }

        public IEscapePodJob GetJob(GetEscapePodJobRequest request)
        {
            return ExceptionDecorator.Run(() =>
            {
                Log.Debug($"GetJob info\n{request}");
                return new GetEscapePodJobParser().Parse(_network.Invoke(request));
            });
        }

        public IEscapePodJob Restore(RestoreRequest request)
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