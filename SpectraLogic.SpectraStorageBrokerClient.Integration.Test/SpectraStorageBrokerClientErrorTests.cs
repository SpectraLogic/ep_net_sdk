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

using NUnit.Framework;
using SpectraLogic.SpectraStorageBrokerClient.Calls;
using SpectraLogic.SpectraStorageBrokerClient.Exceptions;
using SpectraLogic.SpectraStorageBrokerClient.Model;
using SpectraLogic.SpectraStorageBrokerClient.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpectraLogic.SpectraStorageBrokerClient.Integration.Test
{
    [TestFixture]
    public class SpectraStorageBrokerClientErrorTests
    {
        #region Tests

        [Test]
        public void ArchiveErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new ArchiveRequest(null, Enumerable.Empty<ArchiveFile>())));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new ArchiveRequest(string.Empty, null)));

            //TODO add test for InvalidServerCredentialsException

            var request = new ArchiveRequest("not_found", Enumerable.Empty<ArchiveFile>());
            Assert.ThrowsAsync<BrokerNotFoundException>(() => Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Archive(request)));

            ValidationExceptionCheck(
                () =>
                {
                    request = new ArchiveRequest(SpectraStorageBrokerClientFixture.BrokerName, new List<ArchiveFile>
                    {
                        new ArchiveFile("not_found", "bad uri", 0, new Dictionary<string, string>(), false, false)
                    });
                    SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Archive(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("files.uri", "URI", "invalid_format", "bad uri"),
                });
        }

        [Test]
        public void CreateBrokerErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateBrokerRequest(null, SpectraStorageBrokerClientFixture.GetAgentConfig())));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateBrokerRequest(SpectraStorageBrokerClientFixture.BrokerName, null)));

            var request = new CreateBrokerRequest(SpectraStorageBrokerClientFixture.BrokerName, SpectraStorageBrokerClientFixture.GetAgentConfig());
            Assert.ThrowsAsync<BrokerAlreadyExistsException>(() => Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateBroker(request)));

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateBrokerRequest(string.Empty, SpectraStorageBrokerClientFixture.GetAgentConfig());
                    SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateBroker(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("name", "string", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateBrokerRequest("should_fail", new AgentConfig(string.Empty, "bp_name", "username", "bucket", false));
                    SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateBroker(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("agentConfig.name", "string", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateBrokerRequest("should_fail", new AgentConfig("name", "bp_name", "username", "bucket", false));
                    SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateBroker(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("blackPearlName", "string", "not_found")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateBrokerRequest("should_fail", new AgentConfig(SpectraStorageBrokerClientFixture.AgentName, SpectraStorageBrokerClientFixture.DeviceName, "username", "bucket", false));
                    SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateBroker(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("username", "string", "not_found")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateBrokerRequest("should_fail", new AgentConfig(SpectraStorageBrokerClientFixture.AgentName, SpectraStorageBrokerClientFixture.DeviceName, SpectraStorageBrokerClientFixture.BlackPearlUserName, "wrong_bucket", false));
                    SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateBroker(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("bucket", "string", "not_found")
                });
        }

        [Test]
        public void CreateClusterErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateClusterRequest(null)));

            var request = new CreateClusterRequest(SpectraStorageBrokerClientFixture.ClusterName);
            Assert.ThrowsAsync<AlreadyAClusterMemberException>(() => Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateCluster(request)));
        }

        [Test]
        public void CreateDeviceErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateDeviceRequest(null, "localhost", "username", "password")));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateDeviceRequest("name", null, "username", "password")));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateDeviceRequest("name", "localhost", null, "password")));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateDeviceRequest("name", "localhost", "username", null)));

            //TODO add test for InvalidServerCredentialsException

            var request = new CreateDeviceRequest(SpectraStorageBrokerClientFixture.DeviceName, "localhost", "username", "password");
            Assert.ThrowsAsync<DeviceAlreadyExistsException>(() => Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateDevice(request)));

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest(string.Empty, "localhost", "username", "password");
                    SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateDevice(request);
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
                    SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateDevice(request);
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
                    SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateDevice(request);
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
                    SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateDevice(request);
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
                    SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateDevice(request);
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
                    SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("mgmtInterface", "string", "missing"),
                    new UnprocessableError("name", "string", "missing"),
                    new UnprocessableError("username", "string", "missing"),
                    new UnprocessableError("password", "password", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("should_fail", SpectraStorageBrokerClientFixture.MgmtInterface, SpectraStorageBrokerClientFixture.Username, "wrong_password");
                    SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateDevice(request);
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
                    request = new CreateDeviceRequest("should_fail", SpectraStorageBrokerClientFixture.MgmtInterface, "wrong_username", SpectraStorageBrokerClientFixture.Password);
                    SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateDevice(request);
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
                    request = new CreateDeviceRequest("should_fail", SpectraStorageBrokerClientFixture.DataInterface, SpectraStorageBrokerClientFixture.Username, SpectraStorageBrokerClientFixture.Password);
                    SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("mgmtInterface", "uri", "network_timeout")
                });
        }

        [Test]
        public void GetBrokerErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new GetBrokerRequest(null)));

            //TODO add test for InvalidServerCredentialsException

            var request = new GetBrokerRequest("not_found");
            Assert.ThrowsAsync<BrokerNotFoundException>(() => Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.GetBroker(request)));
        }

        [Test]
        public void GetDeviceErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new GetDeviceRequest(null)));

            //TODO add test for InvalidServerCredentialsException

            var request = new GetDeviceRequest("not_found");
            Assert.ThrowsAsync<DeviceNotFoundException>(
                () =>
                Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.GetDevice(request)));
        }

        [Test]
        public void GetJobErrorTests()
        {
            //TODO add test for InvalidServerCredentialsException

            var request = new GetJobRequest(Guid.NewGuid());
            Assert.ThrowsAsync<JobNotFoundException>(() => Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.GetJob(request)));
        }

        [Test]
        public void HeadBrokerErrorTests()
        {
            //TODO add test for InvalidServerCredentialsException

            Assert.IsFalse(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.DoesBrokerExist("not_found"));
        }

        [Test]
        public void HeadDeviceErrorTests()
        {
            //TODO add test for InvalidServerCredentialsException

            Assert.IsFalse(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.DoesDeviceExist("not_found"));
        }

        [Test]
        public void NodeIsNotAClusterMemeberErrorTests()
        {
            try
            {
                SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.DeleteCluster();

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new ArchiveRequest("", Enumerable.Empty<ArchiveFile>());
                        return Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Archive(request));
                    });

                //TODO uncomment when cancel is supported in the server
                //Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                //    () =>
                //    {
                //        var request = new CancelRequest(Guid.NewGuid());
                //        return Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Cancel(request));
                //    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new CreateBrokerRequest("", new AgentConfig("", "", "", "", false));
                        return Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateBroker(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new CreateDeviceRequest("", "", "", "");
                        return Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.CreateDevice(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                   () =>
                   {
                       SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.DeleteCluster();
                       return null;
                   });

                //TODO uncomment when delete is supported in the server
                //Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                //    () =>
                //    {
                //        var request = new DeleteRequest(Enumerable.Empty<DeleteFile>());
                //        return Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Delete(request));
                //    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new GetBrokerRequest("");
                        return Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.GetBroker(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new GetClusterRequest();
                        return Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.GetCluster(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new GetDeviceRequest("");
                        return Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.GetDevice(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new GetJobRequest(Guid.NewGuid());
                        return Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.GetJob(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        return Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.DoesBrokerExist(""));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        return Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.DoesDeviceExist(""));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new RestoreRequest("", Enumerable.Empty<RestoreFile>());
                        return Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Restore(request));
                    });
            }
            finally
            {
                SpectraStorageBrokerClientFixture.CreateCluster();
                SpectraStorageBrokerClientFixture.CreateDevice();
                SpectraStorageBrokerClientFixture.CreateBroker();
            }
        }

        [Test]
        public void RestoreErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                () => Task.FromResult(new RestoreRequest(null, Enumerable.Empty<RestoreFile>())));
            Assert.ThrowsAsync<ArgumentNullException>(
                () => Task.FromResult(new RestoreRequest(SpectraStorageBrokerClientFixture.BrokerName, null)));

            //TODO add test for InvalidServerCredentialsException

            var request = new RestoreRequest("should_fail", Enumerable.Empty<RestoreFile>());
            Assert.ThrowsAsync<BrokerNotFoundException>(() => Task.FromResult(SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Restore(request)));

            ValidationExceptionCheck(
                () =>
                {
                    request = new RestoreRequest(SpectraStorageBrokerClientFixture.BrokerName, new List<RestoreFile>
                    {
                        new RestoreFile("not_found", "bad uri")
                    });
                    SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Restore(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("files.uri", "URI", "invalid_format", "bad uri"),
                });
        }

        #endregion Tests

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