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

using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace SpectraLogic.EscapePodClient.Utils
{
    internal static class XmlExtensions
    {
        #region Methods

        public static XDocument ReadDocument(Stream content)
        {
            return XDocument.Load(new XmlNoNamespaceReader(content));
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

        private static XAttribute AttributeOrThrow(this XElement self, string attributeName)
        {
            var element = self.Attribute(attributeName);
            if (element == null)
            {
                throw new Exception(attributeName);
            }

            return element;
        }

        #endregion Methods

        #region Classes

        private class XmlNoNamespaceReader : XmlTextReader
        {
            #region Constructors

            public XmlNoNamespaceReader(Stream stream)
                : base(stream)
            {
            }

            #endregion Constructors

            #region Properties

            public override string NamespaceURI => "";

            #endregion Properties
        }

        #endregion Classes
    }
}