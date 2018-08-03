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

using log4net;
using Newtonsoft.Json;
using SpectraLogic.SpectraRioBrokerClient.Runtime;
using SpectraLogic.SpectraRioBrokerClient.Utils;
using System.IO;
using System.Linq;
using System.Net;

namespace SpectraLogic.SpectraRioBrokerClient.ResponseParsers
{
    internal class Parser<T>
    {
        #region Private Fields

        private static readonly ILog LOG = LogManager.GetLogger("Parser");

        #endregion Private Fields

        #region Public Methods

        public static T Parse(IHttpWebResponse response, params HttpStatusCode[] expectedStatusCodes)
        {
            using (response)
            {
                ResponseParseUtils.HandleStatusCode(response, expectedStatusCodes);
                using (var stream = response.GetResponseStream())
                using (var textStreamReader = new StreamReader(stream))
                {
                    var responseString = textStreamReader.ReadToEnd();
                    string requestId = response.Headers["request-id"].First();
                    LOG.Debug($"Request: {requestId}\n{responseString.JsonFormat()}");
                    return JsonConvert.DeserializeObject<T>(responseString);
                }
            }
        }

        #endregion Public Methods
    }
}