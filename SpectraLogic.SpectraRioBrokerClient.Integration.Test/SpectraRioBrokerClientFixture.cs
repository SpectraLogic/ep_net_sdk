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
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading;
using log4net.Config;
using SpectraLogic.SpectraRioBrokerClient.Calls.Authentication;
using SpectraLogic.SpectraRioBrokerClient.Calls.Broker;
using SpectraLogic.SpectraRioBrokerClient.Calls.Cluster;
using SpectraLogic.SpectraRioBrokerClient.Calls.DevicesSpectra;
using SpectraLogic.SpectraRioBrokerClient.Exceptions;
using SpectraLogic.SpectraRioBrokerClient.Model;
using SpectraLogic.SpectraRioBrokerClient.Utils;

namespace SpectraLogic.SpectraRioBrokerClient.Integration.Test
{
    public static class SpectraRioBrokerClientFixture
    {
        #region Public Fields

        public static readonly string AgentName = "bp_agent";
        public static readonly string BlackPearlBucket = "ep_net_sdk_tests";
        public static readonly string BlackPearlBucket2 = "ep_net_sdk_tests_2";
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
            UpdateClientToken();
            Thread.Sleep(500); //we need this sleep to let the server create the default user after the cluster gets created
            CreateDevice();
            CreateBrokers();
        }

        #endregion Public Constructors

        #region Public Properties

        public static string ArchiveTempDir { get; private set; }
        public static string BrokerName { get; private set; }
        public static string BrokerName2 { get; private set; }
        public static string ClusterName { get; private set; }
        public static Uri DataInterface { get; private set; }
        public static string DeviceName { get; private set; }
        public static Uri MgmtInterface { get; private set; }
        public static string Password { get; private set; }
        public static string RestoreTempDir { get; private set; }
        public static ISpectraRioBrokerClient SpectraRioBrokerClient { get; private set; }
        public static string Username { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static void CreateBrokers()
        {
            CreateBroker_1();
            CreateBroker_2();
        }

        public static void CreateBroker_1()
        {
            BrokerName = ConfigurationManager.AppSettings["BrokerName"];

            if (!SpectraRioBrokerClient.DoesBrokerExist(BrokerName))
            {
                var agentConfig = GetAgentConfig();
                var createBrokerRequest = new CreateBrokerRequest(BrokerName, AgentName, agentConfig);
                SpectraRioBrokerClient.CreateBroker(createBrokerRequest);
            }
        }

        public static void CreateBroker_2()
        {
            BrokerName2 = ConfigurationManager.AppSettings["BrokerName2"];

            if (!SpectraRioBrokerClient.DoesBrokerExist(BrokerName2))
            {
                var createBrokerRequest = new CreateBrokerRequest(BrokerName2, AgentName, new AgentConfig(DeviceName, BlackPearlUserName, BlackPearlBucket2, false));
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
            MgmtInterface = ConfigurationManager.AppSettings["MgmtInterface"].ToUri();
            Username = ConfigurationManager.AppSettings["Username"];
            Password = ConfigurationManager.AppSettings["Password"];
            DataInterface = ConfigurationManager.AppSettings["DataInterface"].ToUri();

            if (!SpectraRioBrokerClient.DoesDeviceExist(DeviceName))
            {
                var createDeviceRequest = new CreateDeviceRequest(DeviceName, MgmtInterface, Username, Password);
                SpectraRioBrokerClient.CreateDevice(createDeviceRequest);
            }
        }

        public static AgentConfig GetAgentConfig()
        {
            return new AgentConfig(DeviceName, BlackPearlUserName, BlackPearlBucket, false);
        }

        public static void SetupTestData()
        {
            var tempDir = Path.GetTempPath();

            SetupArchiveTestData(tempDir);
            SetupRestoreTestData(tempDir);
        }

        #endregion Public Methods

        #region Private Methods

        private static void CreateClient()
        {
            var SpectraRioBrokerClientBuilder = new SpectraRioBrokerClientBuilder(
                ConfigurationManager.AppSettings["ServerName"],
                int.Parse(ConfigurationManager.AppSettings["ServerPort"]));

            var proxy = ConfigurationManager.AppSettings["Proxy"];
            if (!string.IsNullOrWhiteSpace(proxy))
            {
                SpectraRioBrokerClientBuilder.WithProxy(proxy);
            }

            SpectraRioBrokerClient = SpectraRioBrokerClientBuilder.DisableSslValidation().Build();
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

        private static void UpdateClientToken()
        {
            var token = SpectraRioBrokerClient.CreateToken(new CreateTokenRequest(ConfigurationManager.AppSettings["RioUsername"], ConfigurationManager.AppSettings["RioPassword"])).Token;
            SpectraRioBrokerClient.UpdateToken(token);
        }

        #endregion Private Methods
    }
}