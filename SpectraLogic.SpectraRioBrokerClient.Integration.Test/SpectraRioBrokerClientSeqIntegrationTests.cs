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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using log4net;
using NUnit.Framework;
using SpectraLogic.SpectraRioBrokerClient.Calls.Broker;
using SpectraLogic.SpectraRioBrokerClient.Calls.Jobs;
using SpectraLogic.SpectraRioBrokerClient.Model;

namespace SpectraLogic.SpectraRioBrokerClient.Integration.Test
{
    [TestFixture]
    public class SpectraRioBrokerClientSeqIntegrationTests
    {
        private const int MaxPollingAttempts = 10;
        private const int PollingInterval = 10; // in sec

        private readonly ILog _log = LogManager.GetLogger("SpectraRioBrokerClientSeqIntegrationTests");

        [Test]
        public void GetBrokerObjectsTest()
        {
            var fileName1 = "0a_" + "GetBrokerObjectsTest_" + Guid.NewGuid();
            var fileName2 = "zz_" + "GetBrokerObjectsTest_" + Guid.NewGuid();

            try
            {
                var brokerObjectsBefore = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName)).Objects.Count;

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

                var brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName));

                Assert.AreEqual(brokerObjectsBefore + 2, brokerObjects.Objects.Count);

                brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName,
                        ObjectsSortByEnum.NAME,
                        SortOrderEnum.ASC));

                Assert.AreEqual(brokerObjectsBefore + 2, brokerObjects.Objects.Count);
                Assert.AreEqual(fileName1, brokerObjects.Objects.ElementAt(0).Name);

                brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName,
                        ObjectsSortByEnum.NAME,
                        SortOrderEnum.DESC));

                Assert.AreEqual(brokerObjectsBefore + 2, brokerObjects.Objects.Count);
                Assert.AreEqual(fileName2, brokerObjects.Objects.ElementAt(0).Name);

                brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName, perPage: 1));

                Assert.AreEqual(1, brokerObjects.Objects.Count);
                Assert.AreEqual(fileName2, brokerObjects.Objects.ElementAt(0).Name);
                Assert.AreEqual(2, brokerObjects.Page.TotalPages);
                Assert.AreEqual(2, brokerObjects.Page.TotalItems);
                Assert.AreEqual(1, brokerObjects.Page.PageSize);
                Assert.AreEqual(0, brokerObjects.Page.Number);

                brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName, filename: fileName2));

                Assert.AreEqual(1, brokerObjects.Objects.Count);
                Assert.AreEqual(fileName2, brokerObjects.Objects.ElementAt(0).Name);

                brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName, prefix: "0a"));

                Assert.AreEqual(1, brokerObjects.Objects.Count);
                Assert.AreEqual(fileName1, brokerObjects.Objects.ElementAt(0).Name);

                brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName, prefix: "0a",
                        filename: "not_found"));

                Assert.AreEqual(0, brokerObjects.Objects.Count);

                brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName,
                        metadata: new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("fileName", fileName2)
                        }));

                Assert.AreEqual(1, brokerObjects.Objects.Count);
                Assert.AreEqual(fileName2, brokerObjects.Objects.ElementAt(0).Name);

                brokerObjects = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokerObjects(
                    new GetBrokerObjectsRequest(SpectraRioBrokerClientFixture.BrokerName));

                Assert.AreEqual(2, brokerObjects.Objects.Count);
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
        public void GetBrokersTest()
        {
            var brokers = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetBrokers(new GetBrokersRequest());
            Assert.AreEqual(2, brokers.BrokerList.Count);
        }

        [Test]
        public void GetJobsTest()
        {
            var fileName1 = Guid.NewGuid().ToString();

            try
            {
                var archiveJobsBefore = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJobs(
                    new GetJobsRequest(
                        jobTypes: new List<JobTypeEnum> {JobTypeEnum.ARCHIVE},
                        jobStatuses: new List<JobStatusEnum> {JobStatusEnum.COMPLETED})
                ).JobsList.Count;
                var restoreJobsBefore = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJobs(
                    new GetJobsRequest(
                        jobTypes: new List<JobTypeEnum> {JobTypeEnum.RESTORE})
                ).JobsList.Count;

                SpectraRioBrokerClientFixture.SetupTestData();
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

                var jobs = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJobs(
                    new GetJobsRequest(
                        jobTypes: new List<JobTypeEnum> {JobTypeEnum.ARCHIVE},
                        jobStatuses: new List<JobStatusEnum> {JobStatusEnum.COMPLETED}));
                Assert.AreEqual(archiveJobsBefore + 1, jobs.JobsList.Count);

                jobs = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJobs(
                    new GetJobsRequest(
                        jobTypes: new List<JobTypeEnum> {JobTypeEnum.RESTORE}));
                Assert.AreEqual(restoreJobsBefore, jobs.JobsList.Count);

                jobs = SpectraRioBrokerClientFixture.SpectraRioBrokerClient.GetJobs(
                    new GetJobsRequest(
                        jobTypes: new List<JobTypeEnum> {JobTypeEnum.ARCHIVE},
                        jobStatuses: new List<JobStatusEnum> {JobStatusEnum.COMPLETED},
                        sortBy: JobsSortByEnum.CREATION_DATE,
                        sortOrder: SortOrderEnum.DESC));
                Assert.AreEqual(archiveJob.JobId, jobs.JobsList[0].JobId);
            }
            finally
            {
                var deleteF1Request = new DeleteFileRequest(SpectraRioBrokerClientFixture.BrokerName, fileName1);
                SpectraRioBrokerClientFixture.SpectraRioBrokerClient.DeleteFile(deleteF1Request);
            }
        }
    }
}