/*
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
using System.Linq;
using log4net;

namespace SpectraLogic.SpectraRioBrokerClient.Utils
{
    public static class DictionaryExtensions
    {
        private static readonly ILog Log = LogManager.GetLogger("DictionaryExtensions");
        
        public static string GetRequestIdFromHeader(this IDictionary<string, IEnumerable<string>> headers)
        {
            
            try
            {
                headers.TryGetValue("request-id", out var requestId);
                return requestId.FirstOrDefault();
            } 
            catch (Exception)
            {
                Log.Error("request-id header was not found");
                return null;
            }
        }
    }
}