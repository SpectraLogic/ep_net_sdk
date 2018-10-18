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
using NUnit.Framework;
using SpectraLogic.SpectraRioBrokerClient.Calls;
using SpectraLogic.SpectraRioBrokerClient.Exceptions;
using SpectraLogic.SpectraRioBrokerClient.Model;
using SpectraLogic.SpectraRioBrokerClient.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SpectraLogic.SpectraRioBrokerClient.Integration.Test
{
    [TestFixture]
    public class SpectraRioBrokerClientErrorTests
    {
        #region Private Fields

        //TODO remove this once Job tracking is done in the server
        private readonly int MAX_POLLING_ATTEMPS = 10;

        private readonly int POLLING_INTERVAL = 10;
        private ILog _log = LogManager.GetLogger("SpectraRioBrokerClientErrorTests");

        #endregion Private Fields

        #region Public Methods

        [Test]
        public void ArchiveErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new ArchiveRequest(null, Enumerable.Empty<ArchiveFile>())));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new ArchiveRequest(string.Empty, null)));

            //TODO add test for InvalidServerCredentialsException

            var request = new ArchiveRequest("not_found", new List<ArchiveFile>
            {
                new ArchiveFile("not_found", "uri", 0L, new Dictionary<string, string>(), false, false)
            });
            Assert.ThrowsAsync<BrokerNotFoundException>(() => Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Archive(request)));

            ValidationExceptionCheck(
                () =>
                {
                    request = new ArchiveRequest("not_found", Enumerable.Empty<ArchiveFile>());
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Archive(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("files", "files", "no_files_in_job")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, new List<ArchiveFile>
                    {
                        new ArchiveFile("not_found", "bad uri", 0, new Dictionary<string, string>(), false, false)
                    });
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Archive(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("files.uri", "URI", "invalid_format", "bad uri")
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
                Assert.AreEqual("Job 00000000-0000-0000-0000-000000000000 is not currently running and cannot be canceled", e.ErrorResponse.ErrorMessage);
                Assert.AreEqual(HttpStatusCode.BadRequest, e.ErrorResponse.StatusCode);
            }
        }

        [Test]
        public void CreateBrokerErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateBrokerRequest(null, SpectraRioBrokerClientFixture.AgentName, SpectraRioBrokerClientFixture.GetAgentConfig())));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateBrokerRequest(SpectraRioBrokerClientFixture.BrokerName, null, SpectraRioBrokerClientFixture.GetAgentConfig())));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateBrokerRequest(SpectraRioBrokerClientFixture.BrokerName, SpectraRioBrokerClientFixture.AgentName, null)));

            var request = new CreateBrokerRequest(SpectraRioBrokerClientFixture.BrokerName, SpectraRioBrokerClientFixture.AgentName, SpectraRioBrokerClientFixture.GetAgentConfig());
            Assert.ThrowsAsync<BrokerAlreadyExistsException>(() => Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(request)));

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateBrokerRequest(string.Empty, SpectraRioBrokerClientFixture.AgentName, SpectraRioBrokerClientFixture.GetAgentConfig());
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("name", "string", "missing")
                });

            //TODO can be tested after ESCP-623 is fixed
            //ValidationExceptionCheck(
            //    () =>
            //    {
            //        request = new CreateBrokerRequest("should_fail", SpectraRioBrokerClientFixture.AgentName, new AgentConfig(string.Empty, SpectraRioBrokerClientFixture.DeviceName, SpectraRioBrokerClientFixture.Username, SpectraRioBrokerClientFixture.BlackPearlBucket, false));
            //        SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(request);
            //        Assert.Fail();
            //    },
            //    new List<UnprocessableError>
            //    {
            //        new UnprocessableError("agentConfig.name", "string", "missing")
            //    });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateBrokerRequest("should_fail", SpectraRioBrokerClientFixture.AgentName, new AgentConfig("name", "bp_name", "username", "bucket", false));
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("blackPearlName", "string", "spectra_device_registration_not_found", "bp_name")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateBrokerRequest("should_fail", SpectraRioBrokerClientFixture.AgentName, new AgentConfig(SpectraRioBrokerClientFixture.AgentName, SpectraRioBrokerClientFixture.DeviceName, "username", "bucket", false));
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("username", "string", "not_found", "username")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateBrokerRequest("should_fail", SpectraRioBrokerClientFixture.AgentName, new AgentConfig(SpectraRioBrokerClientFixture.AgentName, SpectraRioBrokerClientFixture.DeviceName, SpectraRioBrokerClientFixture.BlackPearlUserName, "wrong_bucket", false));
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("bucket", "string", "not_found", "wrong_bucket")
                });
        }

        [Test]
        public void CreateClusterErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateClusterRequest(null)));

            var request = new CreateClusterRequest(SpectraRioBrokerClientFixture.ClusterName);
            Assert.ThrowsAsync<AlreadyAClusterMemberException>(() => Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateCluster(request)));
        }

        [Test]
        public void CreateDeviceErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateDeviceRequest(null, "localhost", "username", "password")));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateDeviceRequest("name", null, "username", "password")));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateDeviceRequest("name", "localhost", null, "password")));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateDeviceRequest("name", "localhost", "username", null)));

            //TODO add test for InvalidServerCredentialsException

            var request = new CreateDeviceRequest(SpectraRioBrokerClientFixture.DeviceName, "localhost", "username", "password");
            Assert.ThrowsAsync<DeviceAlreadyExistsException>(() => Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request)));

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest(string.Empty, "localhost", "username", "password");
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("name", "string", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("name", string.Empty, "username", "password");
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("mgmtInterface", "string", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("name", "bad url", "username", "password");
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("mgmtInterface", "uri", "invalid_uri", "bad url")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("name", "localhost", string.Empty, "password");
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("username", "string", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("name", "localhost", "username", string.Empty);
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("password", "password", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest(string.Empty, string.Empty, string.Empty, string.Empty);
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("mgmtInterface", "string", "missing"),
                    new UnprocessableError("username", "string", "missing"),
                    new UnprocessableError("password", "password", "missing"),
                    new UnprocessableError("name", "string", "missing"),
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("should_fail", SpectraRioBrokerClientFixture.MgmtInterface, SpectraRioBrokerClientFixture.Username, "wrong_password");
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("username", "string", "invalid_credentials"),
                    new UnprocessableError("password", "password", "invalid_credentials")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("should_fail", SpectraRioBrokerClientFixture.MgmtInterface, "wrong_username", SpectraRioBrokerClientFixture.Password);
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("username", "string", "invalid_credentials"),
                    new UnprocessableError("password", "password", "invalid_credentials")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("should_fail", SpectraRioBrokerClientFixture.DataInterface, SpectraRioBrokerClientFixture.Username, SpectraRioBrokerClientFixture.Password);
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("mgmtInterface", "uri", "data_interface_specified")
                });
        }

        [Test]
        public void DeleteFileErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new DeleteFileRequest(null, "file")));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new DeleteFileRequest("broker", null)));

            var request = new DeleteFileRequest("not_found", "file");
            Assert.That(() => SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(request), Throws.Exception.TypeOf<BrokerNotFoundException>());

            request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, "not_found");
            Assert.That(() => SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(request), Throws.Exception.TypeOf<Exceptions.FileNotFoundException>());
        }

        [Test]
        public void GetBrokerErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new GetBrokerRequest(null)));

            //TODO add test for InvalidServerCredentialsException

            var request = new GetBrokerRequest("not_found");
            Assert.ThrowsAsync<BrokerNotFoundException>(() => Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBroker(request)));
        }

        [Test]
        public void GetBrokerRelationshipObjectsErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new GetBrokerRelationshipObjectsRequest(null, "relationship")));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new GetBrokerRelationshipObjectsRequest("broker", null)));

            var request = new GetBrokerRelationshipObjectsRequest("not_found", "relationship");
            Assert.That(() => SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerRelationshipObjects(request), Throws.Exception.TypeOf<BrokerNotFoundException>());

            request = new GetBrokerRelationshipObjectsRequest(SpectraRioBrokerClientFixture.BrokerName, "relationship_not_found");
            Assert.AreEqual(0, SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerRelationshipObjects(request).Objects.Count());
        }

        [Test]
        public void GetDeviceErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new GetDeviceRequest(null)));

            //TODO add test for InvalidServerCredentialsException

            var request = new GetDeviceRequest("not_found");
            Assert.ThrowsAsync<DeviceNotFoundException>(
                () =>
                Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetDevice(request)));
        }

        [Test]
        public void GetJobErrorTests()
        {
            //TODO add test for InvalidServerCredentialsException

            var request = new GetJobRequest(Guid.NewGuid());
            Assert.ThrowsAsync<JobNotFoundException>(() => Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(request)));
        }

        [Test]
        public void HeadBrokerErrorTests()
        {
            //TODO add test for InvalidServerCredentialsException

            Assert.IsFalse(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DoesBrokerExist("not_found"));
        }

        [Test]
        public void HeadDeviceErrorTests()
        {
            //TODO add test for InvalidServerCredentialsException

            Assert.IsFalse(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DoesDeviceExist("not_found"));
        }

        [Test]
        public void NodeIsNotAClusterMemeberErrorTests()
        {
            try
            {
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteCluster();

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new ArchiveRequest("", Enumerable.Empty<ArchiveFile>());
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Archive(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new CancelRequest(Guid.NewGuid());
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Cancel(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new CreateBrokerRequest("", "", new AgentConfig("", "", "", "", false));
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new CreateDeviceRequest("", "", "", "");
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateDevice(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                   () =>
                   {
                       SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteCluster();
                       return null;
                   });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new DeleteFileRequest("", "");
                        SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(request);
                        return null;
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new GetBrokerRequest("");
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBroker(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new GetClusterRequest();
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetCluster(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new GetDeviceRequest("");
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetDevice(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new GetJobRequest(Guid.NewGuid());
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DoesBrokerExist(""));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DoesDeviceExist(""));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new RestoreRequest("", Enumerable.Empty<RestoreFile>());
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(request));
                    });

                //TODO uncomment when retry is supported in the server
                //Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                //    () =>
                //    {
                //        var request = new RetryRequest(Guid.Empty);
                //        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Retry(request));
                //    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new GetBrokerRelationshipObjectsRequest("", "");
                        return Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerRelationshipObjects(request));
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

            //TODO add test for InvalidServerCredentialsException

            var request = new RestoreRequest("should_fail", new List<RestoreFile>
            {
                new RestoreFile("", "")
            });
            Assert.ThrowsAsync<BrokerNotFoundException>(() => Task.FromResult(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(request)));

            ValidationExceptionCheck(
                () =>
                {
                    request = new RestoreRequest("not_found", Enumerable.Empty<RestoreFile>());
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("files", "files", "no_files_in_job")
                 });

            ValidationExceptionCheck(
                () =>
                {
                    request = new RestoreRequest(SpectraRioBrokerClientFixture.BrokerName, new List<RestoreFile>
                    {
                        new RestoreFile("not_found", "bad uri")
                    });
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("files.uri", "URI", "invalid_format", "bad uri"),
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new RestoreRequest(SpectraRioBrokerClientFixture.BrokerName, new List<RestoreFile>
                    {
                        new RestoreFile("name", "uri", new ByteRange(-1, 10))
                    });
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("files.byteRange", "object", "invalid", reason:"The startingIndex must be positive"),
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new RestoreRequest(SpectraRioBrokerClientFixture.BrokerName, new List<RestoreFile>
                    {
                        new RestoreFile("name", "uri", new ByteRange(0, -10))
                    });
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("files.byteRange", "object", "invalid", reason:"The endingIndex must be positive"),
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new RestoreRequest(SpectraRioBrokerClientFixture.BrokerName, new List<RestoreFile>
                    {
                        new RestoreFile("name", "uri", new ByteRange(11, 10))
                    });
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("files.byteRange", "object", "invalid", reason:"startingIndex must be lower than endingIndex"),
                });
        }

        [Test]
        public void RestoreJobWithIgnoreDuplicates()
        {
            try
            {
                SpectraRioBrokerClientFixture.SetupTestData();

                var fileName1 = Guid.NewGuid().ToString();
                var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F1.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName1 } }, false, false),
                });

                var archiveJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Archive(archiveRequest);

                var pollingAttemps = 0;
                do
                {
                    archiveJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                        new GetJobRequest(archiveJob.JobId));
                    _log.Debug(archiveJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(POLLING_INTERVAL));
                    pollingAttemps++;
                } while (archiveJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttemps < MAX_POLLING_ATTEMPS);

                Assert.Less(pollingAttemps, MAX_POLLING_ATTEMPS);
                Assert.AreEqual(JobStatusEnum.COMPLETED, archiveJob.Status.Status);

                archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName2, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F1.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName1 } }, false, false),
                });

                archiveJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Archive(archiveRequest);

                pollingAttemps = 0;
                do
                {
                    archiveJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                        new GetJobRequest(archiveJob.JobId));
                    _log.Debug(archiveJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(POLLING_INTERVAL));
                    pollingAttemps++;
                } while (archiveJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttemps < MAX_POLLING_ATTEMPS);

                Assert.Less(pollingAttemps, MAX_POLLING_ATTEMPS);
                Assert.AreEqual(JobStatusEnum.COMPLETED, archiveJob.Status.Status);

                ValidationExceptionCheck(
                    () =>
                    {
                        //Using search and restore
                        var restoreRequest = new RestoreRequest("*", new List<RestoreFile>
                        {
                            new RestoreFile(fileName1, $"{SpectraRioBrokerClientFixture.RestoreTempDir}/F1_restore.txt".ToFileUri()),
                        });
                        SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(restoreRequest);
                        Assert.Fail();
                    },
                    new List<UnprocessableError>
                    {
                        new UnprocessableError(fileName1, "file", "duplicate_file_brokers"),
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

        [Test, Ignore("Retry is not yet implemented in the server")]
        public void RetryErrorTests()
        {
            try
            {
                var request = new RetryRequest(Guid.Empty);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Retry(request);
                Assert.Fail();
            }
            catch (ErrorResponseException e)
            {
                Assert.AreEqual("TBD", e.ErrorResponse.ErrorMessage);
                Assert.AreEqual(HttpStatusCode.BadRequest, e.ErrorResponse.StatusCode);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void ValidationExceptionCheck(Action action, IEnumerable expected)
        {
            try
            {
                action.Invoke();
            }
            catch (ValidationException ex)
            {
                var unprocessableErrorResponse = ex.ExtractUnprocessableErrorResponse();
                CollectionAssert.AreEqual(expected, unprocessableErrorResponse.Errors, new UnprocessableErrorComparer());
            }
        }

        #endregion Private Methods
    }
}