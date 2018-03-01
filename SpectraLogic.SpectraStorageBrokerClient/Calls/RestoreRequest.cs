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
using SpectraLogic.SpectraStorageBrokerClient.Utils;
using System;
using System.Collections.Generic;

namespace SpectraLogic.SpectraStorageBrokerClient.Calls
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraStorageBrokerClient.Calls.RestRequest" />
    public class RestoreRequest : RestRequest
    {
        #region Fields

        /// <summary>
        /// The files to be restored
        /// </summary>
        [JsonProperty(PropertyName = "files")] public IEnumerable<RestoreFile> Files;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreRequest" /> class.
        /// </summary>
        /// <param name="brokerName">Name of the broker.</param>
        /// <param name="files">The files.</param>
        public RestoreRequest(string brokerName, IEnumerable<RestoreFile> files)
        {
            Contract.Requires<ArgumentNullException>(brokerName != null, "brokerName");
            Contract.Requires<ArgumentNullException>(files != null, "files");

            BrokerName = brokerName;
            Files = files;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the name of the broker.
        /// </summary>
        /// <value>
        /// The name of the broker.
        /// </value>
        [JsonIgnore] public string BrokerName { get; private set; }

        internal override string Path => $"api/brokers/{BrokerName}/restore";
        internal override HttpVerb Verb => HttpVerb.POST;

        #endregion Properties

        #region Methods

        internal override string GetBody()
        {
            return JsonConvert.SerializeObject(this);
        }

        #endregion Methods
    }
}