/*
 * ******************************************************************************
 *   Copyright 2014-2020 Spectra Logic Corporation. All Rights Reserved.
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
using NUnit.Framework;
using SpectraLogic.SpectraRioBrokerClient.Calls.Authentication;
using SpectraLogic.SpectraRioBrokerClient.Calls.Broker;
using SpectraLogic.SpectraRioBrokerClient.Calls.Cluster;
using SpectraLogic.SpectraRioBrokerClient.Calls.Devices;
using SpectraLogic.SpectraRioBrokerClient.Calls.Jobs;
using SpectraLogic.SpectraRioBrokerClient.Calls.System;
using SpectraLogic.SpectraRioBrokerClient.Model;
using SpectraLogic.SpectraRioBrokerClient.Runtime;
using SpectraLogic.SpectraRioBrokerClient.Test.Mock;
using SpectraLogic.SpectraRioBrokerClient.Utils;
using System;
using System.Collections.Generic;
using System.Net;

namespace SpectraLogic.SpectraRioBrokerClient.Test
{
    [TestFixture]
    internal class SpectraRioBrokerClientTest
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger("SpectraRioBrokerClientTest");

        #endregion Fields

        #region Constructors

        public SpectraRioBrokerClientTest()
        {
            BasicConfigurator.Configure();
        }

        #endregion Constructors

        #region Methods

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
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.CreationDate.ToString());
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.LastUpdated.ToString());
            Assert.AreEqual(0.0, job.Progress);
            Assert.AreEqual("Initializing", job.Status.Message);
            Assert.AreEqual(JobStatusEnum.ACTIVE, job.Status.Status);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void ArchiveWithJobNameTest()
        {
            var archiveRequest = new ArchiveRequest(Stubs.BrokerName, Stubs.ArchiveFiles, jobName: "archive job");

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(archiveRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.ArchiveWithJobNameResponse",
                    HttpStatusCode.Created, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var job = client.Archive(archiveRequest);
            Assert.AreEqual("archive job", job.Name);
            Assert.AreEqual(new Guid("101bddb7-8b34-4b35-9ef5-3c829d561e19"), job.JobId);
            Assert.AreEqual(JobType.ARCHIVE, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.CreationDate.ToString());
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.LastUpdated.ToString());
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
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.CreationDate.ToString());
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.LastUpdated.ToString());
            Assert.AreEqual(1.0, job.Progress);
            Assert.AreEqual("Canceled", job.Status.Message);
            Assert.AreEqual(JobStatusEnum.CANCELED, job.Status.Status);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void CreateBrokerTest()
        {
            var createBrokerRequest = new CreateBrokerRequest(Stubs.BrokerName, Stubs.AgentName, Stubs.AgentConfig);

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
            Assert.AreEqual("1/30/2018 11:00:29 PM", broker.CreationDate.ToString());

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
        public void CreateSpectraDeviceTest()
        {
            var createSpectraDeviceRequest = new CreateSpectraDeviceRequest("device_test", "https://localhost".ToUri(), "username", "password");

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(createSpectraDeviceRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.CreateSpectraDeviceResponse",
                    HttpStatusCode.Created, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var device = client.CreateSpectraDevice(createSpectraDeviceRequest);
            Assert.AreEqual("device_test", device.DeviceName);
            Assert.AreEqual("https://localhost".ToUri(), device.MgmtInterface);
            Assert.AreEqual("username", device.Username);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void CreateTokenTest()
        {
            var createTokenRequest = new CreateTokenRequest("username", "password");

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(createTokenRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.CreateTokenResponse",
                    HttpStatusCode.Created, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var token = client.CreateToken(createTokenRequest).Token;
            Assert.AreEqual("token123", token);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void DeleteBrokerTest()
        {
            var deleteBrokerRequest = new DeleteBrokerRequest("broker");

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(It.IsAny<DeleteBrokerRequest>()))
                .Returns(new MockHttpWebResponse(null, HttpStatusCode.NoContent, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            client.DeleteBroker(deleteBrokerRequest);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void DeleteClusterTest()
        {
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
        public void DeleteDeviceTest()
        {
            var deleteDeviceRequest = new DeleteSpectraDeviceRequest("device");

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(deleteDeviceRequest))
                .Returns(new MockHttpWebResponse(null, HttpStatusCode.NoContent, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            client.DeleteSpectraDevice(deleteDeviceRequest);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void DeleteFileTest()
        {
            var deleteFileRequest = new DeleteFileRequest("broker", "test.file");

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(deleteFileRequest))
                .Returns(new MockHttpWebResponse(null, HttpStatusCode.NoContent, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            client.DeleteFile(deleteFileRequest);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void GetBrokerObjectsTest()
        {
            var getBrokerObjectsRequest = new GetBrokerObjectsRequest("brokerName");
            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getBrokerObjectsRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetBrokerObjectsResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var brokerObjects = client.GetBrokerObjects(getBrokerObjectsRequest);
            Assert.AreEqual(3, brokerObjects.Objects.Count);
            Assert.AreEqual(0, brokerObjects.Page.Number);
            Assert.AreEqual(100, brokerObjects.Page.PageSize);
            Assert.AreEqual(1, brokerObjects.Page.TotalPages);
        }

        [Test]
        public void GetBrokerObjectTest()
        {
            var getBrokerObjectRequest = new GetBrokerObjectRequest("brokerName", "objectName");
            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getBrokerObjectRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetBrokerObjectResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var brokerObject = client.GetBrokerObject(getBrokerObjectRequest);
            Assert.AreEqual("broker", brokerObject.Broker);
            Assert.AreEqual("5ac04144-bd37-4ee0-a661-09d4db08e9af", brokerObject.Name);
        }

        [Test]
        public void GetBrokersTest()
        {
            var getBrokersRequest = new GetBrokersRequest();

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getBrokersRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetBrokersResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var brokers = client.GetBrokers(getBrokersRequest);
            Assert.AreEqual(2, brokers.BrokerList.Count);

            Assert.AreEqual(0, brokers.Page.Number);
            Assert.AreEqual(100, brokers.Page.PageSize);
            Assert.AreEqual(1, brokers.Page.TotalPages);

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
            Assert.AreEqual("1/24/2018 7:10:22 PM", broker.CreationDate.ToString());
            Assert.AreEqual(10, broker.ObjectCount);

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
        public void GetJobFileStatusesTest()
        {
            var jobId = Guid.NewGuid();
            var getJobFileStatusesRequest = new GetJobFileStatusesRequest(jobId, "");

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getJobFileStatusesRequest))
                .Returns(new MockHttpWebResponse(
                    "SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetJobFileStatusesResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var jobFileStatus = client.GetJobFileStatuses(getJobFileStatusesRequest);
            Assert.AreEqual(0, jobFileStatus.Page.Number);
            Assert.AreEqual(100, jobFileStatus.Page.PageSize);
            Assert.AreEqual(1, jobFileStatus.Page.TotalPages);
            Assert.AreEqual(3, jobFileStatus.FileStatusesList.Count);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void GetJobFileStatusTest()
        {
            var jobId = Guid.NewGuid();
            var getJobFilesStatusRequest = new GetJobFilesStatusRequest(jobId);

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getJobFilesStatusRequest))
                .Returns(new MockHttpWebResponse(
                    "SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetJobFilesStatusResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var jobFileStatus = client.GetJobFilesStatus(getJobFilesStatusRequest);
            Assert.AreEqual(0, jobFileStatus.Page.Number);
            Assert.AreEqual(100, jobFileStatus.Page.PageSize);
            Assert.AreEqual(1, jobFileStatus.Page.TotalPages);
            Assert.AreEqual(6, jobFileStatus.FilesStatusList.Count);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void GetJobsMultiParamsTest()
        {
            var getJobsRequest = new GetJobsRequest(jobTypes: new List<JobTypeEnum> { JobTypeEnum.ARCHIVE, JobTypeEnum.RESTORE });

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getJobsRequest))
                .Returns(new MockHttpWebResponse(
                    "SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetJobsResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var jobs = client.GetJobs(getJobsRequest);
            Assert.AreEqual(2, jobs.JobsList.Count);
            Assert.AreEqual(0, jobs.Page.Number);
            Assert.AreEqual(100, jobs.Page.PageSize);
            Assert.AreEqual(1, jobs.Page.TotalPages);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void GetJobsTest()
        {
            var getJobsRequest = new GetJobsRequest();

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getJobsRequest))
                .Returns(new MockHttpWebResponse(
                    "SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetJobsResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var jobs = client.GetJobs(getJobsRequest);
            Assert.AreEqual(2, jobs.JobsList.Count);
            Assert.AreEqual(0, jobs.Page.Number);
            Assert.AreEqual(100, jobs.Page.PageSize);
            Assert.AreEqual(1, jobs.Page.TotalPages);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void GetJobsWithNameSearchTest()
        {
            var getJobsRequest = new GetJobsRequest();

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getJobsRequest))
                .Returns(new MockHttpWebResponse(
                    "SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetJobsWithNameSearchResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var jobs = client.GetJobs(getJobsRequest);
            Assert.AreEqual(2, jobs.JobsList.Count);
            Assert.AreEqual(0, jobs.Page.Number);
            Assert.AreEqual(100, jobs.Page.PageSize);
            Assert.AreEqual(1, jobs.Page.TotalPages);
            foreach (var job in jobs.JobsList)
            {
                Assert.AreEqual("archive job", job.Name);
            }
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
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.CreationDate.ToString());
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.LastUpdated.ToString());
            Assert.AreEqual(0.5, job.Progress);
            Assert.AreEqual("Active", job.Status.Message);
            Assert.AreEqual(JobStatusEnum.ACTIVE, job.Status.Status);
            foreach (var file in job.Files)
            {
                Assert.AreEqual("Transferring", file.Status);
            }

            job = client.GetJob(getJobWithStatusRequest);
            Assert.AreEqual(new Guid("101bddb7-8b34-4b35-9ef5-3c829d561e19"), job.JobId);
            Assert.AreEqual(JobType.ARCHIVE, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(1, job.FilesTransferred);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.CreationDate.ToString());
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.LastUpdated.ToString());
            Assert.AreEqual(1.0, job.Progress);
            Assert.AreEqual("Completed", job.Status.Message);
            Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
            foreach (var file in job.Files)
            {
                Assert.AreEqual("Completed", file.Status);
            }

            job = client.GetJob(getJobWithStatusRequest);
            Assert.AreEqual(new Guid("101bddb7-8b34-4b35-9ef5-3c829d561e19"), job.JobId);
            Assert.AreEqual(JobType.CANCEL, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(0, job.FilesTransferred);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.CreationDate.ToString());
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.LastUpdated.ToString());
            Assert.AreEqual(1.0, job.Progress);
            Assert.AreEqual("Canceled", job.Status.Message);
            Assert.AreEqual(JobStatusEnum.CANCELED, job.Status.Status);
            foreach (var file in job.Files)
            {
                Assert.AreEqual("Canceled", file.Status);
            }

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void GetJosTest()
        {
            var jobId = Guid.NewGuid();
            var getJobRequest = new GetJobRequest(jobId);

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getJobRequest))
                .Returns(new MockHttpWebResponse(
                    "SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetJobResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var job = client.GetJob(getJobRequest);
            Assert.AreEqual("job name", job.Name);
            Assert.AreEqual("173e8530-5805-4a1c-a57b-969331e49683", job.JobId.ToString());
            Assert.AreEqual("12/17/2018 10:00:34 PM", job.CreationDate.ToString());
            Assert.AreEqual("12/17/2018 10:00:45 PM", job.LastUpdated.ToString());
            Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
            Assert.AreEqual(JobType.ARCHIVE, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(14, job.TotalSizeInBytes);
            Assert.AreEqual(1, job.Progress);

            var jobFileStatusExpected = new JobFileStatus(
                "9edc2043-0ea7-4388-8c8d-fdfa68f68894",
                "Completed",
                "Successfully transferred file to BlackPearl",
                new Uri("atozsequence://file"),
                14,
                "2020-08-31T22:12:36.59Z[UTC]");
            Assert.AreEqual(jobFileStatusExpected.Name, job.Files[0].Name);
            Assert.AreEqual(jobFileStatusExpected.Status, job.Files[0].Status);
            Assert.AreEqual(jobFileStatusExpected.StatusMessage, job.Files[0].StatusMessage);
            Assert.AreEqual(jobFileStatusExpected.Uri.ToString(), job.Files[0].Uri.ToString());
            Assert.AreEqual(jobFileStatusExpected.SizeInBytes, job.Files[0].SizeInBytes);
            Assert.AreEqual(jobFileStatusExpected.LastUpdated, job.Files[0].LastUpdated);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void GetMasterTest()
        {
            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(It.IsAny<GetMasterRequest>()))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetMasterResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var master = client.GetMaster();
            Assert.AreEqual("b4efd674-ae74-4327-8b53-5be871688865", master.MemberId);
            Assert.AreEqual("127.0.0.1", master.IpAddress);
            Assert.AreEqual(5701, master.ClusterPort);
            Assert.AreEqual(5050, master.HttpPort);
            Assert.AreEqual("master", master.Role);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void GetMembersTest()
        {
            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(It.IsAny<GetMembersRequest>()))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetMembersResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var members = client.GetMembers();
            Assert.AreEqual(2, members.Members.Count);

            var master = members.Members[0];
            Assert.AreEqual("127.0.0.1", master.IpAddress);
            Assert.AreEqual(5701, master.ClusterPort);
            Assert.AreEqual(5050, master.HttpPort);
            Assert.AreEqual("master", master.Role);

            var node = members.Members[1];
            Assert.AreEqual("127.0.0.1", node.IpAddress);
            Assert.AreEqual(5702, node.ClusterPort);
            Assert.AreEqual(5051, node.HttpPort);
            Assert.AreEqual("node", node.Role);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void GetSpectraDevicesTest()
        {
            var getSpectraDevicesRequest = new GetSpectraDevicesRequest();

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getSpectraDevicesRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetSpectraDevicesResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var devices = client.GetSpectraDevices(getSpectraDevicesRequest);

            Assert.AreEqual(0, devices.Page.Number);
            Assert.AreEqual(100, devices.Page.PageSize);
            Assert.AreEqual(1, devices.Page.TotalPages);

            Assert.AreEqual("sm2u-11", devices.DeviceList[0].DeviceName);
            Assert.AreEqual("https://sm2u-11-mgmt.eng.sldomain.com".ToUri(), devices.DeviceList[0].MgmtInterface);
            Assert.AreEqual("Administrator", devices.DeviceList[0].Username);

            Assert.AreEqual("sm25-2", devices.DeviceList[1].DeviceName);
            Assert.AreEqual("https://sm25-2-mgmt.eng.sldomain.com".ToUri(), devices.DeviceList[1].MgmtInterface);
            Assert.AreEqual("Administrator", devices.DeviceList[1].Username);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void GetSpectraDeviceTest()
        {
            var getSpectraDeviceRequest = new GetSpectraDeviceRequest("device_test");

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getSpectraDeviceRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetSpectraDeviceResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var device = client.GetSpectraDevice(getSpectraDeviceRequest);
            Assert.AreEqual("device_test", device.DeviceName);
            Assert.AreEqual("https://localhost".ToUri(), device.MgmtInterface);
            Assert.AreEqual("username", device.Username);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void GetSystemTest()
        {
            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(It.IsAny<GetSystemRequest>()))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetSystemResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var system = client.GetSystem();
            Assert.AreEqual("0.2.9-dev", system.Version);
            Assert.AreEqual("11/13/2018 6:19:52 PM", system.BuildDate.ToString());
        }

        [Test]
        public void HeadBrokerObjectTest()
        {
            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .SetupSequence(n => n.Invoke(It.IsAny<HeadBrokerObjectRequest>()))
                .Returns(new MockHttpWebResponse(null, HttpStatusCode.OK, null))
                .Returns(new MockHttpWebResponse(null, HttpStatusCode.NotFound, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            Assert.IsTrue(client.DoesBrokerObjectExist("brokerName", "objectName"));
            Assert.IsFalse(client.DoesBrokerObjectExist("brokerName", "objectNameNotFound"));

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void HeadBrokerTest()
        {
            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .SetupSequence(n => n.Invoke(It.IsAny<HeadBrokerRequest>()))
                .Returns(new MockHttpWebResponse(null, HttpStatusCode.OK, null))
                .Returns(new MockHttpWebResponse(null, HttpStatusCode.NotFound, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            Assert.IsTrue(client.DoesBrokerExist("brokerName"));
            Assert.IsFalse(client.DoesBrokerExist("brokerNameNotFound"));

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void HeadJobTest()
        {
            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .SetupSequence(n => n.Invoke(It.IsAny<HeadJobRequest>()))
                .Returns(new MockHttpWebResponse(null, HttpStatusCode.OK, null))
                .Returns(new MockHttpWebResponse(null, HttpStatusCode.NotFound, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            Assert.IsTrue(client.DoesJobExist(new Guid()));
            Assert.IsFalse(client.DoesJobExist(new Guid()));

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void HeadSpectraDeviceTest()
        {
            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .SetupSequence(n => n.Invoke(It.IsAny<HeadSpectraDeviceRequest>()))
                .Returns(new MockHttpWebResponse(null, HttpStatusCode.OK, null))
                .Returns(new MockHttpWebResponse(null, HttpStatusCode.NotFound, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            Assert.IsTrue(client.DoesSpectraDeviceExist("deviceName"));
            Assert.IsFalse(client.DoesSpectraDeviceExist("deviceNameNotFound"));

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
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.CreationDate.ToString());
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.LastUpdated.ToString());
            Assert.AreEqual(1.0, job.Progress);
            Assert.AreEqual("Completed", job.Status.Message);
            Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void RestoreWithJobNameTest()
        {
            var restoreRequest = new RestoreRequest(Stubs.BrokerName, Stubs.RestoreFiles, jobName: "restore job");

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(restoreRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.RestoreWithJobNameResponse",
                    HttpStatusCode.Created, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var job = client.Restore(restoreRequest);
            Assert.AreEqual("restore job", job.Name);
            Assert.AreEqual(new Guid("101bddb7-8b34-4b35-9ef5-3c829d561e19"), job.JobId);
            Assert.AreEqual(JobType.RESTORE, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.CreationDate.ToString());
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.LastUpdated.ToString());
            Assert.AreEqual(1.0, job.Progress);
            Assert.AreEqual("Completed", job.Status.Message);
            Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void RetryTest()
        {
            var cancelRequest = new RetryRequest("", Guid.NewGuid(), JobType.ARCHIVE);

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
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.CreationDate.ToString());
            Assert.AreEqual("1/23/2018 3:52:46 AM", job.LastUpdated.ToString());
            Assert.AreEqual(0.0, job.Progress);
            Assert.AreEqual("Retry", job.Status.Message);
            Assert.AreEqual(JobStatusEnum.ACTIVE, job.Status.Status);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void UpdateBrokerObjectTest()
        {
            var updateBrokerObjectRequest = new UpdateBrokerObjectRequest("brokerName", "objectName", new Dictionary<string, string>());
            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(updateBrokerObjectRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.UpdateBrokerObjectResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var brokerObject = client.UpdateBrokerObject(updateBrokerObjectRequest);
            Assert.AreEqual("broker", brokerObject.Broker);
            Assert.AreEqual("5ac04144-bd37-4ee0-a661-09d4db08e9af", brokerObject.Name);
            Assert.AreEqual(1, brokerObject.Metadata.Count);
        }

        #endregion Methods
    }
}