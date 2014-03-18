//
// - DomDocumentProcessingInstructionScenarios.cs -
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
using Carbonfrost.Commons.Web.Dom;

namespace Scenarios {

    public class DomDocumentProcessingInstructionScenarios {

        public void construct_generic_dom_document_processing_factory() {
            DomDocument d = new DomDocument();
            var xml = d.CreateProcessingInstruction("xml");
            xml.AppendData("version=\"1.0\" standalone=\"yes\"");
            var html = d.CreateElement("html");
            var body = d.CreateElement("body");
            var p = d.CreateElement("p");
            var text = d.CreateText("Hello, World");

            p.Append(text);

            d.Append(html);
            body.Append(p).AppendTo(html);
        }

        public void construct_generic_dom_document_fluent_appenders() {
            DomDocument d = new DomDocument();
            d.AppendElement("html")
                .AppendElement("body")
                .AppendElement("p")
                .AppendText("Hello, World");
        }
    }
}
