/*
 * ******************************************************************************
 *   Copyright 2014-2018 Spectra Logic Corporation. All Rights Reserved.
 *   Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *   this file except in compliance with the License. A copy of the License is located at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 *   or in the "license" file accompanying this file.
 *   This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *   CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *   specific language governing permissions and limitations under the License.
 * ****************************************************************************
 */

using Newtonsoft.Json;
using SpectraLogic.EscapePodClient.Model;

namespace SpectraLogic.EscapePodClient.Calls
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.EscapePodClient.Calls.RestRequest" />
    public class CreateArchiveRequest : RestRequest
    {
        #region Fields

        /// <summary>
        /// The name
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "name")] public string Name;

        /// <summary>
        /// The resolver configuration
        /// </summary>
        [JsonProperty(Order = 2, PropertyName = "resolverConfig")] public ResolverConfig ResolverConfig;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateArchiveRequest" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="resolver">The resolver.</param>
        public CreateArchiveRequest(string name, ResolverConfig resolver)
        {
            Name = name;
            ResolverConfig = resolver;
        }

        #endregion Constructors

        #region Properties

        internal override HttpVerb Verb => HttpVerb.POST;
        internal override string Path => "/api/archives";

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{Path}\n{Verb}\n{GetBody()}";
        }

        internal override string GetBody()
        {
            return JsonConvert.SerializeObject(this);
        }

        #endregion Methods
    }
}