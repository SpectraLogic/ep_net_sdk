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

using System.Net;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SpectraLogic.EscapePodClient.Calls;
using SpectraLogic.EscapePodClient.Model;
using SpectraLogic.EscapePodClient.Runtime;
using SpectraLogic.EscapePodClient.Test.Mock;
using SpectraLogic.EscapePodClient.Test.Utils;

namespace SpectraLogic.EscapePodClient.Test
{
    [TestFixture]
    internal class EscapePodClientTest
    {
        [Test]
        public void ArchiveTest()
        {
            var archiveRequest =
                JsonConvert.DeserializeObject<ArchiveRequest>(ResourceFilesUtils.Read("SpectraLogic.EscapePodClient.Test.TestFiles.ArchiveRequest"));
            Assert.AreEqual("/api/archives//jobs?operation=archive\nPOST\n{\"files\":[{\"name\":\"fileName\",\"uri\":\"uri\",\"size\":1234,\"metadata\":{\"key\":\"value\"},\"indexMedia\":false,\"storeFileProperties\":false}]}", archiveRequest.ToString());

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(archiveRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.EscapePodClient.Test.TestFiles.ArchiveResponse",
                    HttpStatusCode.Created, null));

            var mockBuilder = new Mock<IEscapePodClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new EscapePodClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var job = client.Archive(archiveRequest);
            Assert.AreEqual("101bddb7-8b34-4b35-9ef5-3c829d561e19", job.JobId.Id);
            Assert.AreEqual(EscapePodJobType.ARCHIVE, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual("2018-01-23T03:52:46.869Z[UTC]", job.Created);
            Assert.AreEqual(0.0, job.Progress);
            Assert.AreEqual("Initializing", job.Status.Message);
            Assert.AreEqual(JobStatus.ACTIVE, job.Status.Status);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void RestoreTest()
        {
            var restoreRequest =
                JsonConvert.DeserializeObject<RestoreRequest>(ResourceFilesUtils.Read("SpectraLogic.EscapePodClient.Test.TestFiles.RestoreRequest"));
            Assert.AreEqual("api/archives//jobs?operation=restore\nPOST\n{\"files\":[{\"name\":\"name\",\"uri\":\"dest\",\"restoreFileAttributes\":true},{\"name\":\"name2\",\"uri\":\"dest2\",\"byteRange\":{\"start\":0,\"stop\":10}},{\"name\":\"name3\",\"uri\":\"dest3\",\"timeCodeRange\":{\"start\":10,\"stop\":20}}]}", restoreRequest.ToString());

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(restoreRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.EscapePodClient.Test.TestFiles.RestoreResponse",
                    HttpStatusCode.Created, null));

            var mockBuilder = new Mock<IEscapePodClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new EscapePodClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var job = client.Restore(restoreRequest);
            Assert.AreEqual("101bddb7-8b34-4b35-9ef5-3c829d561e19", job.JobId.Id);
            Assert.AreEqual(EscapePodJobType.RESTORE, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual("2018-01-23T03:52:46.869Z[UTC]", job.Created);
            Assert.AreEqual(100, job.Progress);
            Assert.AreEqual("Done", job.Status.Message);
            Assert.AreEqual(JobStatus.DONE, job.Status.Status);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void DeleteTest()
        {
            var deleteRequest =
                JsonConvert.DeserializeObject<DeleteRequest>(ResourceFilesUtils.Read("SpectraLogic.EscapePodClient.Test.TestFiles.DeleteRequest"));
            Assert.AreEqual("api/delete\nDELETE\n{\"files\":[{\"name\":\"file1\"},{\"name\":\"file2\"}]}", deleteRequest.ToString());

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(deleteRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.EscapePodClient.Test.TestFiles.DeleteResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<IEscapePodClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new EscapePodClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var job = client.Delete(deleteRequest);
            Assert.AreEqual("101bddb7-8b34-4b35-9ef5-3c829d561e19", job.JobId.Id);
            Assert.AreEqual(EscapePodJobType.DELETE, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual("2018-01-23T03:52:46.869Z[UTC]", job.Created);
            Assert.AreEqual(100, job.Progress);
            Assert.AreEqual("Done", job.Status.Message);
            Assert.AreEqual(JobStatus.DONE, job.Status.Status);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void CancelTest()
        {
            var cancelRequest = new CancelRequest("123456789");
            Assert.AreEqual("api/cancel?id=123456789\nPUT", cancelRequest.ToString());

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(cancelRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.EscapePodClient.Test.TestFiles.CancelResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<IEscapePodClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new EscapePodClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var job = client.Cancel(cancelRequest);
            Assert.AreEqual("101bddb7-8b34-4b35-9ef5-3c829d561e19", job.JobId.Id);
            Assert.AreEqual(EscapePodJobType.CANCEL, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual("2018-01-23T03:52:46.869Z[UTC]", job.Created);
            Assert.AreEqual(100, job.Progress);
            Assert.AreEqual("Done", job.Status.Message);
            Assert.AreEqual(JobStatus.CANCELED, job.Status.Status);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void GetArchiveTest()
        {
            var getArchiveRequest = new GetArchiveRequest("archive");
            Assert.AreEqual("/api/archives/archive\nGET", getArchiveRequest.ToString());

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(getArchiveRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.EscapePodClient.Test.TestFiles.GetArchiveResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<IEscapePodClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new EscapePodClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var archive = client.GetArchive(getArchiveRequest);
            Assert.AreEqual("archive", archive.ArchiveName);
            Assert.AreEqual("2018-01-24T19:10:22.819Z[UTC]", archive.CreationDate);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void GetEscapePodJobWithStatusStringTest()
        {
            var getEscapePodJobWithStatusRequest = new GetEscapePodJobRequest("archiveName", "123456789");
            Assert.AreEqual("api/archive/archiveName/jobs/123456789\nGET", getEscapePodJobWithStatusRequest.ToString());

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .SetupSequence(n => n.Invoke(getEscapePodJobWithStatusRequest))
                .Returns(new MockHttpWebResponse(
                    "SpectraLogic.EscapePodClient.Test.TestFiles.GetEscapePodJobWithStatusInProgressResponse",
                    HttpStatusCode.OK, null))
                .Returns(new MockHttpWebResponse(
                    "SpectraLogic.EscapePodClient.Test.TestFiles.GetEscapePodJobWithStatusDoneResponse",
                    HttpStatusCode.OK, null))
                .Returns(new MockHttpWebResponse(
                    "SpectraLogic.EscapePodClient.Test.TestFiles.GetEscapePodJobWithStatusCanceledResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<IEscapePodClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new EscapePodClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var job = client.GetJob(getEscapePodJobWithStatusRequest);
            Assert.AreEqual("101bddb7-8b34-4b35-9ef5-3c829d561e19", job.JobId.Id);
            Assert.AreEqual(EscapePodJobType.RESTORE, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual("2018-01-23T03:52:46.869Z[UTC]", job.Created);
            Assert.AreEqual(50, job.Progress);
            Assert.AreEqual("In progress", job.Status.Message);
            Assert.AreEqual(JobStatus.IN_PROGRESS, job.Status.Status);

            job = client.GetJob(getEscapePodJobWithStatusRequest);
            Assert.AreEqual("101bddb7-8b34-4b35-9ef5-3c829d561e19", job.JobId.Id);
            Assert.AreEqual(EscapePodJobType.ARCHIVE, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual("2018-01-23T03:52:46.869Z[UTC]", job.Created);
            Assert.AreEqual(100, job.Progress);
            Assert.AreEqual("Done", job.Status.Message);
            Assert.AreEqual(JobStatus.DONE, job.Status.Status);

            job = client.GetJob(getEscapePodJobWithStatusRequest);
            Assert.AreEqual("101bddb7-8b34-4b35-9ef5-3c829d561e19", job.JobId.Id);
            Assert.AreEqual(EscapePodJobType.CANCEL, job.JobType);
            Assert.AreEqual(1, job.NumberOfFiles);
            Assert.AreEqual(1234, job.TotalSizeInBytes);
            Assert.AreEqual("2018-01-23T03:52:46.869Z[UTC]", job.Created);
            Assert.AreEqual(100, job.Progress);
            Assert.AreEqual("Done", job.Status.Message);
            Assert.AreEqual(JobStatus.CANCELED, job.Status.Status);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        [Test]
        public void CreateArchiveTest()
        {
            var createArchiveRequest =
                JsonConvert.DeserializeObject<CreateArchiveRequest>(ResourceFilesUtils.Read("SpectraLogic.EscapePodClient.Test.TestFiles.CreateArchiveRequest"));
            Assert.AreEqual("api/createarchive\nPOST\n{\"name\":\"archive\"}", createArchiveRequest.ToString());

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(createArchiveRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.EscapePodClient.Test.TestFiles.CreateArchiveResponse",
                    HttpStatusCode.OK, null));

            var mockBuilder = new Mock<IEscapePodClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new EscapePodClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var archive = client.CreateArchive(createArchiveRequest);
            Assert.AreEqual("archive", archive.ArchiveName);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }
    }
}
