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

using log4net.Config;
using System.Configuration;

namespace SpectraLogic.EscapePodClient.Integration.Test
{
    public static class EscapePodClientFixture
    {
        public static IEscapePodClient EscapePodClient { get; private set; }
        public static string ArchiveName = "net_archive_test";

        static EscapePodClientFixture()
        {
            BasicConfigurator.Configure();

            var escapePodClientBuilder = new EscapePodClientBuilder(
                ConfigurationManager.AppSettings["ServerName"],
                int.Parse(ConfigurationManager.AppSettings["ServerPort"]),
                ConfigurationManager.AppSettings["UserName"],
                ConfigurationManager.AppSettings["Password"]);

            var proxy = ConfigurationManager.AppSettings["Proxy"];
            if (!string.IsNullOrWhiteSpace(proxy))
            {
                escapePodClientBuilder.WithProxy(proxy);
            }

            EscapePodClient = escapePodClientBuilder.Build();
        }
    }
}
