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

        public static string ResolverName = "bp_resolver";
        public static string BlackPearlUserName = "Administrator";
        public static string BlackPearlBucket = "ep_net_sdk_tests";

        public static IEscapePodClient EscapePodClient { get; private set; }
        public static string ArchiveName { get; private set; }
        public static string DeviceName { get; private set; }
        public static string Endpoint { get; private set; }
        public static string Username { get; private set; }
        public static string Password { get; private set; }

        #endregion Properties

        #region Methods

        public static ResolverConfig GetResolver()
        {
            return new ResolverConfig(ResolverName, DeviceName, BlackPearlUserName, BlackPearlBucket, false);
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

        private static void CreateCluster()
        {
            var clusterName = ConfigurationManager.AppSettings["ClusterName"];
            var getClusterRequest = new GetClusterRequest();
            try
            {
                EscapePodClient.GetCluster(getClusterRequest);
            }
            catch (ClusterNotConfiguredException)
            {
                var createClusterRequest = new CreateClusterRequest(clusterName);
                EscapePodClient.CreateCluster(createClusterRequest);
            }
        }

        private static void CreateDevice()
        {
            DeviceName = ConfigurationManager.AppSettings["DeviceName"];
            Endpoint = ConfigurationManager.AppSettings["Endpoint"];
            Username = ConfigurationManager.AppSettings["Username"];
            Password = ConfigurationManager.AppSettings["Password"];

            var getDeviceRequest = new GetDeviceRequest(DeviceName);
            try
            {
                EscapePodClient.GetDevice(getDeviceRequest);
            }
            catch (DeviceNotFoundException)
            {
                var createDeviceRequest = new CreateDeviceRequest(DeviceName, Endpoint, Username, Password);

                EscapePodClient.CreateDevice(createDeviceRequest);
            }
        }

        private static void CreateArchive()
        {
            ArchiveName = ConfigurationManager.AppSettings["ArchiveName"];
            var getArchiveRequest = new GetArchiveRequest(ArchiveName);
            try
            {
                EscapePodClient.GetArchive(getArchiveRequest);
            }
            catch (ArchiveNotFoundException)
            {
                var resolver = GetResolver();
                var createArchiveRequest = new CreateArchiveRequest(ArchiveName, resolver);
                EscapePodClient.CreateArchive(createArchiveRequest);
            }
        }

        #endregion Methods
    }
}