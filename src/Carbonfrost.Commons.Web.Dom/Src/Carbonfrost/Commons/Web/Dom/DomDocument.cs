//
// - DomDocument.cs -
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
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Carbonfrost.Commons.Shared;
using Carbonfrost.Commons.Shared.Runtime;

namespace Carbonfrost.Commons.Web.Dom {

    public class DomDocument : DomContainer, IDomNodeFactory {

        private readonly DomNodeCollection unlinked;

        public DomElement DocumentElement {
            get {
                return (DomElement) this.ChildNodes.FirstOrDefault(t => t.NodeType == DomNodeType.Element);
            }
        }

        public virtual DomProviderFactory ProviderFactory {
            get {
                return DomProviderFactory.Default;
            }
        }

        internal override DomDocument OwnerDocumentOrSelf {
            get {
                return this;
            }
        }

        protected override DomAttributeCollection DomAttributes {
            get {
                return null;
            }
        }

        public override IReadOnlyList<DomElement> Elements {
            get {
                return DocumentElement.Elements;
            }
        }

        public override string NodeName {
            get {
                return "#document";
            }
        }

        public override DomNodeType NodeType {
            get {
                return DomNodeType.Document;
            }
        }

        internal DomNodeCollection UnlinkedNodes {
            get {
                return unlinked;
            }
        }

        public DomDocument() {
            this.unlinked = new DomNodeCollection(this, new List<DomNode>(32));
        }

        protected override DomNode ReplaceWithCore(DomNode other) {
            if (other == null)
                throw new ArgumentNullException("other");
            if (other == this)
                return this;

            throw DomFailure.CannotReplaceDocument();
        }

        public DomElement GetElementById(string id) {
            // TODO Implement IdDomValue to index these (performance)
            return (DomElement) Select("#" + id).FirstOrDefault();
        }

        public void Load(string fileName) {
            Load(StreamContext.FromFile(fileName));
        }

        public void Load(Uri source) {
            Load(StreamContext.FromSource(source));
        }

        public void Load(Stream input) {
            Load(StreamContext.FromStream(input));
        }

        public void Load(StreamContext input) {
            if (input == null)
                throw new ArgumentNullException("input");

            LoadText(input.OpenText());
        }

        protected virtual void LoadText(TextReader input) {
            if (input == null)
                throw new ArgumentNullException("input");
                
            var reader = this.ProviderFactory.CreateReader(input);
            
            // TODO By default, use document semantics on loading
            throw new NotImplementedException();
        }

        internal override TResult AcceptVisitor<TArgument, TResult>(IDomNodeVisitor<TArgument, TResult> visitor, TArgument argument) {
            return visitor.Visit(this, argument);
        }

        internal override void AcceptVisitor(IDomNodeVisitor visitor) {
            visitor.Visit(this);
        }

        public DomComment CreateComment() {
            return CreateCommentCore();
        }

        public DomComment CreateComment(string data) {
            var result = CreateCommentCore();
            result.Data = data;
            return AddUnlinked(result);
        }

        protected virtual DomComment CreateCommentCore() {
            return this.ProviderFactory.NodeFactory.CreateComment();
        }

        public DomCDataSection CreateCData() {
            return CreateCData(string.Empty);
        }

        public DomCDataSection CreateCData(string data) {
            var result = CreateCDataCore();
            result.Data = data;
            return AddUnlinked(result);
        }

        protected virtual DomCDataSection CreateCDataCore() {
            return this.ProviderFactory.NodeFactory.CreateCData();
        }

        public DomText CreateText() {
            var result = CreateTextCore();
            return AddUnlinked(result);
        }

        public DomText CreateText(string data) {
            var result = CreateTextCore();
            result.Data = data;
            return AddUnlinked(result);
        }

        protected virtual DomText CreateTextCore() {
            return this.ProviderFactory.NodeFactory.CreateText();
        }

        public DomProcessingInstruction CreateProcessingInstruction(string target) {
            var result = CreateProcessingInstructionCore(target);
            return AddUnlinked(result);
        }

        public DomProcessingInstruction CreateProcessingInstruction(string target, string data) {
            var result = CreateProcessingInstructionCore(target);
            result.Data = data;
            return AddUnlinked(result);
        }

        protected virtual DomProcessingInstruction CreateProcessingInstructionCore(string target) {
            if (target == null)
                throw new ArgumentNullException("target");
            if (target.Length == 0)
                throw Failure.EmptyString("target");

            return this.ProviderFactory.NodeFactory.CreateProcessingInstruction(target);
        }

        public virtual Type GetAttributeNodeType(string name) {
            return this.ProviderFactory.NodeFactory.GetAttributeNodeType(name);
        }

        public virtual Type GetElementNodeType(string name) {
            return this.ProviderFactory.NodeFactory.GetElementNodeType(name);
        }

        public virtual Type GetProcessingInstructionNodeType(string name) {
            return this.ProviderFactory.NodeFactory.GetProcessingInstructionNodeType(name);
        }

        public virtual Type GetTextNodeType(string name) {
            return this.ProviderFactory.NodeFactory.GetTextNodeType(name);
        }

        public virtual Type GetCommentNodeType(string name) {
            return this.ProviderFactory.NodeFactory.GetCommentNodeType(name);
        }

        public virtual Type GetNotationNodeType(string name) {
            return this.ProviderFactory.NodeFactory.GetNotationNodeType(name);
        }

        public virtual Type GetEntityReferenceNodeType(string name) {
            return this.ProviderFactory.NodeFactory.GetEntityReferenceNodeType(name);
        }

        public virtual Type GetEntityNodeType(string name) {
            return this.ProviderFactory.NodeFactory.GetEntityNodeType(name);
        }

        public DomAttribute CreateAttribute(string name) {
            return AddUnlinked(CreateAttributeCoreSafe(name, null));
        }

        public DomAttribute CreateAttribute(string name, string value) {
            return AddUnlinked(CreateAttributeCoreSafe(name, value));
        }

        public DomAttribute CreateAttribute(string name, IDomValue value) {
            if (value == null)
                throw new ArgumentNullException("value");

            var result = CreateAttributeCoreSafe(name, value.Value);
            result.DomValue = value;
            return AddUnlinked(result);
        }

        protected virtual DomAttribute CreateAttributeCore(string name, string value) {
            return this.ProviderFactory.NodeFactory.CreateAttribute(name, value);
        }

        private DomAttribute CreateAttributeCoreSafe(string name, string value) {
            var result = CreateAttributeCore(name, value);
            if (result == null) {
                result = new DomAttribute(name);
                result.Value = value;
            }

            return result;
        }

        public DomElement CreateElement(string name) {
            var e = CreateElementCore(name);
            if (e == null) {
                e = new DomElement(name);
            }

            return AddUnlinked(e);
        }

        protected virtual DomElement CreateElementCore(string name) {
            return this.ProviderFactory.NodeFactory.CreateElement(name);
        }

        public DomEntityReference CreateEntityReference(string data) {
            var result = CreateEntityReferenceCore();
            result.Data = data;
            return AddUnlinked(result);
        }

        protected virtual DomEntityReference CreateEntityReferenceCore() {
            return new DomEntityReference();
        }

        public DomDocumentType CreateDocumentType(string name) {
            return CreateDocumentType(name, null, null);
        }

        public DomDocumentType CreateDocumentType(string name, string publicId, string systemId) {
            var result = CreateDocumentTypeCore(name);

            // TODO Lookup based on name
            result.PublicId = publicId;
            result.SystemId = systemId;
            return AddUnlinked(result);
        }

        protected virtual DomDocumentType CreateDocumentTypeCore(string name) {
            return new DomDocumentType(name);
        }

        public virtual DomElementDefinition GetElementDefinition(string name) {
            // TODO Relocate this method, gather from infoset
            return new DomElementDefinition(name);
        }

        internal void UpdateElementIndex(string id, DomElement element) {
        }

        private T AddUnlinked<T>(T item) where T : DomNode {
            this.unlinked.Add(item);
            return item;
        }

    }


}
