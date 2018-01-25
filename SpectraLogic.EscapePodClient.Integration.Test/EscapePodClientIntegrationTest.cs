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
using log4net.Config;
using NUnit.Framework;
using SpectraLogic.EscapePodClient.Calls;
using SpectraLogic.EscapePodClient.Model;
using System.Collections.Generic;
using System.Configuration;

namespace SpectraLogic.EscapePodClient.Integration.Test
{
    [TestFixture]
    public class EscapePodClientIntegrationTest
    {
        ILog _log = LogManager.GetLogger("EscapePodClientIntegrationTest");
        IEscapePodClient EscapePodClient;

        [OneTimeSetUp]
        public void Setup()
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

        [Test]
        public void Archive()
        {
            var archiveRequest = new ArchiveRequest("net_archive", new List<ArchiveFile>
            {
                new ArchiveFile("F1", "file:///C:/Users/sharons/Documents/GitHub/ep_net_sdk/SpectraLogic.EscapePodClient.Integration.Test/TestFiles/F1.txt", 14, new Dictionary<string, string>{ { "fileName", "F1" } }, false, false),
                new ArchiveFile("F2", "file:///C:/Users/sharons/Documents/GitHub/ep_net_sdk/SpectraLogic.EscapePodClient.Integration.Test/TestFiles/F2.txt", 14, new Dictionary<string, string>{ { "fileName", "F2" } }, false, false)
            });

            var job = EscapePodClient.Archive(archiveRequest);
        }


        [Test]
        public void Restore()
        {
            //TODO create a temp dir for the restore
            var restoreRequest = new RestoreRequest("net_archive", new List<RestoreFile>
            {
                new RestoreFile("F1", "file:///C:/Temp/restore/F1_restore.txt"),
                new RestoreFile("F2", "file:///C:/Temp/restore/F2_restore.txt")
            });

            var job = EscapePodClient.Restore(restoreRequest);
        }
    }
}
