/*
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
    public class ArchiveBody
    {
        #region Fields

        /// <summary>
        /// The files to be archive
        /// </summary>
        [JsonProperty(PropertyName = "files")] public IEnumerable<ArchiveFile> Files;

        /// <summary>
        /// The job name
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.AllowNull)] public string JobName;

        #endregion Fields

        #region Constructors

        public ArchiveBody(string jobName, IEnumerable<ArchiveFile> files)
        {
            JobName = jobName;
            Files = files;
        }

        #endregion Constructors
    }

    /// <summary>
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Calls.RestRequest"/>
    public class ArchiveRequest : RestRequest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveRequest"/> class.
        /// </summary>
        /// <param name="brokerName">Name of the broker.</param>
        /// <param name="files">The files.</param>
        /// <param name="uploadNewFilesOnly">
        /// If enabled, when a file already exists in RioBroker it is ignored in the archive job and
        /// no update occurs; only new files are uploaded. Default = false.
        /// </param>
        /// <param name="failFast">
        /// If enabled, when a validation error occurs the job fails immediately. If disabled, the
        /// job continues even though a validation error occurred. Default = true.
        /// </param>
        /// <param name="jobName">Name of the job. Default = null.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ArchiveRequest(string brokerName, IEnumerable<ArchiveFile> files, bool uploadNewFilesOnly = false,
            bool failFast = true, string jobName = null)
        {
            Contract.Requires<ArgumentNullException>(brokerName != null, "brokerName");
            Contract.Requires<ArgumentNullException>(files != null, "files");

            BrokerName = brokerName;
            ArchiveBody = new ArchiveBody(jobName, files);

            AddQueryParam("upload-new-files-only", uploadNewFilesOnly.ToString());
            AddQueryParam("fail-fast", failFast.ToString());
        }

        #endregion Constructors

        #region Properties

        public ArchiveBody ArchiveBody { get; private set; }

        /// <summary>
        /// Gets the name of the broker.
        /// </summary>
        /// <value>The name of the broker.</value>
        [JsonIgnore]
        public string BrokerName { get; private set; }

        internal override string Path => $"/api/brokers/{BrokerName}/archive";
        internal override HttpVerb Verb => HttpVerb.POST;

        #endregion Properties

        #region Methods

        internal override string GetBody()
        {
            return JsonConvert.SerializeObject(ArchiveBody);
        }

        #endregion Methods
    }
}