using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SpectraLogic.EscapePodClient.Model
{
    [DataContract]
    public class ArchiveFile
    {
        [DataMember] public string Name;
        [DataMember] public string Uri;
        [DataMember] public long Size;
        [DataMember] public IDictionary<string, string> Metadata;
        [DataMember] public IEnumerable<string> Links;

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
