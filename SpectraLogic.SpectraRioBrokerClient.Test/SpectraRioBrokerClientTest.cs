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
using log4net.Config;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SpectraLogic.SpectraRioBrokerClient.Calls;
using SpectraLogic.SpectraRioBrokerClient.Model;
using SpectraLogic.SpectraRioBrokerClient.Runtime;
using SpectraLogic.SpectraRioBrokerClient.Test.Mock;
using SpectraLogic.SpectraRioBrokerClient.Test.Utils;
using System;
using System.Net;

namespace SpectraLogic.SpectraRioBrokerClient.Test
{
    [TestFixture]
    internal class SpectraRioBrokerClientTest
    {
        #region Private Fields

        private static readonly ILog Log = LogManager.GetLogger("SpectraRioBrokerClientTest");

        #endregion Private Fields

        #region Public Constructors

        public SpectraRioBrokerClientTest()
        {
            BasicConfigurator.Configure();
        }

        #endregion Public Constructors

        #region Public Methods

        [Test]
        public void ArchiveTest()
        {
            var archiveRequest = new ArchiveRequest(Stubs.BrokerName, Stubs.ArchiveFiles);

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(archiveRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.ArchiveResponse",
                    HttpStatusCode.Created, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var job = client.Archive(archiveRequest);
            Assert.AreEqual(new Guid("101bddb7-8b34-4b35-9ef5-3c829d561e19"), job.JobId);
            Assert.AreEqual(JobType.ARCHIVE, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual("2018-01-23T03:52:46.869Z[UTC]", job.Created);
            Assert.AreEqual(0.0, job.Progress);
            Assert.AreEqual("Initializing", job.Status.Message);
            Assert.AreEqual(JobStatusEnum.ACTIVE, job.Status.Status);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void CancelTest()
        {
            var jobId = Guid.NewGuid();
            var cancelRequest = new CancelRequest(jobId);

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(cancelRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.CancelResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var job = client.Cancel(cancelRequest);
            Assert.AreEqual(new Guid("101bddb7-8b34-4b35-9ef5-3c829d561e19"), job.JobId);
            Assert.AreEqual(JobType.CANCEL, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual("2018-01-23T03:52:46.869Z[UTC]", job.Created);
            Assert.AreEqual(1.0, job.Progress);
            Assert.AreEqual("Cancelled", job.Status.Message);
            Assert.AreEqual(JobStatusEnum.CANCELED, job.Status.Status);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void CreateBrokerTest()
        {
            var createBrokerRequest = new CreateBrokerRequest(Stubs.BrokerName, Stubs.AgentConfig);

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(createBrokerRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.CreateBrokerResponse",
                    HttpStatusCode.Created, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var broker = client.CreateBroker(createBrokerRequest);
            Assert.AreEqual("brokerName", broker.BrokerName);
            Assert.AreEqual("2018-01-30T23:00:29.88Z[UTC]", broker.CreationDate);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void CreateClusterTest()
        {
            var createClusterRequest = new CreateClusterRequest("ep_net_sdk_tests");

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(createClusterRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.CreateClusterResponse",
                    HttpStatusCode.Created, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var cluster = client.CreateCluster(createClusterRequest);
            Assert.AreEqual("ep_net_sdk_tests", cluster.Name);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void CreateDeviceTest()
        {
            var createDeviceRequest = new CreateDeviceRequest("device_test", "localhost", "username", "password");

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(createDeviceRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.CreateDeviceResponse",
                    HttpStatusCode.Created, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var device = client.CreateDevice(createDeviceRequest);
            Assert.AreEqual("device_test", device.DeviceName);
            Assert.AreEqual("localhost", device.MgmtInterface);
            Assert.AreEqual("username", device.Username);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void DeleteCluster()
        {
            var deleteClusterRequest = new DeleteClusterRequest();

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(It.IsAny<DeleteClusterRequest>()))
                .Returns(new MockHttpWebResponse(null, HttpStatusCode.NoContent, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            client.DeleteCluster();

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void DeleteTest()
        {
            var deleteRequest = new DeleteFileRequest("broker", "test.file");

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(deleteRequest))
                .Returns(new MockHttpWebResponse(null, HttpStatusCode.NoContent, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            client.DeleteFile(deleteRequest);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void GetBrokerTest()
        {
            var getBrokerRequest = new GetBrokerRequest(Stubs.BrokerName);

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getBrokerRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetBrokerResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var broker = client.GetBroker(getBrokerRequest);
            Assert.AreEqual("brokerName", broker.BrokerName);
            Assert.AreEqual("2018-01-24T19:10:22.819Z[UTC]", broker.CreationDate);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void GetClusterTest()
        {
            var getClusterRequest = new GetClusterRequest();

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getClusterRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetClusterResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var cluster = client.GetCluster(getClusterRequest);
            Assert.AreEqual("ep_net_sdk_tests", cluster.Name);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void GetDeviceTest()
        {
            var getDeviceRequest = new GetDeviceRequest("device_test");

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getDeviceRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetDeviceResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var device = client.GetDevice(getDeviceRequest);
            Assert.AreEqual("device_test", device.DeviceName);
            Assert.AreEqual("localhost", device.MgmtInterface);
            Assert.AreEqual("username", device.Username);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void GetJobWithStatusStringTest()
        {
            var jobId = Guid.NewGuid();
            var getJobWithStatusRequest = new GetJobRequest(jobId);

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .SetupSequence(n => n.Invoke(getJobWithStatusRequest))
                .Returns(new MockHttpWebResponse(
                    "SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetJobWithStatusActiveResponse",
                    HttpStatusCode.OK, null))
                .Returns(new MockHttpWebResponse(
                    "SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetJobWithStatusCompletedResponse",
                    HttpStatusCode.OK, null))
                .Returns(new MockHttpWebResponse(
                    "SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetJobWithStatusCanceledResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var job = client.GetJob(getJobWithStatusRequest);
            Assert.AreEqual(new Guid("101bddb7-8b34-4b35-9ef5-3c829d561e19"), job.JobId);
            Assert.AreEqual(JobType.RESTORE, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(0, job.FilesTransferred);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual(10, job.BytesTransferred);
            Assert.AreEqual("2018-01-23T03:52:46.869Z[UTC]", job.Created);
            Assert.AreEqual(0.5, job.Progress);
            Assert.AreEqual("Active", job.Status.Message);
            Assert.AreEqual(JobStatusEnum.ACTIVE, job.Status.Status);

            job = client.GetJob(getJobWithStatusRequest);
            Assert.AreEqual(new Guid("101bddb7-8b34-4b35-9ef5-3c829d561e19"), job.JobId);
            Assert.AreEqual(JobType.ARCHIVE, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(1, job.FilesTransferred);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual(1234, job.BytesTransferred);
            Assert.AreEqual("2018-01-23T03:52:46.869Z[UTC]", job.Created);
            Assert.AreEqual(1.0, job.Progress);
            Assert.AreEqual("Completed", job.Status.Message);
            Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);

            job = client.GetJob(getJobWithStatusRequest);
            Assert.AreEqual(new Guid("101bddb7-8b34-4b35-9ef5-3c829d561e19"), job.JobId);
            Assert.AreEqual(JobType.CANCEL, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(0, job.FilesTransferred);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual(10, job.BytesTransferred);
            Assert.AreEqual("2018-01-23T03:52:46.869Z[UTC]", job.Created);
            Assert.AreEqual(1.0, job.Progress);
            Assert.AreEqual("Canceled", job.Status.Message);
            Assert.AreEqual(JobStatusEnum.CANCELED, job.Status.Status);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void HeadBrokerTest()
        {
            var headBrokerRequest = new HeadBrokerRequest(Stubs.BrokerName);

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(It.IsAny<HeadBrokerRequest>()))
                .Returns(new MockHttpWebResponse(null, HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            Assert.IsTrue(client.DoesBrokerExist("brokerName"));

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void HeadDeviceTest()
        {
            var headDeviceRequest = new HeadDeviceRequest("deviceName");

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(It.IsAny<HeadDeviceRequest>()))
                .Returns(new MockHttpWebResponse(null, HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            Assert.IsTrue(client.DoesDeviceExist("deviceName"));

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void RestoreTest()
        {
            var restoreRequest = new RestoreRequest(Stubs.BrokerName, Stubs.RestoreFiles);

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(restoreRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.RestoreResponse",
                    HttpStatusCode.Created, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var job = client.Restore(restoreRequest);
            Assert.AreEqual(new Guid("101bddb7-8b34-4b35-9ef5-3c829d561e19"), job.JobId);
            Assert.AreEqual(JobType.RESTORE, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual("2018-01-23T03:52:46.869Z[UTC]", job.Created);
            Assert.AreEqual(1.0, job.Progress);
            Assert.AreEqual("Completed", job.Status.Message);
            Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void RetryTest()
        {
            var jobId = Guid.NewGuid();
            var cancelRequest = new RetryRequest(jobId);

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(cancelRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.RetryResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var job = client.Retry(cancelRequest);
            Assert.AreEqual(new Guid("101bddb7-8b34-4b35-9ef5-3c829d561e19"), job.JobId);
            Assert.AreEqual(JobType.ARCHIVE, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual("2018-01-23T03:52:46.869Z[UTC]", job.Created);
            Assert.AreEqual(0.0, job.Progress);
            Assert.AreEqual("Retry", job.Status.Message);
            Assert.AreEqual(JobStatusEnum.ACTIVE, job.Status.Status);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        #endregion Public Methods
    }
}