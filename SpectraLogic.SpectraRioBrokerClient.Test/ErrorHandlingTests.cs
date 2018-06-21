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
using SpectraLogic.SpectraRioBrokerClient.Calls;
using SpectraLogic.SpectraRioBrokerClient.Exceptions;
using SpectraLogic.SpectraRioBrokerClient.Runtime;
using SpectraLogic.SpectraRioBrokerClient.Test.Mock;
using System.Net;
using System.Threading.Tasks;

namespace SpectraLogic.SpectraRioBrokerClient.Test
{
    [TestFixture]
    internal class ErrorHandlingTests
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger("ErrorHandlingTests");

        #endregion Fields

        #region Constructors

        public ErrorHandlingTests()
        {
            BasicConfigurator.Configure();
        }

        #endregion Constructors

        #region Tests

        [Test]
        public void GetBrokerExceptionTest()
        {
            var getBrokerRequest = new GetBrokerRequest("error");

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .SetupSequence(n => n.Invoke(getBrokerRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.BrokerNotFoundExceptionResponse",
                    HttpStatusCode.NotFound, null))
                .Returns(new MockHttpWebResponse("SpectraLogic.SpectraRioBrokerClient.Test.TestFiles.InvalidServerCredentialsExceptionResponse",
                    HttpStatusCode.Forbidden, null));

            var mockBuilder = new Mock<ISpectraRioBrokerClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new SpectraRioBrokerClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            Assert.ThrowsAsync<BrokerNotFoundException>(() => Task.FromResult(client.GetBroker(getBrokerRequest)));
            Assert.ThrowsAsync<InvalidServerCredentialsException>(() => Task.FromResult(client.GetBroker(getBrokerRequest)));

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }

        //TODO add more tests for missing api calls

        #endregion Tests
    }
}