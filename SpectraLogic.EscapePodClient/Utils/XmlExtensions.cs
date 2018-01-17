using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace SpectraLogic.EscapePodClient.Utils
{
    internal static class XmlExtensions
    {
        public static XDocument ReadDocument(Stream content)
        {
            return XDocument.Load(new XmlNoNamespaceReader(content));
        }

        private static XAttribute AttributeOrThrow(this XElement self, string attributeName)
        {
            var element = self.Attribute(attributeName);
            if (element == null)
            {
                throw new Exception(attributeName);
            }

            return element;
        }

        public static string AttributeText(this XElement self, string attributeName)
        {
            return self.AttributeOrThrow(attributeName).Value;
        }

        public static string AttributeTextOrNull(this XElement self, string attributeName)
        {
            var attribute = self.Attribute(attributeName);
            return attribute?.Value;
        }

        public static XElement ElementOrThrow(this XContainer self, string elementName)
        {
            var element = self.Element(elementName);
            if (element == null)
            {
                throw new Exception(elementName);
            }
            return element;
        }

        private class XmlNoNamespaceReader : XmlTextReader
        {
            public XmlNoNamespaceReader(Stream stream)
                : base(stream)
            {
            }

            public override string NamespaceURI => "";
        }
    }
}
