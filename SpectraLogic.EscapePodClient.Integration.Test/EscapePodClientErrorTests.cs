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

using NUnit.Framework;
using SpectraLogic.EscapePodClient.Calls;
using SpectraLogic.EscapePodClient.Exceptions;
using SpectraLogic.EscapePodClient.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpectraLogic.EscapePodClient.Integration.Test
{
    [TestFixture]
    public class EscapePodClientErrorTests
    {
        [Test]
        public void ArchiveErrorTests()
        {
            //TODO add test for InvalidEscapePodServerCredentialsException

            var request = new ArchiveRequest("not_found", Enumerable.Empty<ArchiveFile>());
            Assert.ThrowsAsync<ArchiveNotFoundException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.Archive(request)));

        }

        [Test]
        public void GetArchiveErrorTests()
        {
            //TODO add test for InvalidEscapePodServerCredentialsException

            var request = new GetArchiveRequest("not_found");
            Assert.ThrowsAsync<ArchiveNotFoundException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.GetArchive(request)));
        }

        [Test]
        public void GetEscapePodJobErrorTests()
        {
            //TODO add test for InvalidEscapePodServerCredentialsException

            var request = new GetEscapePodJobRequest("not_found", Guid.NewGuid());
            Assert.ThrowsAsync<ArchiveNotFoundException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.GetJob(request)));

            request = new GetEscapePodJobRequest(EscapePodClientFixture.ArchiveName, Guid.NewGuid());
            Assert.ThrowsAsync<ArchiveJobNotFoundException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.GetJob(request)));
        }
    }
}
