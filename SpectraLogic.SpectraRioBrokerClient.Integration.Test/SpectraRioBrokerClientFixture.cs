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
using SpectraLogic.SpectraRioBrokerClient.Calls;
using SpectraLogic.SpectraRioBrokerClient.Exceptions;
using SpectraLogic.SpectraRioBrokerClient.Model;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace SpectraLogic.SpectraRioBrokerClient.Integration.Test
{
    public static class SpectraRioBrokerClientFixture
    {
        #region Public Fields

        public static readonly string AgentName = "bp_agent";
        public static readonly string BlackPearlBucket = "ep_net_sdk_tests";
        public static readonly string BlackPearlUserName = "Administrator";

        #endregion Public Fields

        #region Private Fields

        private static readonly string[] Files = { "F1.txt", "F2.txt" };

        #endregion Private Fields

        #region Public Constructors

        static SpectraRioBrokerClientFixture()
        {
            BasicConfigurator.Configure();

            CreateClient();
            CreateCluster();
            CreateDevice();
            CreateBroker();
        }

        #endregion Public Constructors

        #region Public Properties

        public static string ArchiveTempDir { get; private set; }
        public static string BrokerName { get; private set; }
        public static string ClusterName { get; private set; }
        public static string DataInterface { get; private set; }
        public static string DeviceName { get; private set; }
        public static string MgmtInterface { get; private set; }
        public static string Password { get; private set; }
        public static string RestoreTempDir { get; private set; }
        public static ISpectraRioBrokerClient SpectraRioBrokerClient { get; private set; }
        public static string Username { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static void CreateBroker()
        {
            BrokerName = ConfigurationManager.AppSettings["BrokerName"];

            if (!SpectraRioBrokerClient.DoesBrokerExist(BrokerName))
            {
                var agentConfig = GetAgentConfig();
                var createBrokerRequest = new CreateBrokerRequest(BrokerName, agentConfig);
                SpectraRioBrokerClient.CreateBroker(createBrokerRequest);
            }
        }

        public static void CreateCluster()
        {
            ClusterName = ConfigurationManager.AppSettings["ClusterName"];
            var getClusterRequest = new GetClusterRequest();
            try
            {
                SpectraRioBrokerClient.GetCluster(getClusterRequest);
            }
            catch (NodeIsNotAClusterMemeberException)
            {
                var createClusterRequest = new CreateClusterRequest(ClusterName);
                SpectraRioBrokerClient.CreateCluster(createClusterRequest);
            }
        }

        public static void CreateDevice()
        {
            DeviceName = ConfigurationManager.AppSettings["DeviceName"];
            MgmtInterface = ConfigurationManager.AppSettings["MgmtInterface"];
            Username = ConfigurationManager.AppSettings["Username"];
            Password = ConfigurationManager.AppSettings["Password"];
            DataInterface = ConfigurationManager.AppSettings["DataInterface"];

            if (!SpectraRioBrokerClient.DoesDeviceExist(DeviceName))
            {
                var createDeviceRequest = new CreateDeviceRequest(DeviceName, MgmtInterface, Username, Password);
                SpectraRioBrokerClient.CreateDevice(createDeviceRequest);
            }
        }

        public static AgentConfig GetAgentConfig()
        {
            return new AgentConfig(AgentName, DeviceName, BlackPearlUserName, BlackPearlBucket, false);
        }

        #endregion Public Methods

        #region Private Methods

        public static void SetupTestData()
        {
            var tempDir = Path.GetTempPath();

            SetupArchiveTestData(tempDir);
            SetupRestoreTestData(tempDir);
        }

        private static void CreateClient()
        {
            var SpectraRioBrokerClientBuilder = new SpectraRioBrokerClientBuilder(
                ConfigurationManager.AppSettings["ServerName"],
                int.Parse(ConfigurationManager.AppSettings["ServerPort"]),
                ConfigurationManager.AppSettings["UserName"],
                ConfigurationManager.AppSettings["Password"]);

            var proxy = ConfigurationManager.AppSettings["Proxy"];
            if (!string.IsNullOrWhiteSpace(proxy))
            {
                SpectraRioBrokerClientBuilder.WithProxy(proxy);
            }

            SpectraRioBrokerClient = SpectraRioBrokerClientBuilder.Build();
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
                using (var fileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SpectraLogic.SpectraRioBrokerClient.Integration.Test.TestFiles." + file))
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

        #endregion Private Methods
    }
}