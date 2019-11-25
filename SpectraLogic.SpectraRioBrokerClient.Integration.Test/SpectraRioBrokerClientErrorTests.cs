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
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using NUnit.Framework;
using SpectraLogic.SpectraRioBrokerClient.Calls.Authentication;
using SpectraLogic.SpectraRioBrokerClient.Calls.Broker;
using SpectraLogic.SpectraRioBrokerClient.Calls.Cluster;
using SpectraLogic.SpectraRioBrokerClient.Calls.DevicesSpectra;
using SpectraLogic.SpectraRioBrokerClient.Calls.Jobs;
using SpectraLogic.SpectraRioBrokerClient.Calls.System;
using SpectraLogic.SpectraRioBrokerClient.Exceptions;
using SpectraLogic.SpectraRioBrokerClient.Model;
using SpectraLogic.SpectraRioBrokerClient.Utils;

namespace SpectraLogic.SpectraRioBrokerClient.Integration.Test
{
    [TestFixture]
    public class SpectraRioBrokerClientErrorTests
    {
        #region Fields

        private const int MaxPollingAttempts = 10;

        private const int PollingInterval = 10;
        private readonly ILog _log = LogManager.GetLogger("SpectraRioBrokerClientErrorTests");

        #endregion Fields

        #region Methods

        [Test]
        public void ArchiveErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(new ArchiveRequest(null, Enumerable.Empty<ArchiveFile>())));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new ArchiveRequest(string.Empty, null)));

            var request = new ArchiveRequest("not_found", new List<ArchiveFile>
            {
                new ArchiveFile("not_found", "uri".ToFileUri(), 0L, new Dictionary<string, string>(), false)
            });
            Assert.ThrowsAsync<BrokerNotFoundException>(() =>
                Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Archive(request)));

            ValidationExceptionCheck(
                () =>
                {
                    request = new ArchiveRequest("not_found", Enumerable.Empty<ArchiveFile>());
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Archive(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("files", "files", "no_files_in_job")
                });
        }

        [Test]
        public void CancelErrorTests()
        {
            try
            {
                var request = new CancelRequest(Guid.Empty);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Cancel(request);
                Assert.Fail();
            }
            catch (ErrorResponseException e)
            {
                Assert.AreEqual(
                    "Job 00000000-0000-0000-0000-000000000000 is not currently running and cannot be canceled",
                    e.ErrorResponse.ErrorMessage);
                Assert.AreEqual(HttpStatusCode.BadRequest, e.ErrorResponse.StatusCode);
            }
        }

        [Test]
        public void CreateBrokerErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateBrokerRequest(null,
                SpectraRioBrokerClientFixture.AgentName, SpectraRioBrokerClientFixture.GetAgentConfig())));
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(new CreateBrokerRequest(SpectraRioBrokerClientFixture.BrokerName, null,
                    SpectraRioBrokerClientFixture.GetAgentConfig())));
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(new CreateBrokerRequest(SpectraRioBrokerClientFixture.BrokerName,
                    SpectraRioBrokerClientFixture.AgentName, null)));

            var request = new CreateBrokerRequest(SpectraRioBrokerClientFixture.BrokerName,
                SpectraRioBrokerClientFixture.AgentName, SpectraRioBrokerClientFixture.GetAgentConfig());
            Assert.ThrowsAsync<BrokerAlreadyExistsException>(() =>
                Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(request)));

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateBrokerRequest(string.Empty, SpectraRioBrokerClientFixture.AgentName,
                        SpectraRioBrokerClientFixture.GetAgentConfig());
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("name", "string", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateBrokerRequest("should_fail", string.Empty,
                        new AgentConfig(SpectraRioBrokerClientFixture.DeviceName,
                            SpectraRioBrokerClientFixture.Username, SpectraRioBrokerClientFixture.BlackPearlBucket));
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("agentName", "string", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateBrokerRequest("should_fail", SpectraRioBrokerClientFixture.AgentName,
                        new AgentConfig("bp_name", "username", "bucket"));
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("agentConfig.blackPearlName", "string", "spectra_device_registration_not_found",
                        "bp_name")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateBrokerRequest("should_fail", SpectraRioBrokerClientFixture.AgentName,
                        new AgentConfig(SpectraRioBrokerClientFixture.DeviceName, "username", "bucket"));
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("agentConfig.username", "string", "not_found", "username")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateBrokerRequest("should_fail", SpectraRioBrokerClientFixture.AgentName,
                        new AgentConfig(SpectraRioBrokerClientFixture.DeviceName,
                            SpectraRioBrokerClientFixture.BlackPearlUserName, "wrong_bucket"));
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("agentConfig.bucket", "string", "not_found", "wrong_bucket")
                });
            
            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateBrokerRequest("should_fail", SpectraRioBrokerClientFixture.AgentName,
                        new AgentConfig(SpectraRioBrokerClientFixture.DeviceName,
                            SpectraRioBrokerClientFixture.BlackPearlUserName, "not_found",
                            createBucket: true));
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("agentConfig.dataPolicyUUID", "uuid", "missing")
                });
            
            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateBrokerRequest("should_fail", SpectraRioBrokerClientFixture.AgentName,
                        new AgentConfig(SpectraRioBrokerClientFixture.DeviceName,
                            SpectraRioBrokerClientFixture.BlackPearlUserName, "not_found",
                            createBucket: true, dataPolicyUuid: "d68415f8-5bb3-44a6-acc8-6fd7a3fcf2e4"));
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("agentConfig.dataPolicyUUID", "uuid", "not_found", "d68415f8-5bb3-44a6-acc8-6fd7a3fcf2e4")
                });
            
            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateBrokerRequest("should_fail", SpectraRioBrokerClientFixture.AgentName,
                        new AgentConfig(SpectraRioBrokerClientFixture.DeviceName,
                            SpectraRioBrokerClientFixture.BlackPearlUserName, "not_found",
                            createBucket: true, dataPolicyUuid: "i am the data policy"));
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("agentConfig.dataPolicyUUID", "uuid", "invalid_uuid_value", "i am the data policy")
                });
        }

        [Test]
        public void CreateClusterErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateClusterRequest(null)));

            var request = new CreateClusterRequest(SpectraRioBrokerClientFixture.ClusterName);
            Assert.ThrowsAsync<AlreadyAClusterMemberException>(() =>
                Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateCluster(request)));
        }

        [Test]
        public void CreateDeviceErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(new CreateDeviceRequest(null, "localhost".ToHttpsUri(), "username", "password")));
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(new CreateDeviceRequest("name", null, "username", "password")));
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(new CreateDeviceRequest("name", "localhost".ToHttpsUri(), null, "password")));
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(new CreateDeviceRequest("name", "localhost".ToHttpsUri(), "username", null)));

            var request = new CreateDeviceRequest(SpectraRioBrokerClientFixture.DeviceName, "localhost".ToHttpsUri(), "username",
                "password");
            Assert.ThrowsAsync<DeviceAlreadyExistsException>(() =>
                Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request)));

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest(string.Empty, "localhost".ToHttpsUri(), "username", "password");
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("name", "string", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("name", "localhost".ToHttpsUri(), string.Empty, "password");
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("username", "string", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("name", "localhost".ToHttpsUri(), "username", string.Empty);
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("password", "password", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest(string.Empty, "localhost".ToHttpsUri(), string.Empty, string.Empty);
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("username", "string", "missing"),
                    new ValidationError("password", "password", "missing"),
                    new ValidationError("name", "string", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("should_fail", SpectraRioBrokerClientFixture.MgmtInterface,
                        SpectraRioBrokerClientFixture.Username, "wrong_password");
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("username", "string", "invalid_credentials"),
                    new ValidationError("password", "password", "invalid_credentials")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("should_fail", SpectraRioBrokerClientFixture.MgmtInterface,
                        "wrong_username", SpectraRioBrokerClientFixture.Password);
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("username", "string", "invalid_credentials"),
                    new ValidationError("password", "password", "invalid_credentials")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("should_fail", SpectraRioBrokerClientFixture.DataInterface,
                        SpectraRioBrokerClientFixture.Username, SpectraRioBrokerClientFixture.Password);
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("mgmtInterface", "URI", "data_interface_specified")
                });
            
            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("should_fail", "http://localhost".ToUri(),
                        SpectraRioBrokerClientFixture.Username, SpectraRioBrokerClientFixture.Password);
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("mgmtInterface", "URI", "invalid_scheme", "http", "URI scheme must be HTTPS")
                });
        }

        [Test]
        public void CreateTokenErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateTokenRequest(null, "password")));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateTokenRequest("username", null)));

            var request = new CreateTokenRequest("wrong_username", "spectra");
            Assert.ThrowsAsync<AuthenticationFailureException>(() =>
                Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateToken(request)));

            request = new CreateTokenRequest("spectra", "wrong_password");
            Assert.ThrowsAsync<AuthenticationFailureException>(() =>
                Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateToken(request)));

            request = new CreateTokenRequest("wrong_username", "wrong_password");
            Assert.ThrowsAsync<AuthenticationFailureException>(() =>
                Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateToken(request)));

            var spectraRioBrokerClientBuilder = new SpectraRioBrokerClientBuilder(
                ConfigurationManager.AppSettings["ServerName"],
                int.Parse(ConfigurationManager.AppSettings["ServerPort"]));
            var noAuthClient = spectraRioBrokerClientBuilder.Build();

            Assert.ThrowsAsync<MissingAuthorizationHeaderException>(() =>
                Task.FromResult(noAuthClient.Archive(new ArchiveRequest("", Enumerable.Empty<ArchiveFile>()))));
            Assert.ThrowsAsync<MissingAuthorizationHeaderException>(() =>
                Task.FromResult(noAuthClient.Cancel(new CancelRequest(Guid.Empty))));
            Assert.ThrowsAsync<MissingAuthorizationHeaderException>(() =>
                Task.FromResult(
                    noAuthClient.CreateBroker(new CreateBrokerRequest("", "", new AgentConfig("", "", "")))));
            Assert.ThrowsAsync<MissingAuthorizationHeaderException>(() =>
                Task.FromResult(noAuthClient.CreateDevice(new CreateDeviceRequest("", "localhost".ToHttpsUri(), "", ""))));
            Assert.ThrowsAsync<MissingAuthorizationHeaderException>(() =>
                Task.FromResult(noAuthClient.GetBrokerRelationship(new GetBrokerRelationshipRequest("", ""))));
            Assert.ThrowsAsync<MissingAuthorizationHeaderException>(() =>
                Task.FromResult(noAuthClient.GetBrokerRelationships(new GetBrokerRelationshipsRequest(""))));
            Assert.ThrowsAsync<MissingAuthorizationHeaderException>(() =>
                Task.FromResult(noAuthClient.GetBrokers(new GetBrokersRequest())));
            Assert.ThrowsAsync<MissingAuthorizationHeaderException>(() =>
                Task.FromResult(noAuthClient.GetBroker(new GetBrokerRequest(""))));
            Assert.ThrowsAsync<MissingAuthorizationHeaderException>(() =>
                Task.FromResult(noAuthClient.GetDevice(new GetDeviceRequest(""))));
            Assert.ThrowsAsync<MissingAuthorizationHeaderException>(() =>
                Task.FromResult(noAuthClient.GetDevices(new GetDevicesRequest())));
            Assert.ThrowsAsync<MissingAuthorizationHeaderException>(() =>
                Task.FromResult(noAuthClient.GetJob(new GetJobRequest(Guid.Empty))));
            Assert.ThrowsAsync<MissingAuthorizationHeaderException>(() =>
                Task.FromResult(noAuthClient.GetJobs(new GetJobsRequest())));
            Assert.ThrowsAsync<MissingAuthorizationHeaderException>(() =>
                Task.FromResult(noAuthClient.Restore(new RestoreRequest("", Enumerable.Empty<RestoreFile>()))));
            Assert.ThrowsAsync<MissingAuthorizationHeaderException>(() =>
                Task.FromResult(noAuthClient.Retry(new RetryRequest("", Guid.Empty, JobType.ARCHIVE))));
            Assert.ThrowsAsync<MissingAuthorizationHeaderException>(() =>
            {
                noAuthClient.DeleteBroker(new DeleteBrokerRequest(""));
                return null;
            });
            Assert.ThrowsAsync<MissingAuthorizationHeaderException>(() =>
                Task.FromResult(noAuthClient.UpdateBrokerObject(new UpdateBrokerObjectRequest("", "",
                    new Dictionary<string, string>(), new HashSet<string>()))));

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateTokenRequest(string.Empty, "password");
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateToken(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("username", "string", "missing", reason: "username cannot be empty")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateTokenRequest("username", string.Empty);
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateToken(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("password", "string", "missing", reason: "password cannot be empty")
                });
        }

        [Test]
        public void DeleteBrokerErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new DeleteBrokerRequest(null)));

            var request = new DeleteBrokerRequest("not_found");
            Assert.That(() => SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteBroker(request),
                Throws.Exception.TypeOf<BrokerNotFoundException>());
        }

        [Test]
        public void DeleteFileErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new DeleteFileRequest(null, "file")));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new DeleteFileRequest("broker", null)));

            var request = new DeleteFileRequest("not_found", "file");
            Assert.That(() => SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(request),
                Throws.Exception.TypeOf<BrokerNotFoundException>());

            request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, "not_found");
            Assert.That(() => SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(request),
                Throws.Exception.TypeOf<BrokerObjectNotFoundException>());
        }

        [Test]
        public void GetBrokerErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new GetBrokerRequest(null)));

            var request = new GetBrokerRequest("not_found");
            Assert.ThrowsAsync<BrokerNotFoundException>(() =>
                Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBroker(request)));
        }

        [Test]
        public void GetBrokerObjectErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(new GetBrokerObjectRequest(null, "objectName")));
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(new GetBrokerRelationshipRequest("broker", null)));

            var request = new GetBrokerObjectRequest("not_found", "objectName");
            Assert.That(() => SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObject(request),
                Throws.Exception.TypeOf<BrokerNotFoundException>());

            request = new GetBrokerObjectRequest(SpectraRioBrokerClientFixture.BrokerName, "objectName_not_found");
            Assert.That(() => SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObject(request),
                Throws.Exception.TypeOf<BrokerObjectNotFoundException>());
        }

        [Test]
        [Ignore("https://jira.spectralogic.com/browse/ESCP-1652")]
        public void GetBrokerObjectsErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new GetBrokerObjectsRequest(null)));

            var request = new GetBrokerObjectsRequest("not_found");
            Assert.That(() => SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(request),
                Throws.Exception.TypeOf<BrokerNotFoundException>());
        }

        [Test]
        [Ignore("https://jira.spectralogic.com/browse/ESCP-1653")]
        public void GetBrokerRelationshipErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(new GetBrokerRelationshipRequest(null, "relationship")));
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(new GetBrokerRelationshipRequest("broker", null)));

            var request = new GetBrokerRelationshipRequest("not_found", "relationship");
            Assert.That(() => SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerRelationship(request),
                Throws.Exception.TypeOf<BrokerNotFoundException>());

            request = new GetBrokerRelationshipRequest(SpectraRioBrokerClientFixture.BrokerName,
                "relationship_not_found");
            Assert.AreEqual(0,
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerRelationship(request).Objects.Count);
        }

        [Test]
        public void GetBrokerRelationshipsErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new GetBrokerRelationshipsRequest(null)));

            var request = new GetBrokerRelationshipsRequest("not_found");
            Assert.That(() => SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerRelationships(request),
                Throws.Exception.TypeOf<BrokerNotFoundException>());
        }

        [Test]
        public void GetDeviceErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new GetDeviceRequest(null)));

            var request = new GetDeviceRequest("not_found");
            Assert.ThrowsAsync<DeviceNotFoundException>(
                () =>
                    Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetDevice(request)));
        }

        [Test]
        public void GetJobErrorTests()
        {
            var request = new GetJobRequest(Guid.NewGuid());
            Assert.ThrowsAsync<JobNotFoundException>(() =>
                Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(request)));
        }

        [Test]
        public void HeadBrokerErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DoesBrokerExist(null)));

            Assert.IsFalse(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DoesBrokerExist("not_found"));
        }

        [Test]
        public void HeadBrokerObjectErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DoesBrokerObjectExist(null, "objectName")));
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DoesBrokerObjectExist("brokerName", null)));

            Assert.IsFalse(
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DoesBrokerObjectExist("broker", "not_found"));
        }

        [Test]
        public void HeadDeviceErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DoesDeviceExist(null)));

            Assert.IsFalse(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DoesDeviceExist("not_found"));
        }

        [Test]
        public void HeadJobErrorTests()
        {
            Assert.IsFalse(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DoesJobExist(new Guid()));
        }

        [Test]
        public void HttpErrorTests()
        {
            var spectraRioBrokerClientBuilder = new SpectraRioBrokerClientBuilder(
                ConfigurationManager.AppSettings["ServerName"].Replace("https", "http"),
                int.Parse(ConfigurationManager.AppSettings["ServerPort"]));

            var proxy = ConfigurationManager.AppSettings["Proxy"];
            if (!string.IsNullOrWhiteSpace(proxy))
            {
                spectraRioBrokerClientBuilder.WithProxy(proxy);
            }

            var spectraRioBrokerClient = spectraRioBrokerClientBuilder.Build();

            Assert.ThrowsAsync<WebException>(() =>
                Task.FromResult(spectraRioBrokerClient.GetSystem(new GetSystemRequest())));
        }

        [Test]
        public void NodeIsNotAClusterMemberErrorTests()
        {
            try
            {
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteCluster();

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new ArchiveRequest("", Enumerable.Empty<ArchiveFile>());
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Archive(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new CancelRequest(Guid.NewGuid());
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Cancel(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new CreateBrokerRequest("", "", new AgentConfig("", "", ""));
                        return Task.FromResult(
                            SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new CreateDeviceRequest("", "localhost".ToHttpsUri(), "", "");
                        return Task.FromResult(
                            SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteCluster();
                        return null;
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new DeleteFileRequest("", "");
                        SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(request);
                        return null;
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new GetBrokersRequest();
                        return Task.FromResult(
                            SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokers(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new GetBrokerRequest("");
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBroker(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new GetClusterRequest();
                        return Task.FromResult(
                            SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetCluster(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () => Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetMaster()));

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () => Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetMembers()));

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new GetDeviceRequest("");
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetDevice(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new GetDevicesRequest();
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetDevices(request));
                    });
                
                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new GetJobRequest(Guid.NewGuid());
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new GetJobsRequest();
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJobs(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () => Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient
                        .DoesBrokerExist("")));

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () => Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient
                        .DoesDeviceExist("")));

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new RestoreRequest("", Enumerable.Empty<RestoreFile>());
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new RetryRequest("", Guid.Empty, JobType.ARCHIVE);
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Retry(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new GetBrokerRelationshipRequest("", "");
                        return Task.FromResult(
                            SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerRelationship(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new GetBrokerRelationshipsRequest("");
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient
                            .GetBrokerRelationships(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new DeleteBrokerRequest("");
                        SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteBroker(request);
                        return null;
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemberException>(
                    () =>
                    {
                        var request = new UpdateBrokerObjectRequest("", "", new Dictionary<string, string>(),
                            new HashSet<string>());
                        SpectraRioBrokerClientFixture.SpectraRioBrokerClient.UpdateBrokerObject(request);
                        return null;
                    });
            }
            finally
            {
                SpectraRioBrokerClientFixture.CreateCluster();
                SpectraRioBrokerClientFixture.CreateDevice();
                SpectraRioBrokerClientFixture.CreateBrokers();
            }
        }

        [Test]
        public void RestoreErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                () => Task.FromResult(new RestoreRequest(null, Enumerable.Empty<RestoreFile>())));
            Assert.ThrowsAsync<ArgumentNullException>(
                () => Task.FromResult(new RestoreRequest(SpectraRioBrokerClientFixture.BrokerName, null)));

            var request = new RestoreRequest("should_fail", new List<RestoreFile>
            {
                new RestoreFile("should_fail", "uri".ToFileUri())
            });
            Assert.ThrowsAsync<BrokerNotFoundException>(() =>
                Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(request)));

            ValidationExceptionCheck(
                () =>
                {
                    request = new RestoreRequest("not_found", Enumerable.Empty<RestoreFile>());
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("files", "files", "no_files_in_job")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new RestoreRequest(SpectraRioBrokerClientFixture.BrokerName, new List<RestoreFile>
                    {
                        new RestoreFile("name", "uri".ToFileUri(), new ByteRange(-1, 10))
                    });
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("files.byteRange", "object", "invalid",
                        reason: "The startingIndex must be positive")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new RestoreRequest(SpectraRioBrokerClientFixture.BrokerName, new List<RestoreFile>
                    {
                        new RestoreFile("name", "uri".ToFileUri(), new ByteRange(0, -10))
                    });
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("files.byteRange", "object", "invalid",
                        reason: "The endingIndex must be positive")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new RestoreRequest(SpectraRioBrokerClientFixture.BrokerName, new List<RestoreFile>
                    {
                        new RestoreFile("name", "uri".ToFileUri(), new ByteRange(11, 10))
                    });
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(request);
                    Assert.Fail();
                },
                new List<ValidationError>
                {
                    new ValidationError("files.byteRange", "object", "invalid",
                        reason: "startingIndex must be lower than endingIndex")
                });
        }

        [Test]
        public void RestoreJobWithIgnoreDuplicatesErrorTests()
        {
            try
            {
                SpectraRioBrokerClientFixture.SetupTestData();

                var fileName1 = Guid.NewGuid().ToString();
                var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, "F1.txt".ToAtoZUri(), 14,
                        new Dictionary<string, string> {{"fileName", fileName1}}, false)
                });

                var archiveJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Archive(archiveRequest);

                var pollingAttempts = 0;
                do
                {
                    archiveJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                        new GetJobRequest(archiveJob.JobId));
                    _log.Debug(archiveJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(PollingInterval));
                    pollingAttempts++;
                } while (archiveJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttempts < MaxPollingAttempts);

                Assert.Less(pollingAttempts, MaxPollingAttempts);
                Assert.AreEqual(JobStatusEnum.COMPLETED, archiveJob.Status.Status);

                archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName2, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, "F1.txt".ToAtoZUri(), 14,
                        new Dictionary<string, string> {{"fileName", fileName1}}, false)
                });

                archiveJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Archive(archiveRequest);

                pollingAttempts = 0;
                do
                {
                    archiveJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                        new GetJobRequest(archiveJob.JobId));
                    _log.Debug(archiveJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(PollingInterval));
                    pollingAttempts++;
                } while (archiveJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttempts < MaxPollingAttempts);

                Assert.Less(pollingAttempts, MaxPollingAttempts);
                Assert.AreEqual(JobStatusEnum.COMPLETED, archiveJob.Status.Status);

                ValidationExceptionCheck(
                    () =>
                    {
                        //Using search and restore
                        var restoreRequest = new RestoreRequest("*", new List<RestoreFile>
                        {
                            new RestoreFile(fileName1,
                                "F1_restore.txt".ToDevNullUri())
                        });
                        SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(restoreRequest);
                        Assert.Fail();
                    },
                    new List<ValidationError>
                    {
                        new ValidationError(fileName1, "file", "duplicate_file_brokers")
                    });

                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
                deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName2, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
            }
            finally
            {
                Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
                Directory.Delete(SpectraRioBrokerClientFixture.RestoreTempDir, true);
            }
        }

        [Test]
        public void RetryErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                () => Task.FromResult(new RetryRequest(null, Guid.Empty, JobType.ARCHIVE)));

            Assert.ThrowsAsync<InvalidOperationException>(
                () => Task.FromResult(new RetryRequest("", Guid.Empty, JobType.DELETE)));

            Assert.ThrowsAsync<InvalidOperationException>(
                () => Task.FromResult(new RetryRequest("", Guid.Empty, JobType.CANCEL)));

            var request = new RetryRequest("should_fail", Guid.Empty, JobType.ARCHIVE);
            Assert.ThrowsAsync<JobNotFoundException>(() =>
                Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Retry(request)));

            request = new RetryRequest("should_fail", Guid.Empty, JobType.RESTORE);
            Assert.ThrowsAsync<JobNotFoundException>(() =>
                Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Retry(request)));
        }

        [Test]
        public void UpdateBrokerObjectErrorTests()
        {
            var metadata = new Dictionary<string, string>();
            var relationships = new HashSet<string>();
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(new UpdateBrokerObjectRequest(null, "objectName", metadata, relationships)));
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(new UpdateBrokerObjectRequest("broker", null, metadata, relationships)));
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(new UpdateBrokerObjectRequest("broker", "objectName", null, relationships)));
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(new UpdateBrokerObjectRequest("broker", "objectName", metadata, null)));

            var request = new UpdateBrokerObjectRequest("not_found", "objectName", metadata, relationships);
            Assert.That(() => SpectraRioBrokerClientFixture.SpectraRioBrokerClient.UpdateBrokerObject(request),
                Throws.Exception.TypeOf<BrokerNotFoundException>());

            request = new UpdateBrokerObjectRequest(SpectraRioBrokerClientFixture.BrokerName, "objectName_not_found",
                metadata, relationships);
            Assert.That(() => SpectraRioBrokerClientFixture.SpectraRioBrokerClient.UpdateBrokerObject(request),
                Throws.Exception.TypeOf<BrokerObjectNotFoundException>());
        }

        private static void ValidationExceptionCheck(Action action, IEnumerable expected)
        {
            try
            {
                action.Invoke();
            }
            catch (ValidationException ex)
            {
                CollectionAssert.AreEqual(expected, ex.ValidationErrors, new ValidationErrorComparer());
            }
        }

        #endregion Methods
    }
}