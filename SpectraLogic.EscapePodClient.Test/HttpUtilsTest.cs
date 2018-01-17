using System.Collections.Generic;
using System.Runtime.Serialization;
using NUnit.Framework;
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
    }

    [DataContract]
    internal class TestObject
    {
        [DataMember] public string TestString;
        [DataMember] public int TestInt;
        [DataMember] public IDictionary<string, string> TestDictionary;
        [DataMember] public IEnumerable<string> TestEnumerable;

        public TestObject()
        {
        }

        public TestObject(string testString, int testInt, IDictionary<string, string> testDictionary, IEnumerable<string> testEnumerable)
        {
            TestString = testString;
            TestInt = testInt;
            TestDictionary = testDictionary;
            TestEnumerable = testEnumerable;
        }
    }
}
