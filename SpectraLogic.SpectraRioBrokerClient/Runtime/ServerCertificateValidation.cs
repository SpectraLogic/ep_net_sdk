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

using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace SpectraLogic.SpectraRioBrokerClient.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServerCertificateValidation
    {
        /// <summary>
        /// Overrides the validation.
        /// </summary>
        public static void OverrideValidation()
        {
            //Adding Tls11 and Tls12 to the exsiting Security Protocol to support HTTPS requests
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)SslProtocols.Tls11 |
                                                    (SecurityProtocolType)SslProtocols.Tls12;

            //Adding Server Certificate Validation
            ServicePointManager.ServerCertificateValidationCallback = Validator;
        }

        private static bool Validator(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
