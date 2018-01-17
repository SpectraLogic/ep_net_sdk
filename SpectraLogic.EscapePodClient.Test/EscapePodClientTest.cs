using System.Net;
using Moq;
using NUnit.Framework;
using SpectraLogic.EscapePodClient.Calls;
using SpectraLogic.EscapePodClient.Runtime;
using SpectraLogic.EscapePodClient.Test.Mock;
using SpectraLogic.EscapePodClient.Test.Utils;
using SpectraLogic.EscapePodClient.Utils;

namespace SpectraLogic.EscapePodClient.Test
{
    [TestFixture]
    internal class EscapePodClientTest
    {
        [Test]
        public void ArchiveTest()
        {
            var archiveRequest =
                HttpUtils<ArchiveRequest>.JsonToObject(
                    ResourceFilesUtils.Read("SpectraLogic.EscapePodClient.Test.TestFiles.ArchiveRequest"));
            Assert.AreEqual("api/archive\nPOST\n{\"Files\":[{\"Links\":[\"clipName\"],\"Metadata\":[{\"Key\":\"key\",\"Value\":\"value\"}],\"Name\":\"fileName\",\"Size\":1234,\"Uri\":\"uri\"}]}", archiveRequest.ToString());

            var mockNetwork = new Mock<INetwork>(MockBehavior.Strict);
            mockNetwork
                .Setup(n => n.Invoke(archiveRequest))
                .Returns(new MockHttpWebResponse("SpectraLogic.EscapePodClient.Test.TestFiles.ArchiveResponse",
                    HttpStatusCode.OK, null));
            
            var mockBuilder = new Mock<IEscapePodClientBuilder>(MockBehavior.Strict);
            mockBuilder
                .Setup(b => b.Build())
                .Returns(new EscapePodClient(mockNetwork.Object));

            var builder = mockBuilder.Object;
            var client = builder.Build();

            var job = client.Archive(archiveRequest);
            Assert.AreEqual("123456789", job.Id);

            mockBuilder.VerifyAll();
            mockNetwork.VerifyAll();
        }
    }
}
