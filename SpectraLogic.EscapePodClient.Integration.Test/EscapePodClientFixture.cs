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
        public static IEscapePodClient EscapePodClient { get; private set; }
        public static string ArchiveName { get; private set; }
        public static string DeviceName { get; private set; }

        static EscapePodClientFixture()
        {
            BasicConfigurator.Configure();

            CreateClient();
            CreateDevice();
            CreateArchive();
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

        private static void CreateDevice()
        {
            DeviceName = ConfigurationManager.AppSettings["DeviceName"];
            var getDeviceRequest = new GetDeviceRequest(DeviceName);
            try
            {
                EscapePodClient.GetDevice(getDeviceRequest);
            }
            catch (DeviceNotFoundException)
            {
                var createDeviceRequest = new CreateDeviceRequest(
                    DeviceName,
                    ConfigurationManager.AppSettings["Endpoint"],
                    ConfigurationManager.AppSettings["Username"],
                    ConfigurationManager.AppSettings["Password"]);

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

        private static ResolverConfig GetResolver()
        {
            return new ResolverConfig("bp_resolver", DeviceName, "Administrator", "ep_net_sdk_testing", false);
        }
    }
}