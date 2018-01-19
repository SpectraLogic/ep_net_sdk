using System;
using System.Runtime.Serialization;

namespace SpectraLogic.EscapePodClient.Model
{
    [DataContract]
    public class EscapePodJobStatus : IEscapePodJobStatus
    {
        [DataMember(Name="Status")] public string StatusString { get; set; }

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
