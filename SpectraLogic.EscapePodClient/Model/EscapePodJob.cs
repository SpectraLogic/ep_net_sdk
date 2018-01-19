using System;
using System.Runtime.Serialization;

namespace SpectraLogic.EscapePodClient.Model
{
    [DataContract]
    public class EscapePodJob : IEscapePodJob
    {
        [DataMember(IsRequired = true)] public string Id { get; set; }
        [DataMember(Name = "Status", IsRequired = true)] public string StatusString { get; set; }

        public Status Status => Enum.TryParse(StatusString, true, out Status ret) ? ret : Status.UNKNOWN;
    }

    public enum Status
    {
        IN_PROGRESS,
        CANCELED,
        DONE,
        UNKNOWN
    }
}
