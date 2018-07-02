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
using System.Threading;

namespace SpectraLogic.SpectraRioBrokerClient.Integration.Test
{
    [TestFixture]
    public class SpectraRioBrokerClientIntegrationTests
    {
        #region Private Fields

        //TODO remove this once Job tracking is done in the server
        private readonly int MAX_POLLING_ATTEMPS = 10;

        private readonly int POLLING_INTERVAL = 10;
        private ILog _log = LogManager.GetLogger("SpectraRioBrokerClientIntegrationTest");

        #endregion Private Fields

        // in sec

        #region Public Methods

        [Test]
        public void ArchiveAndRestore()
        {
            try
            {
                SpectraRioBrokerClientFixture.SetupTestData();

                /***********
                 * ARCHIVE *
                 ***********/
                var fileName1 = Guid.NewGuid().ToString();
                var fileName2 = Guid.NewGuid().ToString();
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
                    new RestoreFile(fileName2, $"{SpectraRioBrokerClientFixture.RestoreTempDir}/F2_restore.txt".ToFileUri())
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
                foreach (var file in job.Files)
                {
                    Assert.AreEqual("Completed", file.Status);
                }

                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                var deleteF2Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName2);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF2Request);
            }
            finally
            {
                Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
                Directory.Delete(SpectraRioBrokerClientFixture.RestoreTempDir, true);
            }
        }

        [Test, Ignore("https://jira.spectralogic.com/browse/ESCP-371")]
        public void ArchiveNewFilesOnlyTest_1()
        {
            SpectraRioBrokerClientFixture.SetupTestData();

            var fileName1 = Guid.NewGuid().ToString();
            var fileName2 = Guid.NewGuid().ToString();

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
                });
        }

        [Test]
        public void ArchiveNewFilesOnlyTest_2()
        {
            SpectraRioBrokerClientFixture.SetupTestData();

            var fileName1 = Guid.NewGuid().ToString();
            var fileName2 = Guid.NewGuid().ToString();

            ArchiveNewFilesOnlyTest(
                new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F1.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName1 } }, false, false),
                },
                new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F1.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName1 } }, false, false),
                    new ArchiveFile(fileName2, $"{SpectraRioBrokerClientFixture.ArchiveTempDir}/F2.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName2 } }, false, false)
                });
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
        public void RetryArchiveCanceledJob()
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

                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
            }
            finally
            {
                Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
                Directory.Delete(SpectraRioBrokerClientFixture.RestoreTempDir, true);
            }
        }

        [Test, Ignore("Retry is not yet implemented in the server")]
        public void RetryRestoreCanceledJob()
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

                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
            }
            finally
            {
                Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
                Directory.Delete(SpectraRioBrokerClientFixture.RestoreTempDir, true);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void ArchiveNewFilesOnlyTest(List<ArchiveFile> list1, List<ArchiveFile> list2)
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
                Assert.AreEqual("Archive job completed successfully", job.Status.Message);
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

        [Test]
        public void SearchAndDeleteTest()
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


                var deleteF1Request = new DeleteFileRequest("*", fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
            }
            finally
            {
                Directory.Delete(SpectraRioBrokerClientFixture.ArchiveTempDir, true);
            }
        }


    }
}