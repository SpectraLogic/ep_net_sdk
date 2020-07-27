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
using SpectraLogic.SpectraRioBrokerClient.Utils;

namespace SpectraLogic.SpectraRioBrokerClient.Integration.Test
{
    internal static class StringExtensions
    {
        #region Public Methods

        public static Uri ToFileUri(this string str) => $"file://{str}".ToUri();

        public static Uri ToHttpsUri(this string str) => $"https://{str}".ToUri();

        public static Uri ToAtoZUri(this string str) => $"aToZSequence:///{str}".ToUri();

        public static Uri ToDevNullUri(this string str) => $"null:///{str}".ToUri();

        #endregion Public Methods
    }
}