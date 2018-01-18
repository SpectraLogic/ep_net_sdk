using System.Runtime.Serialization;

namespace SpectraLogic.EscapePodClient.Model
{
    [DataContract]
    public class DeleteFile
    {
        [DataMember(Order = 1)] public string Name;

        public DeleteFile(string name)
        {
            Name = name;
        }
    }
}