//
// - DomProcessingInstructionTests.cs -
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
    public class DomProcessingInstructionTests {

        [Test]
        public void test_create_processing_instruction_nominal() {
            DomDocument doc = new DomDocument();
            var pi = doc.CreateProcessingInstruction("hello", "world");
            Assert.That(pi.TextContent, Is.EqualTo("world"));
            Assert.That(pi.Data, Is.EqualTo("world"));
            Assert.That(pi.Target, Is.EqualTo("hello"));
            Assert.That(pi.NodeValue, Is.EqualTo("world"));
            Assert.That(pi.Prefix, Is.Null);
            Assert.That(pi.LocalName, Is.Null);
            Assert.That(pi.NamespaceUri, Is.Null);
            Assert.That(pi.NodeName, Is.EqualTo("hello"));
            Assert.That(pi.ChildNodes.Count, Is.EqualTo(0));
            Assert.That(pi.NodeType, Is.EqualTo((DomNodeType) 7));
        }

        [Test]
        public void test_created_node_not_in_document_has_owner() {
            DomDocument doc = new DomDocument();
            var pi = doc.CreateProcessingInstruction("hello", "world");
            Assert.That(pi.ParentNode, Is.Null);
            Assert.That(pi.ParentElement, Is.Null);
            Assert.That(pi.OwnerDocument, Is.EqualTo(doc));
        }
    }
}
