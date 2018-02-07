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

using System.IO;
using System.Net;
using log4net;
using Newtonsoft.Json;
using SpectraLogic.EscapePodClient.Model;
using SpectraLogic.EscapePodClient.Runtime;
using SpectraLogic.EscapePodClient.Utils;

namespace SpectraLogic.EscapePodClient.ResponseParsers
{
    internal class GetEscapePodJobParser : IResponseParser<IEscapePodJob>
    {
        private static readonly ILog LOG = LogManager.GetLogger("GetEscapePodJobParser");

        public IEscapePodJob Parse(IHttpWebResponse response)
        {
            using (response)
            {
                ResponseParseUtils.HandleStatusCode(response, HttpStatusCode.OK); //TODO use the right status code
                using (var stream = response.GetResponseStream())
                using (var textStreamReader = new StreamReader(stream))
                {
                    var responseString = textStreamReader.ReadToEnd();
                    LOG.Debug(responseString);
                    return JsonConvert.DeserializeObject<EscapePodJob>(responseString);
                }
            }
        }
    }
}
