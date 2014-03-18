//
// - DomContainerTests.cs -
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
    public class DomContainerTests {

        [Test]
        public void test_append_element_implies_parent() {
            DomDocument doc = new DomDocument();
            var ele = doc.AppendElement("html");
            Assert.That(ele.ParentNode, Is.SameAs(doc));
            Assert.That(doc.DocumentElement, Is.SameAs(ele));
            Assert.That(doc.DocumentElement.ChildNodes, Is.Empty);
            Assert.False(doc.UnlinkedNodes.Contains(ele));
        }
    }
}
