//
// - DomDocumentTests.cs -
//
// Copyright 2013 Carbonfrost Systems, Inc. (http://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Linq;
using System.Xml;
using NUnit.Framework;

namespace Carbonfrost.Commons.Web.Dom {

    [TestFixture]
    public class DomDocumentTests {

        [Test]
        public void test_create_child_implies_owner_document() {
            DomDocument doc = new DomDocument();
            var html = doc.CreateElement("html");

            Assert.That(html.ParentNode, Is.Null);
            Assert.That(html.OwnerDocument, Is.SameAs(doc));
        }

        [Test]
        public void test_owner_document_no_owner() {
            DomDocument doc = new DomDocument();
            Assert.That(doc.OwnerDocument, Is.Null);
        }

        [Test]
        public void test_attributes_null_and_no_values() {
            DomDocument doc = new DomDocument();

            Assert.That(doc.Attributes, Is.Null);

            // TODO Review this behavior: should it be an error?
            Assert.That(doc.Attribute("s", "s"), Is.Null);
        }

        [Test]
        public void test_to_xml_string() {
            DomDocument doc = new DomDocument();
            // doc.AppendDocumentType("html");
            var html = doc.AppendElement("html");
            var body = html.AppendElement("body");
            body.AppendElement("h1").AppendText("Hello, world");
            html.Attribute("lang", "en");

            // N.B. By default, xml decl is omitted
            Assert.That(doc.ToXml(), Is.EqualTo("<html lang=\"en\"><body><h1>Hello, world</h1></body></html>"));
        }

        [Test]
        public void test_create_attribute_implies_owner_document() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var attr = doc.CreateAttribute("class");
            html.Attributes.Add(attr);
            Assert.That(attr.OwnerElement, Is.SameAs(html));
            Assert.That(attr.ParentNode, Is.Null);
            Assert.That(attr.OwnerDocument, Is.SameAs(doc));
        }

        [Test]
        public void test_create_attribute_unlinked_implies_owner_document() {
            DomDocument doc = new DomDocument();
            var html = doc.CreateAttribute("class");
            Assert.That(html.OwnerElement, Is.Null);
            Assert.That(html.ParentNode, Is.Null);
            Assert.That(html.OwnerDocument, Is.SameAs(doc));
        }

        [Test]
        public void test_append_multiple_noncontent_nodes() {
            DomDocument doc = new DomDocument();
            var docType = doc.CreateDocumentType("html");
            var ws = doc.CreateComment("time");
            var html = doc.CreateElement("html");
            doc.Append(docType, ws, html);
            Assert.That(doc.DocumentElement, Is.SameAs(html));
            Assert.That(doc.ChildNodes.Count, Is.EqualTo(3));
        }

        [Test]
        public void test_replace_element_with_element() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var body = html.AppendElement("body");
            var head = (DomElement) body.ReplaceWith(doc.CreateElement("head"));
            Assert.That(((DomElement) html.ChildNode(0)).Name, Is.EqualTo("head"));
            Assert.That(head.Name, Is.EqualTo("head"));
        }

        [Test]
        public void test_common_element_properties() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            Assert.That(html.NodeValue, Is.Null);
        }
    }
}
