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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using log4net;
using log4net.Config;
using Moq;
using NUnit.Framework;
using SpectraLogic.SpectraRioBrokerClient.Calls.Authentication;
using SpectraLogic.SpectraRioBrokerClient.Calls.Broker;
using SpectraLogic.SpectraRioBrokerClient.Calls.Cluster;
using SpectraLogic.SpectraRioBrokerClient.Calls.DevicesSpectra;
using SpectraLogic.SpectraRioBrokerClient.Calls.Jobs;
using SpectraLogic.SpectraRioBrokerClient.Calls.System;
using SpectraLogic.SpectraRioBrokerClient.Model;
using SpectraLogic.SpectraRioBrokerClient.Runtime;
using SpectraLogic.SpectraRioBrokerClient.Test.Mock;
using SpectraLogic.SpectraRioBrokerClient.Utils;

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
        public void CreateDeviceTest()
        {
            var createDeviceRequest = new CreateDeviceRequest("device_test", "https://localhost".ToUri(), "username", "password");

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
            var deleteDeviceRequest = new DeleteDeviceRequest("device");

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

            client.DeleteDevice(deleteDeviceRequest);

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
        public void GetBrokerRelationshipsTest()
        {
            var getRelationshipsRequest = new GetBrokerRelationshipsRequest("brokerName");
            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getRelationshipsRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetBrokerRelationshipsResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var relationships = client.GetBrokerRelationships(getRelationshipsRequest);
            Assert.AreEqual(3, relationships.Relationships.Count);

            Assert.AreEqual(0, relationships.Page.Number);
            Assert.AreEqual(100, relationships.Page.PageSize);
            Assert.AreEqual(1, relationships.Page.TotalPages);
        }

        [Test]
        public void GetBrokerRelationshipTest()
        {
            var getRelationshipRequest = new GetBrokerRelationshipRequest("brokerName", "relationship");
            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getRelationshipRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetBrokerRelationshipResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var relationship = client.GetBrokerRelationship(getRelationshipRequest);
            Assert.AreEqual(3, relationship.Objects.Count);
            foreach (var obj in relationship.Objects)
            {
                Assert.AreEqual(1, obj.Relationships.Count);
                Assert.AreEqual("relation1", obj.Relationships.First());
            }
            Assert.AreEqual(0, relationship.Page.Number);
            Assert.AreEqual(100, relationship.Page.PageSize);
            Assert.AreEqual(1, relationship.Page.TotalPages);
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
        public void GetDevicesTest()
        {
            var getDevicesRequest = new GetDevicesRequest();
            
            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getDevicesRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.GetDevicesResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var devices = client.GetDevices(getDevicesRequest);
            
            Assert.AreEqual(0, devices.Page.Number);
            Assert.AreEqual(100, devices.Page.PageSize);
            Assert.AreEqual(1, devices.Page.TotalPages);
            
            Assert.AreEqual("sm2u-11", devices.DeviceList[0].DeviceName);
            Assert.AreEqual("https://sm2u-11-mgmt.eng.sldomain.com", devices.DeviceList[0].MgmtInterface);
            Assert.AreEqual("Administrator", devices.DeviceList[0].Username);
            
            Assert.AreEqual("sm25-2", devices.DeviceList[1].DeviceName);
            Assert.AreEqual("https://sm25-2-mgmt.eng.sldomain.com", devices.DeviceList[1].MgmtInterface);
            Assert.AreEqual("Administrator", devices.DeviceList[1].Username);

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
            Assert.AreEqual(6, jobFileStatus.FilesStatusList.Count);

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
            Assert.AreEqual(3, jobFileStatus.FileStatusesList.Count);

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
        public void HeadDeviceTest()
        {
            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .SetupSequence(n => n.Invoke(It.IsAny<HeadDeviceRequest>()))
                .Returns(new MockHttpWebResponse(null, HttpStatusCode.OK, null))
                .Returns(new MockHttpWebResponse(null, HttpStatusCode.NotFound, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            Assert.IsTrue(client.DoesDeviceExist("deviceName"));
            Assert.IsFalse(client.DoesDeviceExist("deviceNameNotFound"));

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
            var updateBrokerObjectRequest = new UpdateBrokerObjectRequest("brokerName", "objectName", new Dictionary<string, string>(), new HashSet<string>());
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
            Assert.AreEqual(1, brokerObject.Relationships.Count);
        }

        #endregion Methods
    }
}