//
// - DomWriterAdapter.cs -
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

namespace Carbonfrost.Commons.Web.Dom {

    class DomWriterAdapter : DomNodeVisitor {

        private readonly DomWriter writer;

        public DomWriterAdapter(DomWriter writer) {
            this.writer = writer;
        }

        protected override void VisitElement(DomElement element) {
            if (element == null)
                throw new ArgumentNullException("element");

            writer.WriteStartElement(element.Name, element.NamespaceUri);
            Visit(element.Attributes);
            Visit(element.ChildNodes);
            writer.WriteEndElement();
        }

        protected override void VisitAttribute(DomAttribute attribute) {
            if (attribute == null)
                throw new ArgumentNullException("attribute");

            writer.WriteStartAttribute(attribute.Name, attribute.NamespaceUri);
            writer.WriteValue(attribute.Value);
            writer.WriteEndAttribute();
        }

        protected override void VisitDocument(DomDocument document) {
            if (document == null)
                throw new ArgumentNullException("document");

            writer.WriteStartDocument();
            Visit(document.ChildNodes);
            writer.WriteEndDocument();
        }

        protected override void VisitCDataSection(DomCDataSection section) {
            if (section == null)
                throw new ArgumentNullException("section");

            DefaultVisit(section);
        }

        protected override void VisitComment(DomComment comment) {
            if (comment == null)
                throw new ArgumentNullException("comment");

            writer.WriteComment(comment.Data);
        }

        protected override void VisitText(DomText text) {
            if (text == null)
                throw new ArgumentNullException("text");

            writer.WriteText(text.Data);
        }

        protected override void VisitProcessingInstruction(DomProcessingInstruction instruction) {
            if (instruction == null)
                throw new ArgumentNullException("instruction");

            writer.WriteProcessingInstruction(instruction.Target, instruction.Data);
        }

        protected override void VisitNotation(DomNotation notation) {
            if (notation == null)
                throw new ArgumentNullException("notation");

            writer.WriteNotation();
        }

        protected override void VisitDocumentType(DomDocumentType documentType) {
            if (documentType == null)
                throw new ArgumentNullException("documentType");

            writer.WriteDocumentType(documentType.Name, documentType.PublicId, documentType.SystemId);
        }

        protected override void VisitEntityReference(DomEntityReference reference) {
            if (reference == null)
                throw new ArgumentNullException("reference");

            writer.WriteEntityReference();
        }
    }
}
