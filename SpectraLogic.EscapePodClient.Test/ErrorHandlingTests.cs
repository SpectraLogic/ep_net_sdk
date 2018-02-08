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
using log4net.Config;
using Moq;
using NUnit.Framework;
using SpectraLogic.EscapePodClient.Calls;
using SpectraLogic.EscapePodClient.Exceptions;
using SpectraLogic.EscapePodClient.Runtime;
using SpectraLogic.EscapePodClient.Test.Mock;
using System.Net;
using System.Threading.Tasks;

namespace SpectraLogic.EscapePodClient.Test
{
    [TestFixture]
    internal class ErrorHandlingTests
    {
        private static readonly ILog Log = LogManager.GetLogger("ErrorHandlingTests");

        public ErrorHandlingTests()
        {
            BasicConfigurator.Configure();
        }

        [Test]
        public void GetArchiveExceptionTest()
        {
            var getArchiveRequest = new GetArchiveRequest("error");

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .SetupSequence(n => n.Invoke(getArchiveRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.EscapePodClient.Test.TestFiles.ArchiveNotFoundExceptionResponse",
                    HttpStatusCode.NotFound, null))
                .Returns(new MockHttpWebResponse("SpectraLogic.EscapePodClient.Test.TestFiles.InvalidEscapePodServerCredentialsExceptionResponse",
                    HttpStatusCode.Forbidden, null));

            var mockBuilder = new Mock<IEscapePodClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new EscapePodClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            Assert.ThrowsAsync<ArchiveNotFoundException>(() => Task.FromResult(client.GetArchive(getArchiveRequest)));
            Assert.ThrowsAsync<InvalidEscapePodServerCredentialsException>(() => Task.FromResult(client.GetArchive(getArchiveRequest)));

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }
    }
}
