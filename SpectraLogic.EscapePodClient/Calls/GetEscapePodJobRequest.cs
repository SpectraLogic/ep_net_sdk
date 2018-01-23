namespace SpectraLogic.EscapePodClient.Calls
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="SpectraLogic.EscapePodClient.Calls.RestRequest" />
    public class GetEscapePodJobRequest : RestRequest
    {
        /// <summary>
        /// Gets the name of the archive.
        /// </summary>
        /// <value>
        /// The name of the archive.
        /// </value>
        public string ArchiveName { get; private set; }
        
        /// <summary>
        /// Gets the escape pod job identifier.
        /// </summary>
        /// <value>
        /// The escape pod job identifier.
        /// </value>
        public string EscapePodJobId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetEscapePodJobRequest"/> class.
        /// </summary>
        /// <param name="archiveName">Name of the archive.</param>
        /// <param name="escapePodJobId">The escape pod job identifier.</param>
        public GetEscapePodJobRequest(string archiveName, string escapePodJobId)
        {
            ArchiveName = archiveName;
            EscapePodJobId = escapePodJobId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetEscapePodJobRequest"/> class.
        /// </summary>
        /// <param name="archiveName">Name of the archive.</param>
        /// <param name="escapePodJobId">The escape pod job identifier.</param>
        public GetEscapePodJobRequest(string archiveName, long escapePodJobId)
        {
            ArchiveName = archiveName;
            EscapePodJobId = escapePodJobId.ToString();
        }

        internal override HttpVerb Verb => HttpVerb.GET;
        internal override string Path => $"api/archive/{ArchiveName}/jobs/{EscapePodJobId}";
        
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{Path}\n{Verb}";
        }
    }
}
