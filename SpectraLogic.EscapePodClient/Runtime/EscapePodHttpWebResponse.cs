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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace SpectraLogic.EscapePodClient.Runtime
{
    internal class EscapePodHttpWebResponse : IHttpWebResponse
    {
        #region Constructors

        public EscapePodHttpWebResponse(HttpWebResponse httpWebResponse)
        {
            Response = httpWebResponse;
        }

        #endregion Constructors

        #region Properties

        public HttpStatusCode StatusCode => Response.StatusCode;
        public IDictionary<string, IEnumerable<string>> Headers => Response.Headers.AllKeys.ToDictionary<string, string, IEnumerable<string>>(key => key, key => Response.Headers.GetValues(key));
        private HttpWebResponse Response { get; }

        #endregion Properties

        #region Methods

        public Stream GetResponseStream()
        {
            return Response.GetResponseStream();
        }

        public void Dispose()
        {
            Dispose(true);
            Response.Close();
            GC.SuppressFinalize(this);
        }

        private static void Dispose(bool disposing)
        {
        }

        #endregion Methods
    }
}