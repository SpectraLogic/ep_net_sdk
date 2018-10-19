using Newtonsoft.Json;
using System.Collections.Generic;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Model.IRelationship" />
    public class Relationship : IRelationship
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Relationship"/> class.
        /// </summary>
        /// <param name="results">The results.</param>
        public Relationship(IList<RelationshipResult> results)
        {
            Results = results;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        [JsonProperty(PropertyName = "page")] public RelationshipPage Page { get; }

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        [JsonProperty(PropertyName = "result")] public IList<RelationshipResult> Results { get; }

        #endregion Public Properties
    }
}