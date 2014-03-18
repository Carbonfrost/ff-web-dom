//
// - DomNodeTests.cs -
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
    public class DomNodeTests {

        [Test]
        public void test_append_child_implies_parent() {

            DomDocument doc = new DomDocument();
            var html = doc.CreateElement("html");
            doc.Append(html);
            var body = doc.CreateElement("body");
            html.Append(body);
            Assert.That(html.ParentNode, Is.SameAs(doc));
            Assert.That(body.ParentNode, Is.SameAs(html));
            Assert.That(html.ChildNode(0), Is.SameAs(body));
            Assert.That(html.ChildNodes[0], Is.SameAs(body));
            Assert.That(html.ChildNodes, Is.EquivalentTo(new[] {
                                                             body
                                                         }));
            Assert.That(html.ChildNodes.ToArray(), Is.EquivalentTo(new[] {
                                                                       body
                                                                   }));
        }

        [Test]
        public void test_base_uri_inherited() {

            DomDocument doc = new DomDocument();
            var html = doc.CreateElement("html");
            var example = new Uri("https://example.com");
            html.BaseUri = example;

            doc.Append(html);
            var body = doc.CreateElement("body");
            html.Append(body);

            Assert.That(html.BaseUri, Is.EqualTo(example));
            Assert.That(body.BaseUri, Is.EqualTo(example));
        }
    }
}


