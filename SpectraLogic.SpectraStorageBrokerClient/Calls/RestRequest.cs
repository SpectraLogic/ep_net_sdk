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

using System;
using System.Collections.Generic;

namespace SpectraLogic.SpectraStorageBrokerClient.Calls
{
    /// <summary>
    ///
    /// </summary>
    public abstract class RestRequest
    {
        #region Properties

        internal abstract string Path { get; }
        internal Dictionary<string, string> QueryParams { get; } = new Dictionary<string, string>();
        internal abstract HttpVerb Verb { get; }

        #endregion Properties

        #region Methods

        internal virtual string GetBody()
        {
            return "";
        }

        /// <summary>
        /// Adds the query parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected void AddQueryParam(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                QueryParams.Add(key, value);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        #endregion Methods
    }
}