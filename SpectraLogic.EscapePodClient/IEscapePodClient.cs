using SpectraLogic.EscapePodClient.Calls;
using SpectraLogic.EscapePodClient.Model;

namespace SpectraLogic.EscapePodClient
{
    public interface IEscapePodClient
    {
        void VerifyArchiveName(string archiveName);
        IEscapePodJob Delete();
        IEscapePodJob Restore();
        IEscapePodJob Archive(ArchiveRequest request);
        IEscapePodJob Cancel();
    }
}
