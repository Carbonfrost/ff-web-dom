//
// - DomTextTests.cs -
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
    public class DomTextTests {

        [Test]
        public void test_basic_property_access() {
            DomDocument doc = new DomDocument();
            string ws = "leading ws";
            var html = doc.AppendText(ws);

            Assert.That(html.ToString(), Is.EqualTo(ws));
            Assert.That(html.TextContent, Is.EqualTo(ws));
            Assert.That(html.Data, Is.EqualTo(ws));
            Assert.That(html.NodeValue, Is.EqualTo(ws));
        }

        [Test]
        public void test_replace_with_document_element() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendText("leading ws");
            var head = doc.CreateElement("head");
            var result = html.ReplaceWith(head);

            Assert.That(doc.ToXml(), Is.EqualTo("<head />"));
            Assert.That(result, Is.SameAs(head));
        }

        [Test]
        public void test_outer_text_domelement() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendText("leading ws");
            string htmlText = "leading ws";

            Assert.That(html.OuterText, Is.EqualTo(htmlText));
            Assert.That(html.ToString(), Is.EqualTo(htmlText));

            Assert.That(doc.OuterText, Is.EqualTo(htmlText));
            Assert.That(doc.ToString(), Is.EqualTo(htmlText));
        }

        [Test]
        public void test_add_child_implies_parent_and_owner() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var text = html.AppendText("ws");
            Assert.That(text.ChildNodes.Count, Is.EqualTo(0));
            Assert.That(text.OwnerDocument, Is.SameAs(doc));
            Assert.That(text.ParentNode, Is.SameAs(html));
            Assert.That(text.ParentElement, Is.SameAs(html));
        }

        [Test]
        public void test_add_child_document_element_implies_parent_and_owner() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendText("leading ws");
            Assert.That(html.ChildNodes.Count, Is.EqualTo(0));
            Assert.That(html.OwnerDocument, Is.SameAs(doc));
            Assert.That(html.ParentNode, Is.SameAs(doc));
            Assert.That(html.ParentElement, Is.Null);
        }
    }
}


