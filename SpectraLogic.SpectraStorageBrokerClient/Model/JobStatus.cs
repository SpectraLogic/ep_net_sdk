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

namespace SpectraLogic.SpectraStorageBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public class JobStatus
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JobStatus"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="status">The status.</param>
        public JobStatus(string message, JobStatusEnum status)
        {
            Message = message;
            Status = status;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [JsonProperty(PropertyName = "message")] public string Message { get; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonProperty(PropertyName = "status")] public JobStatusEnum Status { get; }

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
            return string.Format("<message: {0}, status: {1}>", Message, Status);
        }

        #endregion Methods
    }
}