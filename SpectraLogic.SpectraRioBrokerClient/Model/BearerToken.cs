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

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Model.IToken" />
    public class BearerToken : IToken
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public BearerToken(string token)
        {
            Token = token;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [JsonProperty(PropertyName = "token")] public string Token { get; }

        #endregion Public Properties
    }
}