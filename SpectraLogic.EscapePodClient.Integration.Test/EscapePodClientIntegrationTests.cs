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
using SpectraLogic.EscapePodClient.Calls;
using SpectraLogic.EscapePodClient.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SpectraLogic.EscapePodClient.Integration.Test
{
    [TestFixture]
    public class EscapePodClientIntegrationTests
    {
        #region Fields

        private ILog _log = LogManager.GetLogger("EscapePodClientIntegrationTest");

        #endregion Fields

        #region Methods

        [Test]
        public void ArchiveAndRestore()
        {
            /***********
             * ARCHIVE *
             ***********/
            var fileName1 = Guid.NewGuid().ToString();
            var fileName2 = Guid.NewGuid().ToString();
            var archiveRequest = new ArchiveRequest(EscapePodClientFixture.ArchiveName, new List<ArchiveFile>
            {
                new ArchiveFile(fileName1, "file:///C:/Users/sharons/Documents/GitHub/ep_net_sdk/SpectraLogic.EscapePodClient.Integration.Test/TestFiles/F1.txt", 14, new Dictionary<string, string>{ { "fileName", fileName1 } }, false, false),
                new ArchiveFile(fileName2, "file:///C:/Users/sharons/Documents/GitHub/ep_net_sdk/SpectraLogic.EscapePodClient.Integration.Test/TestFiles/F2.txt", 14, new Dictionary<string, string>{ { "fileName", fileName2 } }, false, false)
            });

            var archiveJob = EscapePodClientFixture.EscapePodClient.Archive(archiveRequest);

            do
            {
                archiveJob = EscapePodClientFixture.EscapePodClient.GetJob(new GetEscapePodJobRequest(EscapePodClientFixture.ArchiveName, archiveJob.JobId));
                _log.Debug(archiveJob.Status);
                Thread.Sleep(5000);
            } while (archiveJob.Status.Status == JobStatus.ACTIVE);

            Assert.AreEqual(JobStatus.COMPLETED, archiveJob.Status.Status);

            /**********
            * RESTORE *
            ***********/

            //TODO create a temp dir for the restore
            var restoreRequest = new RestoreRequest(EscapePodClientFixture.ArchiveName, new List<RestoreFile>
            {
                new RestoreFile(fileName1, "file:///C:/Temp/restore/F1_restore.txt"),
                new RestoreFile(fileName2, "file:///C:/Temp/restore/F2_restore.txt")
            });

            var restoreJob = EscapePodClientFixture.EscapePodClient.Restore(restoreRequest);

            do
            {
                restoreJob = EscapePodClientFixture.EscapePodClient.GetJob(new GetEscapePodJobRequest(EscapePodClientFixture.ArchiveName, restoreJob.JobId));
                _log.Debug(restoreJob.Status);
                Thread.Sleep(5000);
            } while (restoreJob.Status.Status == JobStatus.ACTIVE);

            Assert.AreEqual(JobStatus.COMPLETED, restoreJob.Status.Status);
        }

        #endregion Methods
    }
}