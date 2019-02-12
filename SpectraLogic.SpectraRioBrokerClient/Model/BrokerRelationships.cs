using Newtonsoft.Json;
using System.Collections.Generic;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Model.IBrokerRelationships" />
    public class BrokerRelationships : IBrokerRelationships
    {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="BrokerObjects"/> class.</summary>
        /// <param name="relationships">The relationships.</param>
        /// <param name="page">The page.</param>
        public BrokerRelationships(IList<string> relationships, PageResult page)
        {
            Relationships = relationships;
            Page = page;
        }

        #endregion Constructors

        #region Properties

        /// <summary>Gets the page.</summary>
        /// <value>The page.</value>
        [JsonProperty(PropertyName = "page")] public PageResult Page { get; }

        /// <summary>Gets the relationships.</summary>
        /// <value>The relationships.</value>
        [JsonProperty(PropertyName = "relationships")] public IList<string> Relationships { get; }

        #endregion Properties
    }
}