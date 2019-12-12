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
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using log4net;
using NUnit.Framework;
using SpectraLogic.SpectraRioBrokerClient.Calls.Authentication;
using SpectraLogic.SpectraRioBrokerClient.Calls.Broker;
using SpectraLogic.SpectraRioBrokerClient.Calls.Devices;
using SpectraLogic.SpectraRioBrokerClient.Calls.Jobs;
using SpectraLogic.SpectraRioBrokerClient.Exceptions;
using SpectraLogic.SpectraRioBrokerClient.Model;

namespace SpectraLogic.SpectraRioBrokerClient.Integration.Test
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class SpectraRioBrokerClientIntegrationTests
    {
        private const int MaxPollingAttempts = 10;
        private const int PollingInterval = 10; // in sec

        private readonly ILog _log = LogManager.GetLogger("SpectraRioBrokerClientIntegrationTest");

        [OneTimeSetUp]
        public void Setup()
        {
            SpectraRioBrokerClientFixture.SetupTestData();
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
            Directory.Delete(SpectraRioBrokerClientFixture.RestoreTempDir, true);
        }
        
        private void ArchiveNewFilesOnlyTest(IEnumerable<ArchiveFile> list1, IEnumerable<ArchiveFile> list2,
            string message)
        {
                var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, list1);
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

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                    new GetJobRequest(archiveJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Archive job completed successfully", job.Status.Message);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }

                archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, list2, true);
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

                job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(archiveJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual(message, job.Status.Message);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }
        }

        [Test]
        public void ArchiveAndDeleteWithSpecialCharacters()
        {
            var fileName1 = "Archive@And&Delete#With$Special Characters/" + Guid.NewGuid();

                var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, "F1.txt".ToAtoZUri(), 10)
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

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                    new GetJobRequest(archiveJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Archive job completed successfully", job.Status.Message);
                Assert.AreEqual(1, job.FilesTransferred);
                Assert.AreEqual(1, job.Progress);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }

                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
        }

        [Test]
        public void ArchiveAndRestore()
        {
            var fileName1 = "ArchiveAndRestore_" + Guid.NewGuid();
            var fileName2 = "ArchiveAndRestore_" + Guid.NewGuid();

            try
            {
                /***********
                 * ARCHIVE *
                 ***********/
                var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, "F1.txt".ToAtoZUri(), 14,
                        new Dictionary<string, string> {{"fileName", fileName1}}, false),
                    new ArchiveFile(fileName2, "F2.txt".ToAtoZUri(), 14,
                        new Dictionary<string, string> {{"fileName", fileName2}}, false)
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

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                    new GetJobRequest(archiveJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Archive job completed successfully", job.Status.Message);
                Assert.AreEqual(2, job.FilesTransferred);
                Assert.AreEqual(1, job.Progress);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }

                /*********
                * RESTORE *
                ***********/

                var restoreRequest = new RestoreRequest(SpectraRioBrokerClientFixture.BrokerName, new List<RestoreFile>
                {
                    new RestoreFile(fileName1,
                        "F1_restore.txt".ToDevNullUri()),
                    new RestoreFile(fileName2,
                        "F2_restore.txt".ToDevNullUri(),
                        new ByteRange(0, 10))
                });

                var restoreJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(restoreRequest);

                pollingAttempts = 0;
                do
                {
                    restoreJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                        new GetJobRequest(restoreJob.JobId));
                    _log.Debug(restoreJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(PollingInterval));
                    pollingAttempts++;
                } while (restoreJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttempts < MaxPollingAttempts);

                Assert.Less(pollingAttempts, MaxPollingAttempts);
                Assert.AreEqual(JobStatusEnum.COMPLETED, restoreJob.Status.Status);

                job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(restoreJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Restore job completed successfully", job.Status.Message);
                Assert.AreEqual(2, job.FilesTransferred);
                Assert.AreEqual(1, job.Progress);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                var deleteF2Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName2);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF2Request);
            }
        }

        [Test]
        public void ArchiveAndRestoreWithRelationship()
        {
            var fileName1 = "ArchiveAndRestoreWithRelationship_" + Guid.NewGuid();
            var fileName2 = "ArchiveAndRestoreWithRelationship_" + Guid.NewGuid();

            try
            {
                /***********
                 * ARCHIVE *
                 ***********/
                const string relationshipName = "ArchiveAndRestoreWithRelationship";
                var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, "F1.txt".ToAtoZUri(), 14,
                        new Dictionary<string, string> {{"fileName", fileName1}}, false,
                        new HashSet<string> {relationshipName}),
                    new ArchiveFile(fileName2, "F2.txt".ToAtoZUri(), 14,
                        new Dictionary<string, string> {{"fileName", fileName2}}, false,
                        new HashSet<string> {relationshipName})
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

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                    new GetJobRequest(archiveJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Archive job completed successfully", job.Status.Message);
                Assert.AreEqual(2, job.FilesTransferred);
                Assert.AreEqual(1, job.Progress);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }

                var relationshipsRequest = new GetBrokerRelationshipsRequest(SpectraRioBrokerClientFixture.BrokerName);
                var relationships =
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerRelationships(relationshipsRequest);

                Assert.AreEqual(1, relationships.Relationships.Count);
                Assert.AreEqual(new List<string> {relationshipName}, relationships.Relationships);

                var relationshipRequest =
                    new GetBrokerRelationshipRequest(SpectraRioBrokerClientFixture.BrokerName, relationshipName);
                var relationship =
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerRelationship(relationshipRequest);

                Assert.AreEqual(2, relationship.Objects.Count);
                foreach (var obj in relationship.Objects)
                {
                    Assert.AreEqual(1, obj.Relationships.Count);
                    Assert.AreEqual(relationshipName, obj.Relationships.First());
                }

                /**********
                * RESTORE *
                ***********/

                var restoreList = relationship.Objects.Select(obj => new RestoreFile(obj.Name,
                    $"{obj.Name}".ToDevNullUri()));

                var restoreRequest = new RestoreRequest(SpectraRioBrokerClientFixture.BrokerName, restoreList);
                var restoreJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(restoreRequest);

                pollingAttempts = 0;
                do
                {
                    restoreJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                        new GetJobRequest(restoreJob.JobId));
                    _log.Debug(restoreJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(PollingInterval));
                    pollingAttempts++;
                } while (restoreJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttempts < MaxPollingAttempts);

                Assert.Less(pollingAttempts, MaxPollingAttempts);
                Assert.AreEqual(JobStatusEnum.COMPLETED, restoreJob.Status.Status);

                job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(restoreJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Restore job completed successfully", job.Status.Message);
                Assert.AreEqual(2, job.FilesTransferred);
                Assert.AreEqual(1, job.Progress);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }

                var updatedMetadata = new Dictionary<string, string> {{"test", "update"}};
                var updatedRelationships = new HashSet<string> {"test1", "test2"};
                var updateBrokerObjectRequest = new UpdateBrokerObjectRequest(SpectraRioBrokerClientFixture.BrokerName,
                    fileName1, updatedMetadata, updatedRelationships);
                var updatedObject =
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.UpdateBrokerObject(updateBrokerObjectRequest);

                Assert.AreEqual(1, updatedObject.Metadata.Count);
                Assert.AreEqual(updatedMetadata, updatedObject.Metadata);

                Assert.AreEqual(2, updatedObject.Relationships.Count);
                Assert.AreEqual(updatedRelationships, updatedObject.Relationships);
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                var deleteF2Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName2);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF2Request);
            }
        }

        [Test]
        public void ArchiveNewFilesOnlyTest_1()
        {
            var fileName1 = "ArchiveNewFilesOnlyTest_1_" + Guid.NewGuid();
            var fileName2 = "ArchiveNewFilesOnlyTest_1_" + Guid.NewGuid();

            try
            {
                ArchiveNewFilesOnlyTest(
                    new List<ArchiveFile>
                    {
                        new ArchiveFile(fileName1, "F1.txt".ToAtoZUri(),
                            14, new Dictionary<string, string> {{"fileName", fileName1}}, false),
                        new ArchiveFile(fileName2, "F2.txt".ToAtoZUri(),
                            14, new Dictionary<string, string> {{"fileName", fileName2}}, false)
                    },
                    new List<ArchiveFile>
                    {
                        new ArchiveFile(fileName1, "F1.txt".ToAtoZUri(),
                            14, new Dictionary<string, string> {{"fileName", fileName1}}, false),
                        new ArchiveFile(fileName2, "F2.txt".ToAtoZUri(),
                            14, new Dictionary<string, string> {{"fileName", fileName2}}, false)
                    },
                    "All files already exist. Nothing to archive.");
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                var deleteF2Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName2);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF2Request);
            }
        }

        [Test]
        public void ArchiveNewFilesOnlyTest_2()
        {
            var fileName1 = "ArchiveNewFilesOnlyTest_2_" + Guid.NewGuid();
            var fileName2 = "ArchiveNewFilesOnlyTest_2_" + Guid.NewGuid();

            try
            {
                ArchiveNewFilesOnlyTest(
                    new List<ArchiveFile>
                    {
                        new ArchiveFile(fileName1, "F1.txt".ToAtoZUri(),
                            14, new Dictionary<string, string> {{"fileName", fileName1}}, false)
                    },
                    new List<ArchiveFile>
                    {
                        new ArchiveFile(fileName1, "F1.txt".ToAtoZUri(),
                            14, new Dictionary<string, string> {{"fileName", fileName1}}, false),
                        new ArchiveFile(fileName2, "F2.txt".ToAtoZUri(),
                            14, new Dictionary<string, string> {{"fileName", fileName2}}, false)
                    },
                    "Archive job completed successfully");
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                var deleteF2Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName2);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF2Request);
            }
        }

        [Test]
        public void ArchiveWithFailFastFalse()
        {
            var fileName1 = "ArchiveWithFailFastFalse_" + Guid.NewGuid();

            var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, new List<ArchiveFile>
            {
                new ArchiveFile(fileName1, "not_found.txt".ToFileUri(),
                    14,
                    new Dictionary<string, string> {{"fileName", fileName1}}),
            }, failFast: false);

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
            Assert.AreEqual(JobStatusEnum.ERROR, archiveJob.Status.Status);

            var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                new GetJobRequest(archiveJob.JobId));

            Assert.AreEqual(JobStatusEnum.ERROR, job.Status.Status);
            Assert.AreEqual("Archive job encountered an error while running", job.Status.Message);
            Assert.AreEqual(0, job.FilesTransferred);
            Assert.AreEqual(0, job.Progress);
            foreach (var file in job.Files)
            {
                Assert.AreEqual("Error", file.Status);
            }
        }

        [Test]
        public void ArchiveWithoutSize()
        {
            var fileName1 = "ArchiveWithoutSize" + Guid.NewGuid();
            var fileName2 = "ArchiveWithoutSize" + Guid.NewGuid();

            try
            {
                /***********
                 * ARCHIVE *
                 ***********/
                var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F1_No_Size.txt".ToFileUri()),
                    new ArchiveFile(fileName2, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F2_No_Size.txt".ToFileUri())
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

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                    new GetJobRequest(archiveJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Archive job completed successfully", job.Status.Message);
                Assert.AreEqual(2, job.FilesTransferred);
                Assert.AreEqual(1, job.Progress);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                var deleteF2Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName2);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF2Request);
            }
        }

        [Test]
        public void CancelArchiveJob()
        {
                var fileName1 = "CancelArchiveJob_" + Guid.NewGuid();
                var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, "F1.txt".ToAtoZUri(), 14,
                        new Dictionary<string, string> {{"fileName", fileName1}}, false)
                });

                var archiveJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Archive(archiveRequest);

                Assert.AreEqual(JobStatusEnum.ACTIVE, archiveJob.Status.Status);

                var cancelRequest = new CancelRequest(archiveJob.JobId);
                var cancel = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Cancel(cancelRequest);

                Assert.AreEqual(JobStatusEnum.CANCELED, cancel.Status.Status);
                Assert.AreEqual("Canceled", cancel.Status.Message);

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                    new GetJobRequest(archiveJob.JobId));

                Assert.AreEqual(JobStatusEnum.CANCELED, job.Status.Status);
                Assert.AreEqual("Canceled", job.Status.Message);

                //TODO can be tested after ESCP-1023 is fixed
                //foreach (var file in job.Files)
                //{
                //    Assert.AreEqual("Canceled", file.Status);
                //}
            }

        [Test]
        public void CancelRestoreJob()
        {
            var fileName1 = "CancelRestoreJob_" + Guid.NewGuid();

            try
            {
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

                var restoreRequest = new RestoreRequest(SpectraRioBrokerClientFixture.BrokerName, new List<RestoreFile>
                {
                    new RestoreFile(fileName1,
                        "F1_restore.txt".ToDevNullUri())
                });

                var restoreJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(restoreRequest);

                Assert.AreEqual(JobStatusEnum.ACTIVE, restoreJob.Status.Status);

                var cancelRequest = new CancelRequest(restoreJob.JobId);
                var cancel = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Cancel(cancelRequest);

                Assert.AreEqual(JobStatusEnum.CANCELED, cancel.Status.Status);
                Assert.AreEqual("Canceled", cancel.Status.Message);

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                    new GetJobRequest(restoreJob.JobId));

                Assert.AreEqual(JobStatusEnum.CANCELED, job.Status.Status);
                Assert.AreEqual("Canceled", job.Status.Message);
                //TODO can be tested after ESCP-1023 is fixed
                //foreach (var file in job.Files)
                //{
                //    Assert.AreEqual("Canceled", file.Status);
                //}
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
            }
        }

        [Test]
        public void DeleteBrokersTest()
        {
            const string brokerName = "DeleteBrokersTest";
            SpectraRioBrokerClientFixture.SpectraRioBrokerClient.CreateBroker(new CreateBrokerRequest(brokerName, SpectraRioBrokerClientFixture.AgentName, SpectraRioBrokerClientFixture.GetAgentConfig()));
            Assert.DoesNotThrow(() => SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBroker(new GetBrokerRequest(brokerName)));
            
            SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteBroker(
                new DeleteBrokerRequest(brokerName));
            Assert.That(
                () => SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBroker(
                    new GetBrokerRequest(brokerName)),
                Throws.Exception.TypeOf<BrokerNotFoundException>());
        }

        [Test]
        public void DeleteDeviceTest()
        {
            SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteSpectraDevice(
                new DeleteSpectraDeviceRequest(SpectraRioBrokerClientFixture.DeviceName));
            Assert.That(
                () => SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetSpectraDevice(
                    new GetSpectraDeviceRequest(SpectraRioBrokerClientFixture.DeviceName)),
                Throws.Exception.TypeOf<DeviceNotFoundException>());
            SpectraRioBrokerClientFixture.CreateSpectraDevice();
        }

        [Test]
        public void DoesBrokerObjectExistTest()
        {
            var fileName1 = "DoesBrokerObjectExistTest_" + Guid.NewGuid();

            try
            {
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

                Assert.IsTrue(
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DoesBrokerObjectExist(
                        SpectraRioBrokerClientFixture.BrokerName, fileName1));
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
            }
        }

        [Test]
        public void DoesJobExistTest()
        {
            var fileName1 = Guid.NewGuid().ToString();

            try
            {
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

                Assert.IsTrue(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DoesJobExist(archiveJob.JobId));
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
            }
        }

        [Test]
        public void GetBrokerObjectTest()
        {
            var fileName1 = "GetBrokerObjectTest_" + Guid.NewGuid();

            try
            {
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

                var getBrokerObjectRequest =
                    new GetBrokerObjectRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                var brokerObject =
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObject(getBrokerObjectRequest);

                Assert.AreEqual(SpectraRioBrokerClientFixture.BrokerName, brokerObject.Broker);
                Assert.AreEqual(fileName1, brokerObject.Name);
                Assert.AreEqual(14, brokerObject.Size);
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
            }
        }

        [Test]
        public void GetBrokerObjectTestWithSpecialCharacters()
        {
            var fileName1 = "Get!Broker@Object#Test$With%Special Characters/" + Guid.NewGuid();

            try
            {
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

                var getBrokerObjectRequest =
                    new GetBrokerObjectRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                var brokerObject =
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObject(getBrokerObjectRequest);

                Assert.AreEqual(SpectraRioBrokerClientFixture.BrokerName, brokerObject.Broker);
                Assert.AreEqual(fileName1, brokerObject.Name);
                Assert.AreEqual(14, brokerObject.Size);
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
            }
        }

        [Test]
        public void GetMasterTest()
        {
            var master = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetMaster();
            Assert.AreEqual(5701, master.ClusterPort);
            Assert.AreEqual(5050, master.HttpPort);
            Assert.AreEqual("master", master.Role);
        }

        [Test]
        public void GetMembersTest()
        {
            var members = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetMembers();
            Assert.AreEqual(1, members.Members.Count);
            var node = members.Members[0];
            Assert.AreEqual(5701, node.ClusterPort);
            Assert.AreEqual(5050, node.HttpPort);
            Assert.AreEqual("master", node.Role);
        }

        [Test]
        public void GetSystemTest()
        {
            Assert.DoesNotThrow(() =>
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetSystem());
        }

        [Test]
        public void RestoreJobWithIgnoreDuplicates()
        {
            var fileName1 = "RestoreJobWithIgnoreDuplicates_" + Guid.NewGuid();

            try
            {
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

                //Using search and restore
                var restoreRequest = new RestoreRequest("*", new List<RestoreFile>
                {
                    new RestoreFile(fileName1,
                        "F1_restore.txt".ToDevNullUri())
                }, true);

                var restoreJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(restoreRequest);

                Assert.AreEqual(JobStatusEnum.ACTIVE, restoreJob.Status.Status);

                pollingAttempts = 0;
                do
                {
                    restoreJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                        new GetJobRequest(restoreJob.JobId));
                    _log.Debug(restoreJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(PollingInterval));
                    pollingAttempts++;
                } while (restoreJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttempts < MaxPollingAttempts);

                Assert.Less(pollingAttempts, MaxPollingAttempts);
                Assert.AreEqual(JobStatusEnum.COMPLETED, restoreJob.Status.Status);

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                    new GetJobRequest(restoreJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Restore job completed successfully", job.Status.Message);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
                deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName2, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
            }
        }

        [Test]
        public void RetryArchiveCanceledJob()
        {
            var fileName1 = "RetryArchiveCanceledJob_" + Guid.NewGuid();

            try
            {
                var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, "F1.txt".ToAtoZUri(), 14,
                        new Dictionary<string, string> {{"fileName", fileName1}}, false)
                });

                var archiveJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Archive(archiveRequest);

                var cancelRequest = new CancelRequest(archiveJob.JobId);
                var cancel = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Cancel(cancelRequest);

                Assert.AreEqual(JobStatusEnum.CANCELED, cancel.Status.Status);
                Assert.AreEqual("Canceled", cancel.Status.Message);

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                    new GetJobRequest(archiveJob.JobId));

                Assert.AreEqual(JobStatusEnum.CANCELED, job.Status.Status);
                Assert.AreEqual("Canceled", job.Status.Message);

                //TODO can be tested after ESCP-1023 is fixed
                //foreach (var file in job.Files)
                //{
                //    Assert.AreEqual("Canceled", file.Status);
                //}

                var retryRequest = new RetryRequest(SpectraRioBrokerClientFixture.BrokerName, archiveJob.JobId,
                    JobType.ARCHIVE);
                var retryJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Retry(retryRequest);

                Assert.AreNotEqual(retryJob.JobId, archiveJob.JobId);

                var pollingAttempts = 0;
                do
                {
                    retryJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                        new GetJobRequest(retryJob.JobId));
                    _log.Debug(retryJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(PollingInterval));
                    pollingAttempts++;
                } while (retryJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttempts < MaxPollingAttempts);

                Assert.Less(pollingAttempts, MaxPollingAttempts);
                Assert.AreEqual(JobStatusEnum.COMPLETED, retryJob.Status.Status);

                job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(retryJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Archive job completed successfully", job.Status.Message);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
            }
        }

        [Test]
        public void RetryRestoreCanceledJob()
        {
            var fileName1 = "RetryRestoreCanceledJob_" + Guid.NewGuid();

            try
            {
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

                var restoreRequest = new RestoreRequest(SpectraRioBrokerClientFixture.BrokerName, new List<RestoreFile>
                {
                    new RestoreFile(fileName1,
                        "F1_restore.txt".ToDevNullUri())
                });

                var restoreJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(restoreRequest);

                Assert.AreEqual(JobStatusEnum.ACTIVE, restoreJob.Status.Status);

                var cancelRequest = new CancelRequest(restoreJob.JobId);
                var cancel = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Cancel(cancelRequest);

                Assert.AreEqual(JobStatusEnum.CANCELED, cancel.Status.Status);
                Assert.AreEqual("Canceled", cancel.Status.Message);

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                    new GetJobRequest(restoreJob.JobId));

                Assert.AreEqual(JobStatusEnum.CANCELED, job.Status.Status);
                Assert.AreEqual("Canceled", job.Status.Message);

                //TODO can be tested after ESCP-1023 is fixed
                //foreach (var file in job.Files)
                //{
                //    Assert.AreEqual("Canceled", file.Status);
                //}

                var retryRequest = new RetryRequest(SpectraRioBrokerClientFixture.BrokerName, restoreJob.JobId,
                    JobType.RESTORE);
                var retryJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Retry(retryRequest);

                Assert.AreNotEqual(retryJob.JobId, restoreJob.JobId);

                pollingAttempts = 0;
                do
                {
                    retryJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                        new GetJobRequest(retryJob.JobId));
                    _log.Debug(retryJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(PollingInterval));
                    pollingAttempts++;
                } while (retryJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttempts < MaxPollingAttempts);

                Assert.Less(pollingAttempts, MaxPollingAttempts);
                Assert.AreEqual(JobStatusEnum.COMPLETED, retryJob.Status.Status);

                job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(retryJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Restore job completed successfully", job.Status.Message);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
            }
        }

        [Test]
        public void SearchAndDeleteTest()
        {
            var fileName1 = "SearchAndDeleteTest_" + Guid.NewGuid();

            try
            {
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
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest("*", fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
            }
        }

        [Test]
        public void UpdateClientTokenTest()
        {
            var spectraRioBrokerClientBuilder = new SpectraRioBrokerClientBuilder(
                ConfigurationManager.AppSettings["ServerName"],
                int.Parse(ConfigurationManager.AppSettings["ServerPort"]));

            var proxy = ConfigurationManager.AppSettings["Proxy"];
            if (!string.IsNullOrWhiteSpace(proxy))
            {
                spectraRioBrokerClientBuilder.WithProxy(proxy);
            }

            var spectraRioBrokerClient = spectraRioBrokerClientBuilder.DisableSslValidation().Build();

            var fileName1 = "UpdateClientTokenTest" + Guid.NewGuid();

            SpectraRioBrokerClientFixture.SetupTestData();

            var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, new List<ArchiveFile>
            {
                new ArchiveFile(fileName1, "F1.txt".ToAtoZUri(), 14)
            });

            try
            {
                spectraRioBrokerClient.Archive(archiveRequest);
            }
            catch (MissingAuthorizationHeaderException)
            {
                var token = spectraRioBrokerClient.CreateToken(new CreateTokenRequest(
                        ConfigurationManager.AppSettings["RioUsername"],
                        ConfigurationManager.AppSettings["RioPassword"]))
                    .Token;
                spectraRioBrokerClient.UpdateToken(token);

                var archiveJob = spectraRioBrokerClient.Archive(archiveRequest);

                var pollingAttempts = 0;
                do
                {
                    archiveJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                        new GetJobRequest(archiveJob.JobId));
                    _log.Debug(archiveJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(PollingInterval));
                    pollingAttempts++;
                } while (archiveJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttempts < MaxPollingAttempts * 4);

                Assert.Less(pollingAttempts, MaxPollingAttempts * 4);
                Assert.AreEqual(JobStatusEnum.COMPLETED, archiveJob.Status.Status);
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
            }
        }

        [Test]
        public void GetDevicesTest()
        {
            var devices = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetSpectraDevices(new GetSpectraDevicesRequest(0, 1));
            Assert.AreEqual(0, devices.Page.Number);
            Assert.AreEqual(1, devices.Page.PageSize);
            Assert.AreEqual(1, devices.Page.TotalPages);
            
            Assert.AreEqual(1, devices.DeviceList.Count);
        }

        [Test]
        public void GetJobFilesStatusTest()
        {
            var fileName1 = "GetJobFilesStatusTest_a_" + Guid.NewGuid();
            var fileName2 = "GetJobFilesStatusTest_b_" + Guid.NewGuid();

            try
            {
                var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, "F1.txt".ToAtoZUri(), 14,
                        new Dictionary<string, string> {{"fileName", fileName1}}),
                    new ArchiveFile(fileName2, "F2.txt".ToAtoZUri(), 14,
                        new Dictionary<string, string> {{"fileName", fileName2}})
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

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                    new GetJobRequest(archiveJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Archive job completed successfully", job.Status.Message);
                Assert.AreEqual(2, job.FilesTransferred);
                Assert.AreEqual(1, job.Progress);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }

                var fileStatusList =
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJobFilesStatus(
                        new GetJobFilesStatusRequest(job.JobId));
                
                Assert.AreEqual(6, fileStatusList.FilesStatusList.Count);
                Assert.AreEqual(fileName1, fileStatusList.FilesStatusList[0].Name);
                Assert.AreEqual("Initializing", fileStatusList.FilesStatusList[0].Status);
                Assert.AreEqual(fileName1, fileStatusList.FilesStatusList[1].Name);
                Assert.AreEqual("Transferring", fileStatusList.FilesStatusList[1].Status);
                Assert.AreEqual(fileName1, fileStatusList.FilesStatusList[2].Name);
                Assert.AreEqual("Completed", fileStatusList.FilesStatusList[2].Status);
                Assert.AreEqual(fileName2, fileStatusList.FilesStatusList[3].Name);
                Assert.AreEqual("Initializing", fileStatusList.FilesStatusList[3].Status);
                Assert.AreEqual(fileName2, fileStatusList.FilesStatusList[4].Name);
                Assert.AreEqual("Transferring", fileStatusList.FilesStatusList[4].Status);
                Assert.AreEqual(fileName2, fileStatusList.FilesStatusList[5].Name);
                Assert.AreEqual("Completed", fileStatusList.FilesStatusList[5].Status);
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                var deleteF2Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName2);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF2Request);
            }
        }
        
        [Test]
        public void GetJobFileStatusTest()
        {
            var fileName1 = "GetJobFilesStatusTest_a_" + Guid.NewGuid();
            var fileName2 = "GetJobFilesStatusTest_b_" + Guid.NewGuid();

            try
            {
                var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, "F1.txt".ToAtoZUri(), 14,
                        new Dictionary<string, string> {{"fileName", fileName1}}),
                    new ArchiveFile(fileName2, "F2.txt".ToAtoZUri(), 14,
                        new Dictionary<string, string> {{"fileName", fileName2}})
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

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                    new GetJobRequest(archiveJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Archive job completed successfully", job.Status.Message);
                Assert.AreEqual(2, job.FilesTransferred);
                Assert.AreEqual(1, job.Progress);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }

                var fileStatusList =
                    SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJobFileStatuses(
                        new GetJobFileStatusesRequest(job.JobId, fileName2));
                
                Assert.AreEqual(3, fileStatusList.FileStatusesList.Count);
                Assert.AreEqual(fileName2, fileStatusList.FileStatusesList[0].Name);
                Assert.AreEqual("Initializing", fileStatusList.FileStatusesList[0].Status);
                Assert.AreEqual(fileName2, fileStatusList.FileStatusesList[1].Name);
                Assert.AreEqual("Transferring", fileStatusList.FileStatusesList[1].Status);
                Assert.AreEqual(fileName2, fileStatusList.FileStatusesList[2].Name);
                Assert.AreEqual("Completed", fileStatusList.FileStatusesList[2].Status);
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                var deleteF2Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName2);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF2Request);
            }
        }
    }
}