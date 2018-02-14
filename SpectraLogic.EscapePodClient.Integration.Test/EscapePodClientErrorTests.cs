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
        #region Tests

        [Test]
        public void ArchiveErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new ArchiveRequest(null, Enumerable.Empty<ArchiveFile>())));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new ArchiveRequest(string.Empty, null)));

            //TODO add test for InvalidEscapePodServerCredentialsException

            var request = new ArchiveRequest("not_found", Enumerable.Empty<ArchiveFile>());
            Assert.ThrowsAsync<ArchiveNotFoundException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.Archive(request)));
        }

        [Test]
        public void CreateArchiveErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateArchiveRequest(null, EscapePodClientFixture.GetResolver())));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateArchiveRequest(EscapePodClientFixture.ArchiveName, null)));

            var request = new CreateArchiveRequest(EscapePodClientFixture.ArchiveName, EscapePodClientFixture.GetResolver());
            Assert.ThrowsAsync<ArchiveAlreadyExistsException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateArchive(request)));

            request = new CreateArchiveRequest(string.Empty, EscapePodClientFixture.GetResolver());
            Assert.ThrowsAsync<ValidationException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateArchive(request)));

            request = new CreateArchiveRequest("should_fail", new ResolverConfig(string.Empty, "bp_name", "username", "bucket", false));
            Assert.ThrowsAsync<ValidationException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateArchive(request)));

            request = new CreateArchiveRequest("should_fail", new ResolverConfig("name", "bp_name", "username", "bucket", false));
            Assert.ThrowsAsync<ValidationException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateArchive(request)));

            request = new CreateArchiveRequest("should_fail", new ResolverConfig(EscapePodClientFixture.ResolverName, EscapePodClientFixture.DeviceName, "username", "bucket", false));
            Assert.ThrowsAsync<ValidationException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateArchive(request)));

            request = new CreateArchiveRequest("should_fail", new ResolverConfig(EscapePodClientFixture.ResolverName,
                EscapePodClientFixture.DeviceName, EscapePodClientFixture.BlackPearlUserName, "wrong_bucket", false));
            Assert.ThrowsAsync<ValidationException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateArchive(request)));
        }

        [Test]
        public void CreateDeviceErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateDeviceRequest(null, "endpoint", "username", "password")));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateDeviceRequest("name", null, "username", "password")));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateDeviceRequest("name", "endpoint", null, "password")));
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new CreateDeviceRequest("name", "endpoint", "username", null)));

            //TODO add test for InvalidEscapePodServerCredentialsException

            var request = new CreateDeviceRequest(EscapePodClientFixture.DeviceName, "localhost", "username", "password");
            Assert.ThrowsAsync<DeviceAlreadyExistsException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateDevice(request)));

            request = new CreateDeviceRequest(string.Empty, "localhost", "username", "password");
            Assert.ThrowsAsync<ValidationException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateDevice(request)));

            request = new CreateDeviceRequest("name", string.Empty, "username", "password");
            Assert.ThrowsAsync<ValidationException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateDevice(request)));

            request = new CreateDeviceRequest("name", "bad url", "username", "password");
            Assert.ThrowsAsync<ValidationException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateDevice(request)));

            request = new CreateDeviceRequest("name", "localhost", string.Empty, "password");
            Assert.ThrowsAsync<ValidationException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateDevice(request)));

            request = new CreateDeviceRequest("name", "localhost", "username", string.Empty);
            Assert.ThrowsAsync<ValidationException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateDevice(request)));

            request = new CreateDeviceRequest(string.Empty, string.Empty, string.Empty, string.Empty);
            Assert.ThrowsAsync<ValidationException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateDevice(request)));

            request = new CreateDeviceRequest("should_fail", EscapePodClientFixture.Endpoint, EscapePodClientFixture.Username, "wrong_password");
            Assert.ThrowsAsync<ValidationException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateDevice(request)));

            request = new CreateDeviceRequest("should_fail", EscapePodClientFixture.Endpoint, "wrong_username", EscapePodClientFixture.Password);
            Assert.ThrowsAsync<ValidationException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.CreateDevice(request)));
        }

        [Test]
        public void GetArchiveErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new GetArchiveRequest(null)));

            //TODO add test for InvalidEscapePodServerCredentialsException

            var request = new GetArchiveRequest("not_found");
            Assert.ThrowsAsync<ArchiveNotFoundException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.GetArchive(request)));
        }

        [Test]
        public void GetEscapePodJobErrorTests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new GetEscapePodJobRequest(null, Guid.NewGuid())));

            //TODO add test for InvalidEscapePodServerCredentialsException

            var request = new GetEscapePodJobRequest("not_found", Guid.NewGuid());
            Assert.ThrowsAsync<ArchiveNotFoundException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.GetJob(request)));

            request = new GetEscapePodJobRequest(EscapePodClientFixture.ArchiveName, Guid.NewGuid());
            Assert.ThrowsAsync<ArchiveJobNotFoundException>(() => Task.FromResult(EscapePodClientFixture.EscapePodClient.GetJob(request)));
        }

        [Test]
        public void HeadDeviceErrorTests()
        {
            //TODO add test for InvalidEscapePodServerCredentialsException

            Assert.IsFalse(EscapePodClientFixture.EscapePodClient.IsDeviceExist("not_found"));
        }

        [Test]
        public void HeadArchiveErrorTests()
        {
            //TODO add test for InvalidEscapePodServerCredentialsException

            Assert.IsFalse(EscapePodClientFixture.EscapePodClient.IsArchiveExist("not_found"));
        }

        #endregion Tests
    }
}