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

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public class JobFileStatus
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JobFileStatus"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="status">The status.</param>
        /// <param name="statusMessage">The status message.</param>
        public JobFileStatus(string name, string status, string statusMessage)
        {
            Name = name;
            Status = status;
            StatusMessage = statusMessage;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty(PropertyName = "name")] public string Name { get; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonProperty(PropertyName = "status")] public string Status { get; }

        /// <summary>
        /// Gets the status message.
        /// </summary>
        /// <value>
        /// The status message.
        /// </value>
        [JsonProperty(PropertyName = "statusMessage")] public string StatusMessage { get; }

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
            return $"<name: {Name}, status: {Status}, statusMessage: {StatusMessage}>";
        }

        #endregion Methods
    }
}