using SpectraLogic.EscapePodClient.Calls;
using SpectraLogic.EscapePodClient.Model;

namespace SpectraLogic.EscapePodClient
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEscapePodClient
    {
        /**
         *
         */
        IEscapePodArchive GetArchive(string archiveName);

        /**
         *
         */
        IEscapePodJob Delete();

        /**
         *
         */
        IEscapePodJob Restore(RestoreRequest request);

        /**
         *
         */
        IEscapePodJob Archive(ArchiveRequest request);

        /**
         *
         */
        IEscapePodJob Cancel();
    }
}
