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
using SpectraLogic.EscapePodClient.Calls;
using SpectraLogic.EscapePodClient.Exceptions;
using SpectraLogic.EscapePodClient.Model;
using SpectraLogic.EscapePodClient.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpectraLogic.EscapePodClient.Integration.Test
{
    [TestFixture]
    public class EscapePodClientErrorTests
    {
        #region Tests

        [Test]
        public void ArchiveErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new ArchiveRequest(null, Enumerable.Empty<ArchiveFile>())));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new ArchiveRequest(string.Empty, null)));

            //TODO add test for InvalidEscapePodServerCredentialsException

            var request = new ArchiveRequest("not_found", Enumerable.Empty<ArchiveFile>());
            Assert.ThrowsAsync<ArchiveNotFoundException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.Archive(request)));
        }

        [Test]
        public void CreateArchiveErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateArchiveRequest(null, EscapePodClientFixture.GetResolver())));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateArchiveRequest(EscapePodClientFixture.ArchiveName, null)));

            var request = new CreateArchiveRequest(EscapePodClientFixture.ArchiveName, EscapePodClientFixture.GetResolver());
            Assert.ThrowsAsync<ArchiveAlreadyExistsException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateArchive(request)));

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateArchiveRequest(string.Empty, EscapePodClientFixture.GetResolver());
                    EscapePodClientFixture.EscapePodClient.CreateArchive(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("name", "String", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateArchiveRequest("should_fail", new ResolverConfig(string.Empty, "bp_name", "username", "bucket", false));
                    EscapePodClientFixture.EscapePodClient.CreateArchive(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("resolverConfig.name", "String", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateArchiveRequest("should_fail", new ResolverConfig("name", "bp_name", "username", "bucket", false));
                    EscapePodClientFixture.EscapePodClient.CreateArchive(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("blackPearlName", "String", "not_found")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateArchiveRequest("should_fail", new ResolverConfig(EscapePodClientFixture.ResolverName, EscapePodClientFixture.DeviceName, "username", "bucket", false));
                    EscapePodClientFixture.EscapePodClient.CreateArchive(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("username", "String", "not_found")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateArchiveRequest("should_fail", new ResolverConfig(EscapePodClientFixture.ResolverName, EscapePodClientFixture.DeviceName, EscapePodClientFixture.BlackPearlUserName, "wrong_bucket", false));
                    EscapePodClientFixture.EscapePodClient.CreateArchive(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("bucket", "String", "not_found")
                });
        }

        [Test]
        public void CreateClusterErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateClusterRequest(null)));

            var request = new CreateClusterRequest(EscapePodClientFixture.ClusterName);
            Assert.ThrowsAsync<AlreadyAClusterMemberException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateCluster(request)));
        }

        [Test]
        public void CreateDeviceErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateDeviceRequest(null, "endpoint", "username", "password")));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateDeviceRequest("name", null, "username", "password")));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateDeviceRequest("name", "endpoint", null, "password")));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateDeviceRequest("name", "endpoint", "username", null)));

            //TODO add test for InvalidEscapePodServerCredentialsException

            var request = new CreateDeviceRequest(EscapePodClientFixture.DeviceName, "localhost", "username", "password");
            Assert.ThrowsAsync<DeviceAlreadyExistsException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateDevice(request)));

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest(string.Empty, "localhost", "username", "password");
                    EscapePodClientFixture.EscapePodClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("name", "String", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("name", string.Empty, "username", "password");
                    EscapePodClientFixture.EscapePodClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("endpoint", "String", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("name", "bad url", "username", "password");
                    EscapePodClientFixture.EscapePodClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("endpoint", "uri", "invalid_uri")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("name", "localhost", string.Empty, "password");
                    EscapePodClientFixture.EscapePodClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("username", "String", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("name", "localhost", "username", string.Empty);
                    EscapePodClientFixture.EscapePodClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("password", "Password", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest(string.Empty, string.Empty, string.Empty, string.Empty);
                    EscapePodClientFixture.EscapePodClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("endpoint", "String", "missing"),
                    new UnprocessableError("name", "String", "missing"),
                    new UnprocessableError("username", "String", "missing"),
                    new UnprocessableError("password", "Password", "missing")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("should_fail", EscapePodClientFixture.Endpoint, EscapePodClientFixture.Username, "wrong_password");
                    EscapePodClientFixture.EscapePodClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("username", "String", "invalid_credentials"),
                    new UnprocessableError("password", "Password", "invalid_credentials")
                });

            ValidationExceptionCheck(
                () =>
                {
                    request = new CreateDeviceRequest("should_fail", EscapePodClientFixture.Endpoint, "wrong_username", EscapePodClientFixture.Password);
                    EscapePodClientFixture.EscapePodClient.CreateDevice(request);
                    Assert.Fail();
                },
                new List<UnprocessableError>
                {
                    new UnprocessableError("username", "String", "invalid_credentials"),
                    new UnprocessableError("password", "Password", "invalid_credentials")
                });
        }

        [Test]
        public void GetArchiveErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new GetArchiveRequest(null)));

            //TODO add test for InvalidEscapePodServerCredentialsException

            var request = new GetArchiveRequest("not_found");
            Assert.ThrowsAsync<ArchiveNotFoundException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.GetArchive(request)));
        }

        [Test]
        public void GetDeviceErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new GetDeviceRequest(null)));

            //TODO add test for InvalidEscapePodServerCredentialsException

            var request = new GetDeviceRequest("not_found");
            Assert.ThrowsAsync<DeviceNotFoundException>(
                () =>
                Task.FromResult(EscapePodClientFixture.EscapePodClient.GetDevice(request)));
        }

        [Test]
        public void GetEscapePodJobErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new GetEscapePodJobRequest(null, Guid.NewGuid())));

            //TODO add test for InvalidEscapePodServerCredentialsException

            var request = new GetEscapePodJobRequest("not_found", Guid.NewGuid());
            Assert.ThrowsAsync<ArchiveNotFoundException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.GetJob(request)));

            request = new GetEscapePodJobRequest(EscapePodClientFixture.ArchiveName, Guid.NewGuid());
            Assert.ThrowsAsync<EscapePodJobNotFoundException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.GetJob(request)));
        }

        [Test]
        public void HeadArchiveErrorTests()
        {
            //TODO add test for InvalidEscapePodServerCredentialsException

            Assert.IsFalse(EscapePodClientFixture.EscapePodClient.IsArchiveExist("not_found"));
        }

        [Test]
        public void HeadDeviceErrorTests()
        {
            //TODO add test for InvalidEscapePodServerCredentialsException

            Assert.IsFalse(EscapePodClientFixture.EscapePodClient.IsDeviceExist("not_found"));
        }

        [Test]
        public void NodeIsNotAClusterMemeberErrorTests()
        {
            try
            {
                EscapePodClientFixture.EscapePodClient.DeleteCluster();

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new ArchiveRequest("", Enumerable.Empty<ArchiveFile>());
                        return Task.FromResult(EscapePodClientFixture.EscapePodClient.Archive(request));
                    });

                //TODO uncomment when cancel is supported in the server
                //Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                //    () =>
                //    {
                //        var request = new CancelRequest(Guid.NewGuid());
                //        return Task.FromResult(EscapePodClientFixture.EscapePodClient.Cancel(request));
                //    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new CreateArchiveRequest("", new ResolverConfig("", "", "", "", false));
                        return Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateArchive(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new CreateDeviceRequest("", "", "", "");
                        return Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateDevice(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                   () =>
                   {
                       EscapePodClientFixture.EscapePodClient.DeleteCluster();
                       return null;
                   });

                //TODO uncomment when delete is supported in the server
                //Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                //    () =>
                //    {
                //        var request = new DeleteRequest(Enumerable.Empty<DeleteFile>());
                //        return Task.FromResult(EscapePodClientFixture.EscapePodClient.Delete(request));
                //    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new GetArchiveRequest("");
                        return Task.FromResult(EscapePodClientFixture.EscapePodClient.GetArchive(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new GetClusterRequest();
                        return Task.FromResult(EscapePodClientFixture.EscapePodClient.GetCluster(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new GetDeviceRequest("");
                        return Task.FromResult(EscapePodClientFixture.EscapePodClient.GetDevice(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new GetEscapePodJobRequest("", Guid.NewGuid());
                        return Task.FromResult(EscapePodClientFixture.EscapePodClient.GetJob(request));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        return Task.FromResult(EscapePodClientFixture.EscapePodClient.IsArchiveExist(""));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        return Task.FromResult(EscapePodClientFixture.EscapePodClient.IsDeviceExist(""));
                    });

                Assert.ThrowsAsync<NodeIsNotAClusterMemeberException>(
                    () =>
                    {
                        var request = new RestoreRequest("", Enumerable.Empty<RestoreFile>());
                        return Task.FromResult(EscapePodClientFixture.EscapePodClient.Restore(request));
                    });
            }
            finally
            {
                EscapePodClientFixture.CreateCluster();
                EscapePodClientFixture.CreateDevice();
                EscapePodClientFixture.CreateArchive();
            }
        }

        [Test]
        public void RestoreErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                () => Task.FromResult(new RestoreRequest(null, Enumerable.Empty<RestoreFile>())));
            Assert.ThrowsAsync<ArgumentNullException>(
                () => Task.FromResult(new RestoreRequest(EscapePodClientFixture.ArchiveName, null)));

            //TODO add test for InvalidEscapePodServerCredentialsException

            var request = new RestoreRequest("should_fail", Enumerable.Empty<RestoreFile>());
            Assert.ThrowsAsync<ArchiveNotFoundException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.Restore(request)));

            //TODO test after ESCP-158 is done
            //EscapePodClientFixture.EscapePodClient.Restore(
            //    new RestoreRequest(EscapePodClientFixture.ArchiveName, new List<RestoreFile>
            //    {
            //        new RestoreFile("not_found", "bad uri")
            //    }));
        }

        #endregion Tests

        #region Methods

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

        #endregion Methods
    }
}