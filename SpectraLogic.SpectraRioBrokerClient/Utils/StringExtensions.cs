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

using System;
using Newtonsoft.Json;

namespace SpectraLogic.SpectraRioBrokerClient.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>Format the given string to a Json format</summary>
        /// <param name="str">The string to format.</param>
        /// <returns></returns>
        public static string JsonFormat(this string str)
        {
            dynamic json = JsonConvert.DeserializeObject(str);
            return JsonConvert.SerializeObject(json, Formatting.Indented);
        }

        /// <summary>Converts to universal DateTime object.</summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str) =>
            DateTime.Parse(str.Replace("[UTC]", "")).ToUniversalTime();

        /// <summary>Converts to Uri object.</summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static Uri ToUri(this string str) => new Uri(str);
    }
}