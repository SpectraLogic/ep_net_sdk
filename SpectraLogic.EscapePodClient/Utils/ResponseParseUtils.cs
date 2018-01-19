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
using System.IO;
using System.Linq;
using System.Net;
using SpectraLogic.EscapePodClient.Runtime;

namespace SpectraLogic.EscapePodClient.Utils
{
    internal static class ResponseParseUtils
    {
        internal static void HandleStatusCode(IHttpWebResponse response, params HttpStatusCode[] expectedStatusCodes)
        {
            var actualStatusCode = response.StatusCode;
            if (expectedStatusCodes.Contains(actualStatusCode))
            {
                return;
            }

            var responseContent = GetResponseContent(response);
            throw new Exception(responseContent);
        }

        private static string GetResponseContent(IHttpWebResponse response)
        {
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}
