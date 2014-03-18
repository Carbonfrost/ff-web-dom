//
// - DomElementVisitorSpecializationTests.cs -
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace Carbonfrost.Commons.Web.Dom {

    [TestFixture]
    public class DomElementVisitorSpecializationTests {

        //        class FutureElement : DomElement<FutureElement> {
        //        }

        //        class FutureVisitor : DomElementVisitor<FutureElement, TextWriter> {
        //
        //            // probably needs to be TArgument passed along
        //
        //            private StringBuilder output;
        //
        //            public FutureVisitor(StringBuilder output) {
        //                this.output = output;
        //            }
        //
        //            public override void Visit(FutureElement element, TextWriter output) {
        //                output.Write("future(");
        //                // Visit(element.ChildNodes, output);
        //                output.Write(")");
        //            }
        //        }

        [Test]
        public void test_use_specialized_visitor_nominal() {
            // Because a FutureElement visitor is available, it should
            // be used
            //FutureElement e = new FutureElement();
            // e.AppendChild(new DomElement());

            // var visitor = DomNodeVisitor<TextWriter>.Compose(
            //    new FutureVisitor());
        }

        [Test]
        public void test_use_default_visitor_fallback() {
            // Because FutureElement isn't understood, default should
            // be used
        }
    }
}
