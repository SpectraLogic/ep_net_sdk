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
using SpectraLogic.SpectraStorageBrokerClient.Model;
using System.Collections.Generic;

namespace SpectraLogic.SpectraStorageBrokerClient.Calls
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraStorageBrokerClient.Calls.RestRequest" />
    public class DeleteRequest : RestRequest
    {
        #region Public Fields

        /// <summary>
        /// The files to be deleted
        /// </summary>
        [JsonProperty(Order = 1, PropertyName = "files")] public IEnumerable<DeleteFile> Files;

        #endregion Public Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteRequest"/> class.
        /// </summary>
        /// <param name="files">The files.</param>
        public DeleteRequest(IEnumerable<DeleteFile> files)
        {
            Files = files;
        }

        #endregion Public Constructors

        #region Internal Properties

        internal override string Path => "api/delete"; //TODO use the right path
        internal override HttpVerb Verb => HttpVerb.DELETE;

        #endregion Internal Properties

        #region Internal Methods

        internal override string GetBody()
        {
            return JsonConvert.SerializeObject(this);
        }

        #endregion Internal Methods
    }
}