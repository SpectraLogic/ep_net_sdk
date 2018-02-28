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

using System.Net;

namespace SpectraLogic.SpectraStorageBrokerClient.Runtime
{
    internal class SpectraStorageBrokerHttpWebRequest : IHttpWebRequest
    {
        #region Fields

        private readonly HttpWebRequest _httpWebRequest;

        #endregion Fields

        #region Constructors

        public SpectraStorageBrokerHttpWebRequest(HttpWebRequest httpWebRequest)
        {
            _httpWebRequest = httpWebRequest;
        }

        #endregion Constructors

        #region Methods

        public IHttpWebResponse GetResponse()
        {
            return new SpectraStorageBrokerHttpWebResponse((HttpWebResponse)_httpWebRequest.GetResponse());
        }

        #endregion Methods
    }
}