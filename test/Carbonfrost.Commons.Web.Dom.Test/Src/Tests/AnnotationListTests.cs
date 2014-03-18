//
// - AnnotationListTests.cs -
//
// Copyright 2014 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
    public class AnnotationListTests {

        [Test]
        public void test_add_annotation_nominal() {
            DomDocument d = new DomDocument();
            d.AddAnnotation(DBNull.Value);
            Assert.True(d.HasAnnotation<DBNull>());
            Assert.That(d.Annotations<DBNull>(), Is.Not.Empty);
            Assert.That(d.Annotations<Uri>(), Is.Empty);
            Assert.That(d.Annotation<DBNull>(), Is.EqualTo(DBNull.Value));
        }
    }
}


