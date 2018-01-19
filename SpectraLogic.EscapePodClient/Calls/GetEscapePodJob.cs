namespace SpectraLogic.EscapePodClient.Calls
{
    public class GetEscapePodJob : RestRequest
    {
        public string ArchiveName { get; private set; }
        public string EscapePodJobId { get; private set; }

        public GetEscapePodJob(string archiveName, string escapePodJobId)
        {
            ArchiveName = archiveName;
            EscapePodJobId = escapePodJobId;
        }

        public GetEscapePodJob(string archiveName, long escapePodJobId)
        {
            ArchiveName = archiveName;
            EscapePodJobId = escapePodJobId.ToString();
        }

        internal override HttpVerb Verb => HttpVerb.GET;
        internal override string Path => $"api/archive/{ArchiveName}/jobs/{EscapePodJobId}";
        public override string ToString()
        {
            return $"{Path}\n{Verb}";
        }
    }
}
