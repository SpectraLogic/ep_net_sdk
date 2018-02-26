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
using SpectraLogic.EscapePodClient.Calls;
using SpectraLogic.EscapePodClient.Exceptions;
using SpectraLogic.EscapePodClient.Model;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading;

namespace SpectraLogic.EscapePodClient.Integration.Test
{
    public static class EscapePodClientFixture
    {
        #region Public Fields

        public static readonly string BlackPearlBucket = "ep_net_sdk_tests";
        public static readonly string BlackPearlUserName = "Administrator";
        public static readonly string ResolverName = "bp_resolver";

        #endregion Public Fields

        #region Private Fields

        private static readonly string[] Files = { "F1.txt", "F2.txt" };

        #endregion Private Fields

        #region Public Constructors

        static EscapePodClientFixture()
        {
            BasicConfigurator.Configure();

            CreateClient();
            CreateCluster();
            CreateDevice();
            CreateArchive();

            SetupTestData();
        }

        #endregion Public Constructors

        #region Public Properties

        public static string ArchiveName { get; private set; }
        public static string ArchiveTempDir { get; private set; }
        public static string ClusterName { get; private set; }
        public static string DataPort { get; private set; }
        public static string DeviceName { get; private set; }
        public static string Endpoint { get; private set; }
        public static IEscapePodClient EscapePodClient { get; private set; }
        public static string Password { get; private set; }
        public static string RestoreTempDir { get; private set; }
        public static string Username { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static void CreateArchive()
        {
            ArchiveName = ConfigurationManager.AppSettings["ArchiveName"];

            if (!EscapePodClient.DoesArchiveExist(ArchiveName))
            {
                var resolver = GetResolver();
                var createArchiveRequest = new CreateArchiveRequest(ArchiveName, resolver);
                EscapePodClient.CreateArchive(createArchiveRequest);
            }
        }

        public static void CreateCluster()
        {
            ClusterName = ConfigurationManager.AppSettings["ClusterName"];
            var getClusterRequest = new GetClusterRequest();
            try
            {
                EscapePodClient.GetCluster(getClusterRequest);
            }
            catch (NodeIsNotAClusterMemeberException)
            {
                var createClusterRequest = new CreateClusterRequest(ClusterName);
                EscapePodClient.CreateCluster(createClusterRequest);

                //TODO remove this sleep once ESCP-154 is fixed
                Thread.Sleep(30 * 1000);
            }
        }

        public static void CreateDevice()
        {
            DeviceName = ConfigurationManager.AppSettings["DeviceName"];
            Endpoint = ConfigurationManager.AppSettings["Endpoint"];
            Username = ConfigurationManager.AppSettings["Username"];
            Password = ConfigurationManager.AppSettings["Password"];
            DataPort = ConfigurationManager.AppSettings["DataPort"];

            if (!EscapePodClient.DoesDeviceExist(DeviceName))
            {
                var createDeviceRequest = new CreateDeviceRequest(DeviceName, Endpoint, Username, Password);
                EscapePodClient.CreateDevice(createDeviceRequest);
            }
        }

        public static ResolverConfig GetResolver()
        {
            return new ResolverConfig(ResolverName, DeviceName, BlackPearlUserName, BlackPearlBucket, false);
        }

        #endregion Public Methods

        #region Private Methods

        private static void CreateClient()
        {
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

        private static void SetupArchiveTestData(string tempDir)
        {
            ArchiveTempDir = $"{tempDir}archvie";
            if (!Directory.Exists(ArchiveTempDir))
            {
                Directory.CreateDirectory(ArchiveTempDir);
            }

            foreach (var file in Files)
            {
                using (var writer = File.OpenWrite(ArchiveTempDir + "/" + file))
                using (var fileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SpectraLogic.EscapePodClient.Integration.Test.TestFiles." + file))
                {
                    fileStream.CopyTo(writer);
                    fileStream.Seek(0, SeekOrigin.Begin);
                }
            }
        }

        private static void SetupRestoreTestData(string tempDir)
        {
            RestoreTempDir = $"{tempDir}restore";
            if (!Directory.Exists(RestoreTempDir))
            {
                Directory.CreateDirectory(RestoreTempDir);
            }
        }

        private static void SetupTestData()
        {
            var tempDir = Path.GetTempPath();

            SetupArchiveTestData(tempDir);
            SetupRestoreTestData(tempDir);
        }

        #endregion Private Methods
    }
}