﻿/*
 * ******************************************************************************
 *   Copyright 2014-2020 Spectra Logic Corporation. All Rights Reserved.
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
using SpectraLogic.SpectraRioBrokerClient.Model;
using SpectraLogic.SpectraRioBrokerClient.Utils;
using System;
using System.Collections.Generic;

namespace SpectraLogic.SpectraRioBrokerClient.Calls.Broker
{
    /// <summary>
    /// </summary>
    public class RestoreBody
    {
        #region Fields

        /// <summary>
        /// The files to be restore
        /// </summary>
        [JsonProperty(PropertyName = "files")] public IEnumerable<RestoreFile> Files;

        /// <summary>
        /// The job name
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.AllowNull)]
        public string JobName;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreBody"/> class.
        /// </summary>
        /// <param name="jobName">Name of the job.</param>
        /// <param name="files">The files.</param>
        public RestoreBody(string jobName, IEnumerable<RestoreFile> files)
        {
            JobName = jobName;
            Files = files;
        }

        #endregion Constructors
    }

    /// <summary>
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Calls.RestRequest"/>
    public class RestoreRequest : RestRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreRequest"/> class.
        /// </summary>
        /// <param name="brokerName">Name of the broker.</param>
        /// <param name="files">The files.</param>
        /// <param name="ignoreDuplicates">
        /// if set ignore duplicate items in a restore job if they appear in multiple brokers when
        /// doing a search then restore operation.
        /// </param>
        /// <param name="jobName">Name of the job. Default = null.</param>
        /// <param name="jobPriority">Sets the job priority. Null if not specified.</param>
        /// <param name="failFast">If enabled, when a validation error occurs the job fails immediately. If disabled, the job continues even though a validation error occurred. Default = true.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public RestoreRequest(string brokerName, IEnumerable<RestoreFile> files, bool ignoreDuplicates = false,
            string jobName = null, JobPriority? jobPriority = null, bool failFast = true)
        {
            Contract.Requires<ArgumentNullException>(brokerName != null, "brokerName");
            Contract.Requires<ArgumentNullException>(files != null, "files");

            BrokerName = brokerName;
            RestoreBody = new RestoreBody(jobName, files);

            AddQueryParam("ignore-duplicates", ignoreDuplicates.ToString());
            if (jobPriority != null)
            {
                AddQueryParam("priority", jobPriority.ToString().ToUpper());
            }
            AddQueryParam("fail-fast", failFast.ToString());
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the name of the broker.
        /// </summary>
        /// <value>The name of the broker.</value>
        [JsonIgnore]
        public string BrokerName { get; private set; }

        /// <summary>
        /// Gets the restore body.
        /// </summary>
        /// <value>The restore body.</value>
        public RestoreBody RestoreBody { get; private set; }

        internal override string Path => $"api/brokers/{BrokerName}/restore";
        internal override HttpVerb Verb => HttpVerb.POST;

        #endregion Properties

        #region Methods

        internal override string GetBody()
        {
            return JsonConvert.SerializeObject(RestoreBody);
        }

        #endregion Methods
    }
}