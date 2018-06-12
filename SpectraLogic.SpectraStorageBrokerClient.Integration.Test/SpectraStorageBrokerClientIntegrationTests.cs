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
using SpectraLogic.SpectraStorageBrokerClient.Calls;
using SpectraLogic.SpectraStorageBrokerClient.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SpectraLogic.SpectraStorageBrokerClient.Integration.Test
{
    [TestFixture]
    public class SpectraStorageBrokerClientIntegrationTests
    {
        #region Private Fields

        //TODO remove this once Job tracking is done in the server
        private readonly int MAX_POLLING_ATTEMPS = 10;

        private readonly int POLLING_INTERVAL = 10;
        private ILog _log = LogManager.GetLogger("SpectraStorageBrokerClientIntegrationTest");

        #endregion Private Fields

        // in sec

        #region Public Methods

        [Test]
        public void ArchiveAndRestore()
        {
            try
            {
                SpectraStorageBrokerClientFixture.SetupTestData();

                /***********
                 * ARCHIVE *
                 ***********/
                var fileName1 = Guid.NewGuid().ToString();
                var fileName2 = Guid.NewGuid().ToString();
                var archiveRequest = new ArchiveRequest(SpectraStorageBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, $"{SpectraStorageBrokerClientFixture.ArchiveTempDir}/F1.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName1 } }, false, false),
                    new ArchiveFile(fileName2, $"{SpectraStorageBrokerClientFixture.ArchiveTempDir}/F2.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName2 } }, false, false)
                });

                var archiveJob = SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Archive(archiveRequest);

                var pollingAttemps = 0;
                do
                {
                    archiveJob = SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.GetJob(
                        new GetJobRequest(archiveJob.JobId));
                    _log.Debug(archiveJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(POLLING_INTERVAL));
                    pollingAttemps++;
                } while (archiveJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttemps < MAX_POLLING_ATTEMPS);

                Assert.Less(pollingAttemps, MAX_POLLING_ATTEMPS);
                Assert.AreEqual(JobStatusEnum.COMPLETED, archiveJob.Status.Status);

                /**********
                * RESTORE *
                ***********/

                var restoreRequest = new RestoreRequest(SpectraStorageBrokerClientFixture.BrokerName, new List<RestoreFile>
                {
                    new RestoreFile(fileName1, $"{SpectraStorageBrokerClientFixture.RestoreTempDir}/F1_restore.txt".ToFileUri()),
                    new RestoreFile(fileName2, $"{SpectraStorageBrokerClientFixture.RestoreTempDir}/F2_restore.txt".ToFileUri())
                });

                var restoreJob = SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Restore(restoreRequest);

                pollingAttemps = 0;
                do
                {
                    restoreJob = SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.GetJob(
                        new GetJobRequest(restoreJob.JobId));
                    _log.Debug(restoreJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(POLLING_INTERVAL));
                    pollingAttemps++;
                } while (restoreJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttemps < MAX_POLLING_ATTEMPS);

                Assert.Less(pollingAttemps, MAX_POLLING_ATTEMPS);
                Assert.AreEqual(JobStatusEnum.COMPLETED, restoreJob.Status.Status);
            }
            finally
            {
                Directory.Delete(SpectraStorageBrokerClientFixture.ArchiveTempDir, true);
                Directory.Delete(SpectraStorageBrokerClientFixture.RestoreTempDir, true);
            }
        }

        [Test]
        public void CancelArchiveJob()
        {
            try
            {
                SpectraStorageBrokerClientFixture.SetupTestData();
                Directory.Delete(SpectraStorageBrokerClientFixture.ArchiveTempDir, true);
                var fileName1 = Guid.NewGuid().ToString();
                var archiveRequest = new ArchiveRequest(SpectraStorageBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, $"{SpectraStorageBrokerClientFixture.ArchiveTempDir}/F1.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName1 } }, false, false),
                });

                var archiveJob = SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Archive(archiveRequest);

                Assert.AreEqual(JobStatusEnum.ACTIVE, archiveJob.Status.Status);

                var cancelRequest = new CancelRequest(archiveJob.JobId);
                var cancel = SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Cancel(cancelRequest);

                Assert.AreEqual(JobStatusEnum.CANCELED, cancel.Status.Status);
                Assert.AreEqual("Cancelled", cancel.Status.Message);
            }
            finally
            {
                Directory.Delete(SpectraStorageBrokerClientFixture.ArchiveTempDir, true);
                Directory.Delete(SpectraStorageBrokerClientFixture.RestoreTempDir, true);
            }
        }

        [Test]
        public void CancelRestoreJob()
        {
            try
            {
                SpectraStorageBrokerClientFixture.SetupTestData();

                var fileName1 = Guid.NewGuid().ToString();
                var archiveRequest = new ArchiveRequest(SpectraStorageBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, $"{SpectraStorageBrokerClientFixture.ArchiveTempDir}/F1.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName1 } }, false, false),
                });

                var archiveJob = SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Archive(archiveRequest);

                var pollingAttemps = 0;
                do
                {
                    archiveJob = SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.GetJob(
                        new GetJobRequest(archiveJob.JobId));
                    _log.Debug(archiveJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(POLLING_INTERVAL));
                    pollingAttemps++;
                } while (archiveJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttemps < MAX_POLLING_ATTEMPS);

                Assert.Less(pollingAttemps, MAX_POLLING_ATTEMPS);
                Assert.AreEqual(JobStatusEnum.COMPLETED, archiveJob.Status.Status);

                var restoreRequest = new RestoreRequest(SpectraStorageBrokerClientFixture.BrokerName, new List<RestoreFile>
                {
                    new RestoreFile(fileName1, $"{SpectraStorageBrokerClientFixture.RestoreTempDir}/F1_restore.txt".ToFileUri()),
                });

                var restoreJob = SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Restore(restoreRequest);

                Assert.AreEqual(JobStatusEnum.ACTIVE, restoreJob.Status.Status);

                var cancelRequest = new CancelRequest(restoreJob.JobId);
                var cancel = SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Cancel(cancelRequest);

                Assert.AreEqual(JobStatusEnum.CANCELED, cancel.Status.Status);
                Assert.AreEqual("Cancelled", cancel.Status.Message);
            }
            finally
            {
                Directory.Delete(SpectraStorageBrokerClientFixture.ArchiveTempDir, true);
                Directory.Delete(SpectraStorageBrokerClientFixture.RestoreTempDir, true);
            }
        }

        [Test, Ignore("Cancel Archive job is not yet supported by the server ; Retry is not yet implemented in the server")]
        public void RetryArchiveCancelledJob()
        {
        }

        [Test, Ignore("Retry is not yet implemented in the server")]
        public void RetryRestoreCancelledJob()
        {
            try
            {
                SpectraStorageBrokerClientFixture.SetupTestData();

                var fileName1 = Guid.NewGuid().ToString();
                var archiveRequest = new ArchiveRequest(SpectraStorageBrokerClientFixture.BrokerName, new List<ArchiveFile>
                {
                    new ArchiveFile(fileName1, $"{SpectraStorageBrokerClientFixture.ArchiveTempDir}/F1.txt".ToFileUri(), 14, new Dictionary<string, string>{ { "fileName", fileName1 } }, false, false),
                });

                var archiveJob = SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Archive(archiveRequest);

                var pollingAttemps = 0;
                do
                {
                    archiveJob = SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.GetJob(
                        new GetJobRequest(archiveJob.JobId));
                    _log.Debug(archiveJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(POLLING_INTERVAL));
                    pollingAttemps++;
                } while (archiveJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttemps < MAX_POLLING_ATTEMPS);

                Assert.Less(pollingAttemps, MAX_POLLING_ATTEMPS);
                Assert.AreEqual(JobStatusEnum.COMPLETED, archiveJob.Status.Status);

                var restoreRequest = new RestoreRequest(SpectraStorageBrokerClientFixture.BrokerName, new List<RestoreFile>
                {
                    new RestoreFile(fileName1, $"{SpectraStorageBrokerClientFixture.RestoreTempDir}/F1_restore.txt".ToFileUri()),
                });

                var restoreJob = SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Restore(restoreRequest);

                Assert.AreEqual(JobStatusEnum.ACTIVE, restoreJob.Status.Status);

                var cancelRequest = new CancelRequest(restoreJob.JobId);
                var cancel = SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Cancel(cancelRequest);

                Assert.AreEqual(JobStatusEnum.CANCELED, cancel.Status.Status);
                Assert.AreEqual("Cancelled", cancel.Status.Message);

                var retryRequest = new RetryRequest(restoreJob.JobId);
                var retryJob = SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.Retry(retryRequest);

                Assert.AreNotEqual(retryJob.JobId, restoreJob.JobId);

                pollingAttemps = 0;
                do
                {
                    retryJob = SpectraStorageBrokerClientFixture.SpectraStorageBrokerClient.GetJob(
                        new GetJobRequest(retryJob.JobId));
                    _log.Debug(retryJob.Status);
                    Thread.Sleep(TimeSpan.FromSeconds(POLLING_INTERVAL));
                    pollingAttemps++;
                } while (retryJob.Status.Status == JobStatusEnum.ACTIVE && pollingAttemps < MAX_POLLING_ATTEMPS);

                Assert.Less(pollingAttemps, MAX_POLLING_ATTEMPS);
                Assert.AreEqual(JobStatusEnum.COMPLETED, retryJob.Status.Status); 

            }
            finally
            {
                Directory.Delete(SpectraStorageBrokerClientFixture.ArchiveTempDir, true);
                Directory.Delete(SpectraStorageBrokerClientFixture.RestoreTempDir, true);
            }
        }

        #endregion Public Methods
    }
}