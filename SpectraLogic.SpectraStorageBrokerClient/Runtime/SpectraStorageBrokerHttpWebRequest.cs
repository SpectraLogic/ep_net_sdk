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
using System.Net;
using System.Text;

namespace SpectraLogic.SpectraRioBrokerClient.Runtime
{
    internal class SpectraRioBrokerHttpWebRequest : IHttpWebRequest
    {
        #region Private Fields

        private readonly HttpWebRequest _httpWebRequest;

        #endregion Private Fields

        #region Public Constructors

        public SpectraRioBrokerHttpWebRequest(HttpWebRequest httpWebRequest)
        {
            _httpWebRequest = httpWebRequest;
        }

        #endregion Public Constructors

        #region Public Methods

        public IHttpWebResponse GetResponse()
        {
            return new SpectraRioBrokerHttpWebResponse((HttpWebResponse)_httpWebRequest.GetResponse());
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("HttpWebRequest information:");
            sb.AppendFormat("{0}{1} {2}", Environment.NewLine, _httpWebRequest.Method, _httpWebRequest.Address);
            sb.AppendFormat("{0}{1}", Environment.NewLine, _httpWebRequest.Headers.ToString().TrimEnd());

            return sb.ToString();
        }

        #endregion Public Methods
    }
}