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
using System.Threading;

namespace SpectraLogic.EscapePodClient.Integration.Test
{
    public static class EscapePodClientFixture
    {
        #region Constructors

        static EscapePodClientFixture()
        {
            BasicConfigurator.Configure();

            CreateClient();
            CreateCluster();
            CreateDevice();
            CreateArchive();
        }

        #endregion Constructors

        #region Properties

        public static string BlackPearlBucket = "ep_net_sdk_tests";
        public static string BlackPearlUserName = "Administrator";
        public static string ResolverName = "bp_resolver";

        public static IEscapePodClient EscapePodClient { get; private set; }
        public static string ArchiveName { get; private set; }
        public static string ClusterName { get; private set; }
        public static string DeviceName { get; private set; }
        public static string Endpoint { get; private set; }
        public static string Password { get; private set; }
        public static string Username { get; private set; }

        #endregion Properties

        #region Methods

        public static ResolverConfig GetResolver()
        {
            return new ResolverConfig(ResolverName, DeviceName, BlackPearlUserName, BlackPearlBucket, false);
        }

        public static void CreateCluster()
        {
            ClusterName = ConfigurationManager.AppSettings["ClusterName"];
            var getClusterRequest = new GetClusterRequest();
            try
            {
                EscapePodClient.GetCluster(getClusterRequest);
            }
            catch (NodeIsNotAMemberOfAClusterException)
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

            if (!EscapePodClient.IsDeviceExist(DeviceName))
            {
                var createDeviceRequest = new CreateDeviceRequest(DeviceName, Endpoint, Username, Password);
                EscapePodClient.CreateDevice(createDeviceRequest);
            }
        }

        public static void CreateArchive()
        {
            ArchiveName = ConfigurationManager.AppSettings["ArchiveName"];

            if (!EscapePodClient.IsArchiveExist(ArchiveName))
            {
                var resolver = GetResolver();
                var createArchiveRequest = new CreateArchiveRequest(ArchiveName, resolver);
                EscapePodClient.CreateArchive(createArchiveRequest);
            }
        }

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

        #endregion Methods
    }
}