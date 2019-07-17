﻿/*
 * ******************************************************************************
 *   Copyright 2014-2019 Spectra Logic Corporation. All Rights Reserved.
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

namespace SpectraLogic.SpectraRioBrokerClient.Runtime
{
    internal class SpectraRioBrokerHttpWebResponse : IHttpWebResponse
    {
        #region Constructors

        public SpectraRioBrokerHttpWebResponse(HttpWebResponse httpWebResponse)
        {
            Response = httpWebResponse;
        }

        #endregion Constructors

        #region Properties

        public IDictionary<string, IEnumerable<string>> Headers => Response.Headers.AllKeys.ToDictionary<string, string, IEnumerable<string>>(key => key, key => Response.Headers.GetValues(key));
        public HttpStatusCode StatusCode => Response.StatusCode;
        private HttpWebResponse Response { get; }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            Dispose(true);
            Response.Close();
            GC.SuppressFinalize(this);
        }

        public Stream GetResponseStream()
        {
            return Response.GetResponseStream();
        }

        private static void Dispose(bool disposing)
        {
        }

        #endregion Methods
    }
}