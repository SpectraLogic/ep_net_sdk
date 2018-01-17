using log4net;
using SpectraLogic.EscapePodClient.Calls;
using SpectraLogic.EscapePodClient.Model;
using SpectraLogic.EscapePodClient.ResponseParsers;
using SpectraLogic.EscapePodClient.Runtime;

namespace SpectraLogic.EscapePodClient
{
    public class EscapePodClient : IEscapePodClient
    {
        private readonly ILog _log = LogManager.GetLogger("EscapePodClient");
        private readonly INetwork _network;

        internal EscapePodClient(INetwork network)
        {
            _network = network;
        }

        public IEscapePodArchive GetArchive(string archiveName)
        {
            _log.Info("GetArchive");
            return null;
        }

        public IEscapePodJob Delete()
        {
            _log.Info("Delete");
            return null;
        }

        public IEscapePodJob Restore()
        {
            _log.Info("Retore");
            return null;
        }

        public IEscapePodJob Archive(ArchiveRequest request)
        {
            _log.Debug($"Archive info\n{request}");
            return new ArchiveResponseParser().Parse(_network.Invoke(request));
        }

        public IEscapePodJob Cancel()
        {
            _log.InfoFormat("Cancel");
            return null;
        }
    }
}
