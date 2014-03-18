//
// - DomAttributeCollectionTests.cs -
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
    public class DomAttributeCollectionTests {

        [Test]
        public void test_clear_collection_implies_no_parent() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var attr1 = html.AppendAttribute("lang", "en");
            var attr2 = html.AppendAttribute("dir", "ltr");
            var attr3 = html.AppendAttribute("class", "y");

            Assert.That(attr1.OwnerDocument, Is.SameAs(doc));
            Assert.That(attr1.OwnerElement, Is.SameAs(html));
            Assert.That(attr1.ParentElement, Is.Null); // spec
            html.Attributes.Clear();

            Assert.That(html.Attributes.Count, Is.EqualTo(0));
            Assert.That(attr1.OwnerDocument, Is.SameAs(doc));
            Assert.That(attr1.OwnerElement, Is.Null); // spec
            Assert.That(attr1.ParentElement, Is.Null);
        }

        [Test]
        public void test_remove_from_collection_implies_no_parent() {
            DomDocument doc = new DomDocument();
            var html = doc.AppendElement("html");
            var attr1 = html.AppendAttribute("lang", "en");

            Assert.That(attr1.OwnerDocument, Is.SameAs(doc));
            Assert.That(attr1.OwnerElement, Is.SameAs(html));
            Assert.That(attr1.ParentElement, Is.Null); // spec
            Assert.That(attr1.NodePosition, Is.EqualTo(0));
            Assert.That(html.Attributes.Count, Is.EqualTo(1));
            Assert.That(html.Attributes.IndexOf(attr1), Is.EqualTo(0));
            Assert.True(html.Attributes.Remove(attr1));
            Assert.That(html.Attributes.Count, Is.EqualTo(0));
            Assert.That(html.Attributes.IndexOf(attr1), Is.EqualTo(-1));

            Assert.That(attr1.OwnerDocument, Is.SameAs(doc));
            Assert.That(attr1.OwnerElement, Is.Null); // spec
            Assert.That(attr1.ParentElement, Is.Null);
        }
    }
}
