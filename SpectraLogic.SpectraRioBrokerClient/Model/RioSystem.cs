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

using Newtonsoft.Json;
using SpectraLogic.SpectraRioBrokerClient.Utils;
using System;

namespace SpectraLogic.SpectraRioBrokerClient.Model
{
    /// <summary>
    ///
    /// </summary>
    public class Jvm
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Jvm"/> class.
        /// </summary>
        /// <param name="vendor">The vendor.</param>
        /// <param name="version">The version.</param>
        /// <param name="vmName">Name of the vm.</param>
        /// <param name="vmVersion">The vm version.</param>
        public Jvm(string vendor, string version, string vmName, string vmVersion)
        {
            Vendor = vendor;
            Version = version;
            VmName = vmName;
            VmVersion = vmVersion;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the vendor.
        /// </summary>
        /// <value>
        /// The vendor.
        /// </value>
        [JsonProperty(PropertyName = "vendor")] public string Vendor { get; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [JsonProperty(PropertyName = "version")] public string Version { get; }

        /// <summary>
        /// Gets the name of the vm.
        /// </summary>
        /// <value>
        /// The name of the vm.
        /// </value>
        [JsonProperty(PropertyName = "vmName")] public string VmName { get; }

        /// <summary>
        /// Gets the vm version.
        /// </summary>
        /// <value>
        /// The vm version.
        /// </value>
        [JsonProperty(PropertyName = "vmVersion")] public string VmVersion { get; }

        #endregion Public Properties
    }

    /// <summary>
    ///
    /// </summary>
    public class RioOperatingSystem
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RioOperatingSystem"/> class.
        /// </summary>
        /// <param name="arch">The arch.</param>
        /// <param name="cores">The cores.</param>
        /// <param name="name">The name.</param>
        /// <param name="version">The version.</param>
        public RioOperatingSystem(string arch, int cores, string name, string version)
        {
            Arch = arch;
            Cores = cores;
            Name = name;
            Version = version;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the arch.
        /// </summary>
        /// <value>
        /// The arch.
        /// </value>
        [JsonProperty(PropertyName = "arch")] public string Arch { get; }

        /// <summary>
        /// Gets the cores.
        /// </summary>
        /// <value>
        /// The cores.
        /// </value>
        [JsonProperty(PropertyName = "cores")] public int Cores { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty(PropertyName = "name")] public string Name { get; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [JsonProperty(PropertyName = "version")] public string Version { get; }

        #endregion Public Properties
    }

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="SpectraLogic.SpectraRioBrokerClient.Model.IRioSystem" />
    public class RioSystem : IRioSystem
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RioSystem"/> class.
        /// </summary>
        /// <param name="apiVersion">The API version.</param>
        /// <param name="buildDate">The build date.</param>
        /// <param name="gitCommitHash">The git commit hash.</param>
        /// <param name="runtimeStats">The runtime stats.</param>
        /// <param name="server">The server.</param>
        /// <param name="version">The version.</param>
        public RioSystem(string apiVersion, string buildDate, string gitCommitHash, RuntimeStats runtimeStats, Server server, string version)
        {
            ApiVersion = apiVersion;
            BuildDate = buildDate.ToDateTime();
            GitCommitHash = gitCommitHash;
            RuntimeStats = runtimeStats;
            Server = server;
            Version = version;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the API version.
        /// </summary>
        /// <value>
        /// The API version.
        /// </value>
        [JsonProperty(PropertyName = "apiVersion")] public string ApiVersion { get; }

        /// <summary>
        /// Gets the build date.
        /// </summary>
        /// <value>
        /// The build date.
        /// </value>
        [JsonProperty(PropertyName = "buildDate")] public DateTime BuildDate { get; }

        /// <summary>
        /// Gets the git commit hash.
        /// </summary>
        /// <value>
        /// The git commit hash.
        /// </value>
        [JsonProperty(PropertyName = "gitCommitHash")] public string GitCommitHash { get; }

        /// <summary>
        /// Gets the runtime stats.
        /// </summary>
        /// <value>
        /// The runtime stats.
        /// </value>
        [JsonProperty(PropertyName = "runtimeStats")] public RuntimeStats RuntimeStats { get; }

        /// <summary>
        /// Gets the server.
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        [JsonProperty(PropertyName = "server")] public Server Server { get; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [JsonProperty(PropertyName = "version")] public string Version { get; }

        #endregion Public Properties
    }

    /// <summary>
    ///
    /// </summary>
    public class RuntimeStats
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeStats"/> class.
        /// </summary>
        /// <param name="freeMemory">The free memory.</param>
        /// <param name="totalMemory">The total memory.</param>
        /// <param name="uptime">The uptime.</param>
        /// <param name="usedMemory">The used memory.</param>
        public RuntimeStats(long freeMemory, long totalMemory, long uptime, long usedMemory)
        {
            FreeMemory = freeMemory;
            TotalMemory = totalMemory;
            Uptime = uptime;
            UsedMemory = usedMemory;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the free memory.
        /// </summary>
        /// <value>
        /// The free memory.
        /// </value>
        [JsonProperty(PropertyName = "freeMemory")] public long FreeMemory { get; }

        /// <summary>
        /// Gets the total memory.
        /// </summary>
        /// <value>
        /// The total memory.
        /// </value>
        [JsonProperty(PropertyName = "totalMemory")] public long TotalMemory { get; }

        /// <summary>
        /// Gets the uptime.
        /// </summary>
        /// <value>
        /// The uptime.
        /// </value>
        [JsonProperty(PropertyName = "uptime")] public long Uptime { get; }

        /// <summary>
        /// Gets the used memory.
        /// </summary>
        /// <value>
        /// The used memory.
        /// </value>
        [JsonProperty(PropertyName = "usedMemory")] public long UsedMemory { get; }

        #endregion Public Properties
    }

    /// <summary>
    ///
    /// </summary>
    public class Server
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Server" /> class.
        /// </summary>
        /// <param name="jvm">The JVM.</param>
        /// <param name="rioOperatingSystem">The rio operating system.</param>
        public Server(Jvm jvm, RioOperatingSystem rioOperatingSystem)
        {
            Jvm = jvm;
            RioOperatingSystem = rioOperatingSystem;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the JVM.
        /// </summary>
        /// <value>
        /// The JVM.
        /// </value>
        [JsonProperty(PropertyName = "jvm")] public Jvm Jvm { get; }

        /// <summary>
        /// Gets the rio operating system.
        /// </summary>
        /// <value>
        /// The rio operating system.
        /// </value>
        [JsonProperty(PropertyName = "operatingSystem")] public RioOperatingSystem RioOperatingSystem { get; }

        #endregion Public Properties
    }
}