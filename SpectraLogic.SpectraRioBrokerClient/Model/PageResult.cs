using Newtonsoft.Json;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public class PageResult
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PageResult"/> class.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">The total pages.</param>
        public PageResult(long number, long pageSize, long totalPages)
        {
            Number = number;
            PageSize = pageSize;
            TotalPages = totalPages;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        [JsonProperty(PropertyName = "number")] public long Number { get; }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        [JsonProperty(PropertyName = "pageSize")] public long PageSize { get; }

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        /// <value>
        /// The total pages.
        /// </value>
        [JsonProperty(PropertyName = "totalPages")] public long TotalPages { get; }

        #endregion Public Properties
    }
}