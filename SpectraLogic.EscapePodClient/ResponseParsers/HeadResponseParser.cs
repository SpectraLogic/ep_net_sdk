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
using SpectraLogic.EscapePodClient.Exceptions;
using SpectraLogic.EscapePodClient.Model;
using SpectraLogic.EscapePodClient.Runtime;
using System.Net;

namespace SpectraLogic.EscapePodClient.ResponseParsers
{
    internal class HeadResponseParser : IResponseParser<bool>
    {
        #region Fields

        private static ILog LOG = LogManager.GetLogger("HeadResponseParser");

        #endregion Fields

        #region Methods

        public bool Parse(IHttpWebResponse response)
        {
            using (response)
            {
                LOG.Debug(response.StatusCode);

                if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>("{\"message\":\"The service is unavailable\",\"statusCode\":503}");
                    throw new ErrorResponseException(errorResponse);
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }

                return false;
            }
        }

        #endregion Methods
    }
}