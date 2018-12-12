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
using SpectraLogic.SpectraRioBrokerClient.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace SpectraLogic.SpectraRioBrokerClient.Integration.Test
{
    [TestFixture]
    public class SpectraRioBrokerClientIntegrationTests
    {
        #region Private Fields

        //TODO remove this once Job tracking is done in the server
        private readonly int MAX_POLLING_ATTEMPS = 10;

        private readonly int POLLING_INTERVAL = 10;  // in sec
        private ILog _log = LogManager.GetLogger("SpectraRioBrokerClientIntegrationTest");

        #endregion Private Fields

        #region Public Methods

        [Test, Ignore("ESCP-750 - Successful Restore job has 'bytesTransferred':0")]
        public void ArchiveAndRestore()
        {
            var fileName1 = Guid.NewGuid().ToString();
            var fileName2 = Guid.NewGuid().ToString();

            try
            {
                SpectraRioBrokerClientFixture.SetupTestData();

                /***********
                 * ARCHIVE *
                 ***********/
                var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F1.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName1 } }, false, false),
                    new ArchiveFile(fileName2, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F2.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName2 } }, false, false)
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

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(archiveJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Archive job completed successfully", job.Status.Message);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }

                /**********
                * RESTORE *
                ***********/

                var restoreRequest = new RestoreRequest(SpectraRioBrokerClientFixture.BrokerName, new List<RestoreFile>
                {
                    new RestoreFile(fileName1, $"{SpectraRioBrokerClientFixture.RestoreTempDir}/F1_restore.txt".ToFileUri()),
                    new RestoreFile(fileName2, $"{SpectraRioBrokerClientFixture.RestoreTempDir}/F2_restore.txt".ToFileUri(), new ByteRange(0, 10))
                });

                var restoreJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(restoreRequest);

                pollingAttemps = 0;
                do
                {
                    restoreJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                        new GetJobRequest(restoreJob.JobId));
                    _log.Debug(restoreJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(POLLING_INTERVAL));
                    pollingAttemps++;
                } while (restoreJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttemps < MAX_POLLING_ATTEMPS);

                Assert.Less(pollingAttemps, MAX_POLLING_ATTEMPS);
                Assert.AreEqual(JobStatusEnum.COMPLETED, restoreJob.Status.Status);

                job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(restoreJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Restore job completed successfully", job.Status.Message);
                Assert.AreEqual(14 + 11, job.BytesTransferred);
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

                Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
                Directory.Delete(SpectraRioBrokerClientFixture.RestoreTempDir, true);
            }
        }

        [Test, Ignore("ESCP-750 - Successful Restore job has 'bytesTransferred':0")]
        public void ArchiveAndRestoreWithRelationship()
        {
            var fileName1 = Guid.NewGuid().ToString();
            var fileName2 = Guid.NewGuid().ToString();

            try
            {
                SpectraRioBrokerClientFixture.SetupTestData();

                /***********
                 * ARCHIVE *
                 ***********/
                var relationshipName = "ep_net_test";
                var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F1.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName1 } }, false, false, new HashSet<string>(){relationshipName}),
                    new ArchiveFile(fileName2, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F2.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName2 } }, false, false, new HashSet<string>(){relationshipName})
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

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(archiveJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Archive job completed successfully", job.Status.Message);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }

                var relationshipRequest = new GetBrokerRelationshipRequest(SpectraRioBrokerClientFixture.BrokerName, relationshipName);
                var relationship = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerRelationship(relationshipRequest);

                Assert.AreEqual(2, relationship.Objects.Count());
                foreach (var obj in relationship.Objects)
                {
                    Assert.AreEqual(1, obj.Relationships.Count);
                    Assert.AreEqual(relationshipName, obj.Relationships.First());
                }

                /**********
                * RESTORE *
                ***********/

                var restoreList = relationship.Objects.Select(obj =>
                {
                    return new RestoreFile(obj.Name, $"{SpectraRioBrokerClientFixture.RestoreTempDir}/{obj.Name}".ToFileUri());
                });

                var restoreRequest = new RestoreRequest(SpectraRioBrokerClientFixture.BrokerName, restoreList);
                var restoreJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(restoreRequest);

                pollingAttemps = 0;
                do
                {
                    restoreJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                        new GetJobRequest(restoreJob.JobId));
                    _log.Debug(restoreJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(POLLING_INTERVAL));
                    pollingAttemps++;
                } while (restoreJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttemps < MAX_POLLING_ATTEMPS);

                Assert.Less(pollingAttemps, MAX_POLLING_ATTEMPS);
                Assert.AreEqual(JobStatusEnum.COMPLETED, restoreJob.Status.Status);

                job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(restoreJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Restore job completed successfully", job.Status.Message);
                Assert.AreEqual(28, job.BytesTransferred);
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

                Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
                Directory.Delete(SpectraRioBrokerClientFixture.RestoreTempDir, true);
            }
        }

        [Test]
        public void ArchiveNewFilesOnlyTest_1()
        {
            var fileName1 = Guid.NewGuid().ToString();
            var fileName2 = Guid.NewGuid().ToString();

            try
            {
                SpectraRioBrokerClientFixture.SetupTestData();

                ArchiveNewFilesOnlyTest(
                new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F1.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName1 } }, false, false),
                    new ArchiveFile(fileName2, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F2.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName2 } }, false, false)
                },
                new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F1.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName1 } }, false, false),
                    new ArchiveFile(fileName2, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F2.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName2 } }, false, false)
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
            var fileName1 = Guid.NewGuid().ToString();
            var fileName2 = Guid.NewGuid().ToString();

            try
            {
                SpectraRioBrokerClientFixture.SetupTestData();

                ArchiveNewFilesOnlyTest(
                new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F1.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName1 } }, false, false),
                },
                new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F1.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName1 } }, false, false),
                    new ArchiveFile(fileName2, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F2.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName2 } }, false, false)
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

        [Test, Ignore("Not working yet")]
        public void CancelArchiveJob()
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

                Assert.AreEqual(JobStatusEnum.ACTIVE, archiveJob.Status.Status);

                var cancelRequest = new CancelRequest(archiveJob.JobId);
                var cancel = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Cancel(cancelRequest);

                Assert.AreEqual(JobStatusEnum.CANCELED, cancel.Status.Status);
                Assert.AreEqual("Canceled", cancel.Status.Message);

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(archiveJob.JobId));

                Assert.AreEqual(JobStatusEnum.CANCELED, job.Status.Status);
                Assert.AreEqual("Canceled", job.Status.Message);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Canceled", file.Status);
                }
            }
            finally
            {
                Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
                Directory.Delete(SpectraRioBrokerClientFixture.RestoreTempDir, true);
            }
        }

        [Test, Ignore("Not working yet")]
        public void CancelRestoreJob()
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

                var restoreRequest = new RestoreRequest(SpectraRioBrokerClientFixture.BrokerName, new List<RestoreFile>
                {
                    new RestoreFile(fileName1, $"{SpectraRioBrokerClientFixture.RestoreTempDir}/F1_restore.txt".ToFileUri()),
                });

                var restoreJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(restoreRequest);

                Assert.AreEqual(JobStatusEnum.ACTIVE, restoreJob.Status.Status);

                var cancelRequest = new CancelRequest(restoreJob.JobId);
                var cancel = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Cancel(cancelRequest);

                Assert.AreEqual(JobStatusEnum.CANCELED, cancel.Status.Status);
                Assert.AreEqual("Canceled", cancel.Status.Message);

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(restoreJob.JobId));

                Assert.AreEqual(JobStatusEnum.CANCELED, job.Status.Status);
                Assert.AreEqual("Canceled", job.Status.Message);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Canceled", file.Status);
                }
            }
            finally
            {
                Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
                Directory.Delete(SpectraRioBrokerClientFixture.RestoreTempDir, true);
            }
        }

        [Test]
        public void DoesBrokerObjectExistTest()
        {
            var fileName1 = Guid.NewGuid().ToString();

            try
            {
                SpectraRioBrokerClientFixture.SetupTestData();

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

                Assert.IsTrue(SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DoesBrokerObjectExist(SpectraRioBrokerClientFixture.BrokerName, fileName1));
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);

                Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
            }
        }

        [Test]
        public void GetBrokerObjectsTest()
        {
            var fileName1 = "a" + Guid.NewGuid().ToString();
            var fileName2 = "b" + Guid.NewGuid().ToString();

            try
            {
                SpectraRioBrokerClientFixture.SetupTestData();

                /***********
                 * ARCHIVE *
                 ***********/
                var relationshipName = "ep_net_test";
                var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F1.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName1 } }, false, false, new HashSet<string>(){relationshipName}),
                    new ArchiveFile(fileName2, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F2.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName2 } }, false, false, new HashSet<string>(){relationshipName})
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

                var brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName));

                Assert.AreEqual(2, brokerObjects.Objects.Count());

                brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName, sortBy: SortBy.Name, sortOrder: SortOrder.Asc));

                Assert.AreEqual(2, brokerObjects.Objects.Count());
                Assert.AreEqual(fileName1, brokerObjects.Objects.ElementAt(0).Name);

                brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName, sortBy: SortBy.Name, sortOrder: SortOrder.Desc));

                Assert.AreEqual(2, brokerObjects.Objects.Count());
                Assert.AreEqual(fileName2, brokerObjects.Objects.ElementAt(0).Name);

                brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName, perPage: 1));

                Assert.AreEqual(1, brokerObjects.Objects.Count());
                Assert.AreEqual(fileName1, brokerObjects.Objects.ElementAt(0).Name);
                Assert.AreEqual(2, brokerObjects.Page.TotalPages);
                Assert.AreEqual(1, brokerObjects.Page.PageSize);
                Assert.AreEqual(0, brokerObjects.Page.Number);

                brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName, filename: fileName2));

                Assert.AreEqual(1, brokerObjects.Objects.Count());
                Assert.AreEqual(fileName2, brokerObjects.Objects.ElementAt(0).Name);

                brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName, prefix: "a"));

                Assert.AreEqual(1, brokerObjects.Objects.Count());
                Assert.AreEqual(fileName1, brokerObjects.Objects.ElementAt(0).Name);

                brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName, prefix: "a", filename: "not_found"));

                Assert.AreEqual(0, brokerObjects.Objects.Count());

                brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName, metadata: new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("fileName", fileName2)
                    }));

                Assert.AreEqual(1, brokerObjects.Objects.Count());
                Assert.AreEqual(fileName2, brokerObjects.Objects.ElementAt(0).Name);

                brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName, relationships: new List<string>()
                    {
                        "ep_net_test"
                    }));

                Assert.AreEqual(2, brokerObjects.Objects.Count());
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                var deleteF2Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName2);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF2Request);

                Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
            }
        }

        [Test]
        public void GetBrokerObjectTest()
        {
            var fileName1 = Guid.NewGuid().ToString();

            try
            {
                SpectraRioBrokerClientFixture.SetupTestData();

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

                var getBrokerObjectRequest = new GetBrokerObjectRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                var brokerObject = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObject(getBrokerObjectRequest);

                Assert.AreEqual(SpectraRioBrokerClientFixture.BrokerName, brokerObject.Broker);
                Assert.AreEqual(fileName1, brokerObject.Name);
                Assert.AreEqual(14, brokerObject.Size);
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);

                Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
            }
        }

        [Test]
        public void GetSystemTest()
        {
            Assert.DoesNotThrow(() => SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetSystem(new GetSystemRequest()));
        }

        [Test]
        public void RestoreJobWithIgnoreDuplicates()
        {
            var fileName1 = Guid.NewGuid().ToString();

            try
            {
                SpectraRioBrokerClientFixture.SetupTestData();

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

                //Using search and restore
                var restoreRequest = new RestoreRequest("*", new List<RestoreFile>
                {
                    new RestoreFile(fileName1, $"{SpectraRioBrokerClientFixture.RestoreTempDir}/F1_restore.txt".ToFileUri()),
                }, true);

                var restoreJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(restoreRequest);

                Assert.AreEqual(JobStatusEnum.ACTIVE, restoreJob.Status.Status);

                pollingAttemps = 0;
                do
                {
                    restoreJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                        new GetJobRequest(restoreJob.JobId));
                    _log.Debug(restoreJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(POLLING_INTERVAL));
                    pollingAttemps++;
                } while (restoreJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttemps < MAX_POLLING_ATTEMPS);

                Assert.Less(pollingAttemps, MAX_POLLING_ATTEMPS);
                Assert.AreEqual(JobStatusEnum.COMPLETED, restoreJob.Status.Status);

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(restoreJob.JobId));

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

                Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
                Directory.Delete(SpectraRioBrokerClientFixture.RestoreTempDir, true);
            }
        }

        [Test, Ignore("Retry is not yet implemented in the server")]
        public void RetryArchiveCanceledJob()
        {
            var fileName1 = Guid.NewGuid().ToString();

            try
            {
                SpectraRioBrokerClientFixture.SetupTestData();

                var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F1.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName1 } }, false, false),
                });

                var archiveJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Archive(archiveRequest);

                var cancelRequest = new CancelRequest(archiveJob.JobId);
                var cancel = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Cancel(cancelRequest);

                Assert.AreEqual(JobStatusEnum.CANCELED, cancel.Status.Status);
                Assert.AreEqual("Canceled", cancel.Status.Message);

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(archiveJob.JobId));

                Assert.AreEqual(JobStatusEnum.CANCELED, job.Status.Status);
                Assert.AreEqual("Canceled", job.Status.Message);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Canceled", file.Status);
                }

                var retryRequest = new RetryRequest(archiveJob.JobId);
                var retryJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Retry(retryRequest);

                Assert.AreNotEqual(retryJob.JobId, archiveJob.JobId);

                var pollingAttemps = 0;
                do
                {
                    retryJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                        new GetJobRequest(retryJob.JobId));
                    _log.Debug(retryJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(POLLING_INTERVAL));
                    pollingAttemps++;
                } while (retryJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttemps < MAX_POLLING_ATTEMPS);

                Assert.Less(pollingAttemps, MAX_POLLING_ATTEMPS);
                Assert.AreEqual(JobStatusEnum.COMPLETED, retryJob.Status.Status);

                job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(retryJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Compleated", job.Status.Message);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Archive job completed successfully", file.Status);
                }
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);

                Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
                Directory.Delete(SpectraRioBrokerClientFixture.RestoreTempDir, true);
            }
        }

        [Test, Ignore("Retry is not yet implemented in the server")]
        public void RetryRestoreCanceledJob()
        {
            var fileName1 = Guid.NewGuid().ToString();

            try
            {
                SpectraRioBrokerClientFixture.SetupTestData();

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

                var restoreRequest = new RestoreRequest(SpectraRioBrokerClientFixture.BrokerName, new List<RestoreFile>
                {
                    new RestoreFile(fileName1, $"{SpectraRioBrokerClientFixture.RestoreTempDir}/F1_restore.txt".ToFileUri()),
                });

                var restoreJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Restore(restoreRequest);

                Assert.AreEqual(JobStatusEnum.ACTIVE, restoreJob.Status.Status);

                var cancelRequest = new CancelRequest(restoreJob.JobId);
                var cancel = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Cancel(cancelRequest);

                Assert.AreEqual(JobStatusEnum.CANCELED, cancel.Status.Status);
                Assert.AreEqual("Canceled", cancel.Status.Message);

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(restoreJob.JobId));

                Assert.AreEqual(JobStatusEnum.CANCELED, job.Status.Status);
                Assert.AreEqual("Canceled", job.Status.Message);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Canceled", file.Status);
                }

                var retryRequest = new RetryRequest(restoreJob.JobId);
                var retryJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.Retry(retryRequest);

                Assert.AreNotEqual(retryJob.JobId, restoreJob.JobId);

                pollingAttemps = 0;
                do
                {
                    retryJob = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(
                        new GetJobRequest(retryJob.JobId));
                    _log.Debug(retryJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(POLLING_INTERVAL));
                    pollingAttemps++;
                } while (retryJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttemps < MAX_POLLING_ATTEMPS);

                Assert.Less(pollingAttemps, MAX_POLLING_ATTEMPS);
                Assert.AreEqual(JobStatusEnum.COMPLETED, retryJob.Status.Status);

                job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(retryJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Compleated", job.Status.Message);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Restore job completed successfully", file.Status);
                }
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);

                Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
                Directory.Delete(SpectraRioBrokerClientFixture.RestoreTempDir, true);
            }
        }

        [Test]
        public void SearchAndDeleteTest()
        {
            var fileName1 = Guid.NewGuid().ToString();

            try
            {
                SpectraRioBrokerClientFixture.SetupTestData();

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
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest("*", fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);

                Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void ArchiveNewFilesOnlyTest(List<ArchiveFile> list1, List<ArchiveFile> list2, string message)
        {
            try
            {
                var archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, list1);
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

                var job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(archiveJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual("Archive job completed successfully", job.Status.Message);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }

                archiveRequest = new ArchiveRequest(SpectraRioBrokerClientFixture.BrokerName, list2, true);
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

                job = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJob(new GetJobRequest(archiveJob.JobId));

                Assert.AreEqual(JobStatusEnum.COMPLETED, job.Status.Status);
                Assert.AreEqual(message, job.Status.Message);
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }
            }
            finally
            {
                Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
            }
        }

        #endregion Private Methods
    }
}