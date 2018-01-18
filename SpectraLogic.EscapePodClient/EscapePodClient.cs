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

        public IEscapePodArchive GetArchive(string archiveName)
        {
            Log.Info("GetArchive");
            return null;
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

        public IEscapePodJob Cancel()
        {
            Log.InfoFormat("Cancel");
            return null;
        }
    }
}
