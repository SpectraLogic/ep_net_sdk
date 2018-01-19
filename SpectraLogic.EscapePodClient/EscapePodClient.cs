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

namespace SpectraLogic.EscapePodClient
{
    public class EscapePodClient : IEscapePodClient
    {
        private static readonly ILog Log = LogManager.GetLogger("EscapePodClient");
        private readonly INetwork _network;

        internal EscapePodClient(INetwork network)
        {
            _network = network;
        }

        public IEscapePodArchive GetArchive(GetArchiveRequest request)
        {
            Log.Debug($"GetArchive info\n{request}");
            return new GetArchiveResponseParser().Parse(_network.Invoke(request));
        }

        public IEscapePodJobStatus GetJobStatus(GetEscapePodJobStatus request)
        {
            Log.Debug($"GetJobStatus info\n{request}");
            return new GetEscapePodJobStatusParser().Parse(_network.Invoke(request));
        }

        public IEscapePodJob Delete(DeleteRequest request)
        {
            Log.Debug($"Delete info\n{request}");
            return new DeleteResponseParser().Parse(_network.Invoke(request));
        }

        public IEscapePodJob Restore(RestoreRequest request)
        {
            Log.Debug($"Retore info\n{request}");
            return new RestoreResponseParser().Parse(_network.Invoke(request));
        }

        public IEscapePodJob Archive(ArchiveRequest request)
        {
            Log.Debug($"Archive info\n{request}");
            return new ArchiveResponseParser().Parse(_network.Invoke(request));
        }

        public IEscapePodJob Cancel(CancelRequest request)
        {
            Log.Debug($"Cancel info\n{request}");
            return new CancelResponseParser().Parse(_network.Invoke(request));
        }

        public IEscapePodArchive CreateArchive(CreateArchiveRequest request)
        {
            Log.Debug($"CreateArchive info\n{request}");
            return new CreateArchiveResponseParser().Parse(_network.Invoke(request));
        }
    }
}
