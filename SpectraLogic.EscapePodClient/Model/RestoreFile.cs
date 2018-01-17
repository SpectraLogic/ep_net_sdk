using System.Runtime.Serialization;

namespace SpectraLogic.EscapePodClient.Model
{
    [DataContract]
    public class RestoreFile
    {
        [DataMember(Order = 1)] private string Name;
        [DataMember(Order = 2)] private string Destination;
        [DataMember(Order = 3)] private bool RestoreFileAttributes;
        [DataMember(Order = 4)] private ByteRange ByteRange;
        [DataMember(Order = 5)] private TimecodeRange TimeCodeRange;

        public RestoreFile(string name, string destination, bool restoreFileAttributes)
        {
            Name = name;
            Destination = destination;
            RestoreFileAttributes = restoreFileAttributes;
        }

        public RestoreFile(string name, string destination, ByteRange byteRange)
        {
            Name = name;
            Destination = destination;
            ByteRange = byteRange;
        }

        public RestoreFile(string name, string destination, TimecodeRange timeCodeRange)
        {
            Name = name;
            Destination = destination;
            TimeCodeRange = timeCodeRange;
        }
    }
}
