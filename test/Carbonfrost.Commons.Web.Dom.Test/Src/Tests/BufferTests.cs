//
// - BufferTests.cs -
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
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Carbonfrost.Commons.Web.Dom {

    [TestFixture]
    public class BufferTests {

        [Test]
        public void test_concurrent_modification_retains_cache() {
            var list = new List<int> {
                1,
                2,
                3,
                4
            };

            var buffer = new Buffer<int>(list);
            Assert.That(buffer[0], Is.EqualTo(1));
            Assert.That(buffer[3], Is.EqualTo(4));
            Assert.That(buffer.Count, Is.EqualTo(4));

            // Underlying enumeration is concurrently modified
            list.RemoveAt(0);

            Assert.That(buffer[0], Is.EqualTo(2));
            Assert.That(buffer[2], Is.EqualTo(4));
            Assert.That(buffer.Count, Is.EqualTo(3));
        }
    }
}
