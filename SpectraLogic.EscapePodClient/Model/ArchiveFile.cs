using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SpectraLogic.EscapePodClient.Model
{
    [DataContract]
    public class ArchiveFile
    {
        [DataMember(Order = 1)] public string Name;
        [DataMember(Order = 2)] public string Uri;
        [DataMember(Order = 3)] public long Size;
        [DataMember(Order = 4)] public IDictionary<string, string> Metadata;
        [DataMember(Order = 5)] public IEnumerable<string> Links;

        public ArchiveFile(string name, string uri, long size, IDictionary<string, string> metadata, IEnumerable<string> links)
        {
            Name = name;
            Uri = uri;
            Size = size;
            Metadata = metadata;
            Links = links;
        }
    }
}
