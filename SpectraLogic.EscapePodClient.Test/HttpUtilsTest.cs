using System.Collections.Generic;
using System.Runtime.Serialization;
using NUnit.Framework;
using SpectraLogic.EscapePodClient.Calls;
using SpectraLogic.EscapePodClient.Model;
using SpectraLogic.EscapePodClient.Utils;

namespace SpectraLogic.EscapePodClient.Test
{
    [TestFixture]
    internal class HttpUtilsTest
    {
        [Test]
        public void ObjectToJsonTest()
        {
            var testObject = new TestObject(
                "testString",
                123,
                new Dictionary<string, string>
                {
                    {"key", "value"}
                },
                new List<string>
                {
                    "value"
                });

            var json = HttpUtils<TestObject>.ObjectToJson(testObject);
            const string expected = "{\"TestDictionary\":[{\"Key\":\"key\",\"Value\":\"value\"}],\"TestEnumerable\":[\"value\"],\"TestInt\":123,\"TestString\":\"testString\"}";
            Assert.AreEqual(expected, json);
        }

        [Test]
        public void JsonToObject()
        {
            const string json = "{\"TestDictionary\":[{\"Key\":\"key\",\"Value\":\"value\"}],\"TestEnumerable\":[\"value\"],\"TestInt\":123,\"TestString\":\"testString\"}";
            var testObject = HttpUtils<TestObject>.JsonToObject(json);
            Assert.AreEqual("testString", testObject.TestString);
            Assert.AreEqual(123, testObject.TestInt);
            Assert.AreEqual(new Dictionary<string, string>{{"key", "value"}}, testObject.TestDictionary);
            Assert.AreEqual(new List<string>{"value"}, testObject.TestEnumerable);
        }

        [Test]
        public void ObjectToJsonArchiveRequestTest()
        {
            var files = new List<ArchiveFile>
            {
                new ArchiveFile("name", "uri", 123, new Dictionary<string, string> {{"key", "value"}},
                    new List<string> {"clip"})
            };

            var archiveRequest = new ArchiveRequest(files);
            var json = HttpUtils<ArchiveRequest>.ObjectToJson(archiveRequest);
            const string expected = "{\"Files\":[{\"Name\":\"name\",\"Uri\":\"uri\",\"Size\":123,\"Metadata\":[{\"Key\":\"key\",\"Value\":\"value\"}],\"Links\":[\"clip\"]}]}";
            Assert.AreEqual(expected, json);
        }

        [Test]
        public void ObjectToJsonRestoreRequestTest()
        {
            var files = new List<RestoreFile>
            {
                new RestoreFile("name", "dest", true),
                new RestoreFile("name2", "dest2", new ByteRange(0, 10)),
                new RestoreFile("name3", "dest3", new TimecodeRange(10, 20))
            };

            var restoreRequest = new RestoreRequest(files);
            var json = HttpUtils<RestoreRequest>.ObjectToJson(restoreRequest);
            const string expected = "{\"Files\":[{\"Name\":\"name\",\"Destination\":\"dest\",\"RestoreFileAttributes\":true,\"ByteRange\":null,\"TimeCodeRange\":null},{\"Name\":\"name2\",\"Destination\":\"dest2\",\"RestoreFileAttributes\":false,\"ByteRange\":{\"Start\":0,\"Stop\":10},\"TimeCodeRange\":null},{\"Name\":\"name3\",\"Destination\":\"dest3\",\"RestoreFileAttributes\":false,\"ByteRange\":null,\"TimeCodeRange\":{\"Start\":10,\"Stop\":20}}]}";
            Assert.AreEqual(expected, json);
        }

        [Test]
        public void ObjectToJsonDeleteRequestTest()
        {
            var files = new List<DeleteFile>
            {
                new DeleteFile("file1"),
                new DeleteFile("file2")
            };

            var deleteRequest = new DeleteRequest(files);
            var json = HttpUtils<DeleteRequest>.ObjectToJson(deleteRequest);
            const string expected = "{\"Files\":[{\"Name\":\"file1\"},{\"Name\":\"file2\"}]}";
            Assert.AreEqual(expected, json);
        }
    }

    [DataContract]
    internal class TestObject
    {
        [DataMember] public string TestString;
        [DataMember] public int TestInt;
        [DataMember] public IDictionary<string, string> TestDictionary;
        [DataMember] public IEnumerable<string> TestEnumerable;

        public TestObject(){ }

        public TestObject(string testString, int testInt, IDictionary<string, string> testDictionary, IEnumerable<string> testEnumerable)
        {
            TestString = testString;
            TestInt = testInt;
            TestDictionary = testDictionary;
            TestEnumerable = testEnumerable;
        }
    }
}
