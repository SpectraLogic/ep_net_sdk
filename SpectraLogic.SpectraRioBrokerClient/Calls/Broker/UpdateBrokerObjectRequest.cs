﻿/*
 * ******************************************************************************
 *   Copyright 2014-2019 Spectra Logic Corporation. All Rights Reserved.
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
using SpectraLogic.SpectraRioBrokerClient.Utils;
using System;
using System.Collections.Generic;

namespace SpectraLogic.SpectraRioBrokerClient.Calls
{
    /// <summary></summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Calls.RestRequest" />
    public class UpdateBrokerObjectRequest : RestRequest
    {
        #region Fields

        /// <summary>The metadata</summary>
        [JsonProperty(PropertyName = "metadata")] public IDictionary<string, string> Metadata;

        /// <summary>The relationships</summary>
        [JsonProperty(PropertyName = "relationships")] public ISet<string> Relationships;

        #endregion Fields

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="UpdateBrokerObjectRequest"/> class.</summary>
        /// <param name="brokerName">Name of the broker.</param>
        /// <param name="objectName">Name of the object.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="relationships">The relationships.</param>
        public UpdateBrokerObjectRequest(string brokerName, string objectName, IDictionary<string, string> metadata, ISet<string> relationships)
        {
            Contract.Requires<ArgumentNullException>(brokerName != null, "brokerName");
            Contract.Requires<ArgumentNullException>(objectName != null, "objectName");
            Contract.Requires<ArgumentNullException>(metadata != null, "metadata");
            Contract.Requires<ArgumentNullException>(relationships != null, "relationships");

            BrokerName = brokerName;
            ObjectName = objectName;
            Metadata = metadata;
            Relationships = relationships;
        }

        #endregion Constructors

        #region Properties

        /// <summary>Gets the name of the broker.</summary>
        /// <value>The name of the broker.</value>
        [JsonIgnore] public string BrokerName { get; private set; }

        /// <summary>Gets the name of the object.</summary>
        /// <value>The name of the object.</value>
        [JsonIgnore] public string ObjectName { get; private set; }

        internal override string Path => $"/api/brokers/{BrokerName}/objects/{Uri.EscapeDataString(ObjectName)}";
        internal override HttpVerb Verb => HttpVerb.PUT;

        #endregion Properties

        #region Methods

        internal override string GetBody()
        {
            return JsonConvert.SerializeObject(this);
        }

        #endregion Methods
    }
}