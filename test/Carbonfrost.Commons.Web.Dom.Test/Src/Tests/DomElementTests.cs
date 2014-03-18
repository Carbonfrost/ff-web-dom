//
// - DomElementTests.cs -
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
    public class DomElementTests {

        [Test]
        public void test_outer_text_domelement() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var body = html.AppendElement("body");
            html.Attribute("lang", "en");
            html.Attribute("data-cast", "true");
            body.Attribute("dir", "ltr");
            body.Attribute("class", "hl");
            body.AppendText("Hello, world!");
            string htmlText = "<html lang=\"en\" data-cast=\"true\"><body dir=\"ltr\" class=\"hl\">Hello, world!</body></html>";
            Assert.That(html.OuterText, Is.EqualTo(htmlText));
            Assert.That(html.ToString(), Is.EqualTo(htmlText));
        }

        [Test]
        public void test_remove_self_implies_parent_and_owner() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var body = html.AppendElement("head");

            DomElement e = body.RemoveSelf();
            Assert.That(html.ChildNodes.Count, Is.EqualTo(0));
            Assert.That(e, Is.SameAs(body));
            Assert.That(e.OwnerDocument, Is.SameAs(doc));
            Assert.That(e.ParentNode, Is.Null);
            Assert.That(e.ParentElement, Is.Null);
        }

        [Test]
        public void test_replace_with_many_nominal() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var body = html.AppendElement("body");
            var h1 = body.AppendElement("h1");
            var h3 = doc.CreateElement("h3");

            var result = h1.ReplaceWith(h3, doc.CreateElement("h4"), doc.CreateElement("h5"));
            Assert.That(doc.ToXml(), Is.EqualTo("<html><body><h3 /><h4 /><h5 /></body></html>"));
            Assert.That(result, Is.SameAs(h3));
        }

        [Test]
        public void test_descendents_and_self() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var body = html.AppendElement("body");
            var para1 = body.AppendElement("p");
            var para2 = body.AppendElement("div");
            para1.AppendText("Hello, world!");
            para2.AppendText("Hello, world!");

            Assert.That(string.Join(" ", html.DescendantsAndSelf.Select(t => t.NodeName)),
                        Is.EquivalentTo("html body p div"));
            Assert.That(string.Join(" ", html.Descendants.Select(t => t.NodeName)),
                        Is.EquivalentTo("body p div"));
        }

        [Test]
        public void test_elements_list_nominal() {
            DomDocument doc = new DomDocument();
            var svg = doc.AppendElement("svg");
            var g1 = svg.AppendElement("g");
            var g2 = svg.AppendElement("g");
            var g3 = svg.AppendElement("g");

            Assert.That(svg.Elements[1], Is.SameAs(g2));
            Assert.That(svg.Elements[2], Is.SameAs(g3));

            Assert.That(svg.Element(1), Is.SameAs(g2));
            Assert.That(svg.Element(2), Is.SameAs(g3));

            Assert.That(svg.Element("g"), Is.SameAs(g1));
        }

    }

}
