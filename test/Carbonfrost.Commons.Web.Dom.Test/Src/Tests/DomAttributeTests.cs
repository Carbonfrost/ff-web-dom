//
// - DomAttributeTests.cs -
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
using NUnit.Framework;

namespace Carbonfrost.Commons.Web.Dom {

    [TestFixture]
    public class DomAttributeTests {

        [Test]
        public void test_attribute_implies_new_instance() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            html.Attribute("lang", "en");
            DomAttribute attr = html.Attributes[0];
            Assert.That(html.Attributes.Count, Is.EqualTo(1));
            Assert.That(html.ChildNodes.Count, Is.EqualTo(0));

            // Excludes attributes
            Assert.That(attr.Value, Is.EqualTo("en"));
            Assert.That(attr.Name, Is.EqualTo("lang"));
            Assert.False(doc.UnlinkedNodes.Contains(attr));
        }

        [Test]
        public void test_add_attribute_implies_parent_and_owner() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            html.Attribute("lang", "en");

            DomAttribute attr = html.Attributes[0];
            Assert.That(attr.OwnerDocument, Is.SameAs(doc));
            Assert.That(attr.OwnerElement, Is.SameAs(html));
            Assert.That(attr.ParentNode, Is.Null); // per spec
            Assert.False(doc.UnlinkedNodes.Contains(attr));
        }

        [Test]
        public void test_append_attribute_implies_parent_and_owner() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var attr = html.AppendAttribute("lang", "en");

            Assert.That(attr.OwnerDocument, Is.SameAs(doc));
            Assert.That(attr.OwnerElement, Is.SameAs(html));
            Assert.That(attr.ParentNode, Is.Null); // per spec
        }

        [Test]
        public void test_set_dom_value_getter() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var classList = new DomStringTokenList();
            html.Attribute("class", classList);

            Assert.That(html.Attribute("class"), Is.EqualTo(string.Empty));
            classList.Add("cool");
            classList.Add("down");
            Assert.That(html.Attribute("class"), Is.EqualTo("cool down"));
        }

        [Test]
        public void test_set_dom_value_setter() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var classList = new DomStringTokenList();
            html.Attribute("class", classList);
            html.Attribute("class", "heat up");
            Assert.That(classList, Contains.Item("heat"));
            Assert.That(classList, Contains.Item("up"));
        }

        [Test]
        public void test_remove_self_implies_parent_and_owner() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            html.Attribute("lang", "en");

            DomAttribute attr = html.Attributes[0].RemoveSelf();
            Assert.That(html.Attributes.Count, Is.EqualTo(0));
            Assert.That(attr.OwnerDocument, Is.SameAs(doc));
            Assert.That(attr.ParentNode, Is.Null);
        }

        // TODO Similar RemoveSelf tests with DomElement

        [Test]
        public void test_append_attribute_implies_add() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var attr = doc.CreateAttribute("lang", "en");

            html.Append(attr);

            Assert.That(html.Attributes.Count, Is.EqualTo(1));
            Assert.That(html.Attribute("lang"), Is.EqualTo("en"));
            Assert.That(attr.OwnerDocument, Is.SameAs(doc));
        }

        [Test]
        public void test_create_attribute_is_unlinked() {
            DomDocument doc = new DomDocument();
            var attr = doc.CreateAttribute("lang", "en");

            Assert.That(attr.OwnerDocument, Is.SameAs(doc));
        }

        [Test]
        public void test_attribute_duplicate_name_error() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var attr = html.AppendAttribute("lang", "en");

            Assert.That(() => {
                            html.AppendAttribute("lang", "fr");
                        }, Throws.ArgumentException);

            Assert.That(() => {
                            html.Append(doc.CreateAttribute("lang", "fr"));
                        }, Throws.ArgumentException);

            Assert.That(() => {
                            html.Attributes.Add(doc.CreateAttribute("lang", "fr"));
                        }, Throws.ArgumentException);
        }

        [Test]
        public void test_attribute_duplicate_name_same_instance() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var attr = html.AppendAttribute("lang", "en");
            Assert.That(html.Attributes.Count, Is.EqualTo(1));

            // TODO Should this move attribute to end of collection?

            html.Append(attr); // legal
            Assert.That(html.Attributes.Count, Is.EqualTo(1));
        }

        [Test]
        public void test_attribute_remove_self() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var attr = html.AppendAttribute("lang", "en");

            Assert.That(html.Attributes.Count, Is.EqualTo(1));
            attr.RemoveSelf();

            Assert.That(html.Attributes.Count, Is.EqualTo(0));
        }

        [Test]
        public void test_attribute_replace_with_non_attribute() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var attr = html.AppendAttribute("lang", "en");

            Assert.That(() => {
                            attr.ReplaceWith(doc.CreateCData());
                        }, Throws.ArgumentException);
        }

        [Test]
        public void test_attribute_replace_with_attribute() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var attr = html.AppendAttribute("lang", "en");
            var attr2 = doc.CreateAttribute("dir", "ltr");

            Assert.That(attr.ReplaceWith(attr2), Is.SameAs(attr2));
            Assert.That(html.Attributes.Count, Is.EqualTo(1));
            Assert.That(html.Attributes[0], Is.SameAs(attr2));
            Assert.True(doc.UnlinkedNodes.Contains(attr));
            Assert.False(doc.UnlinkedNodes.Contains(attr2));
        }

        [Test]
        public void test_attribute_adjacent_nominal() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            html.Attribute("lang", "en");
            html.Attribute("dir", "ltr");
            html.Attribute("data-e", "e");

            DomAttribute attr = html.Attributes[1];
            Assert.That(attr.NodePosition, Is.EqualTo(1));
            Assert.That(attr.NextAttribute.Name, Is.EqualTo("data-e"));
            Assert.That(attr.PreviousAttribute.Name, Is.EqualTo("lang"));
        }

        [Test]
        public void test_attribute_adjacent_singleton() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            html.Attribute("lang", "en");

            DomAttribute attr = html.Attributes[0];
            Assert.That(attr.NextAttribute, Is.Null);
            Assert.That(attr.PreviousAttribute, Is.Null);
        }
    }
}
