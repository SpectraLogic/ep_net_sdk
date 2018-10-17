using Newtonsoft.Json;
using System.Collections.Generic;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Model.IRelationshipObjects" />
    public class RelationshipObjects : IRelationshipObjects
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationshipObjects"/> class.
        /// </summary>
        /// <param name="objects">The objects.</param>
        public RelationshipObjects(IList<RelationshipObject> objects)
        {
            Objects = objects;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the objects.
        /// </summary>
        /// <value>
        /// The objects.
        /// </value>
        [JsonProperty(PropertyName = "objects")] public IList<RelationshipObject> Objects { get; }

        #endregion Public Properties
    }
}