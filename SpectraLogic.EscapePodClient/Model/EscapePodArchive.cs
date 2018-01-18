using System.Runtime.Serialization;

namespace SpectraLogic.EscapePodClient.Model
{
    [DataContract]
    public class EscapePodArchive : IEscapePodArchive
    {
        [DataMember] public string Name { get; set; }
    }
}
