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
        IEscapePodArchive GetArchive(GetArchiveRequest request);

        /**
         *
         */
        IEscapePodJob Delete(DeleteRequest request);

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
        IEscapePodJob Cancel(CancelRequest request);
    }
}
