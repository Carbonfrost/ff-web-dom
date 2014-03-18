//
// - DomNode.Manipulation.cs -
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

namespace Carbonfrost.Commons.Web.Dom {

    partial class DomNode : IDomNodeManipulation<DomNode>, IDomNodeQuery<DomNode> {

        internal virtual DomDocument OwnerDocumentOrSelf {
            get {
                return this.OwnerDocument;
            }
        }

        public DomNode After(params DomNode[] nextSiblings) {
            if (nextSiblings == null || nextSiblings.Length == 0)
                return this;

            throw new NotImplementedException();
        }

        public DomNode Attribute(string name, object value) {
            if (this.Attributes == null)
                return null;

            var attr = this.Attributes.GetByNameOrCreate(name);
            attr.SetTypedValue(value);
            return this;
        }

        public DomNode Before(DomNode node) {
            if (node == null)
                throw new ArgumentNullException("node");

            if (this.ParentNode == null)
                throw DomFailure.ParentNodeRequired();

            this.ParentNode.ChildNodes.Insert(this.NodePosition, node);
            return this;
        }

        public DomNode After(DomNode node) {
            if (node == null)
                throw new ArgumentNullException("node");

            if (this.ParentNode == null)
                throw DomFailure.ParentNodeRequired();

            this.ParentNode.ChildNodes.Insert(this.NodePosition + 1, node);
            return this;
        }

        public DomNode InsertAfter(DomNode other) {
            other.After(this);
            return this;
        }

        public DomNode After(string markup) {
            var writer = this.AppendAfter();
            return AddSiblingHtml(writer, markup);
        }

        public DomNode Before(string markup) {
            var writer = this.PreviousSibling.AppendAfter();
            return AddSiblingHtml(writer, markup);
        }

        // TODO Before and After implementations

        public DomNode Before(params DomNode[] previousSiblings) {
            if (previousSiblings == null || previousSiblings.Length == 0)
                return this;

            throw new NotImplementedException();
        }

        public DomNode InsertBefore(DomNode other) {
            if (other == null)
                throw new ArgumentNullException("other");

            other.Before(this);
            return this;
        }

        public DomNode Append(params DomNode[] childNodes) {
            return Append((IEnumerable<DomNode>) childNodes);
        }

        public DomNode Append(IEnumerable<DomNode> childNodes) {
            if (childNodes == null)
                return this;

            foreach (var e in childNodes) {
                Append(e);
            }
            return this;
        }

        public DomNode Append(DomNode child) {
            if (child == null)
                return this;

            // TODO Relying on exceptions thrown from a method with different parameter names (ux)
            if (child.NodeType == DomNodeType.Attribute)
                this.Attributes.Add((DomAttribute) child);
            else {
                if (this.ChildNodes.IsReadOnly)
                    throw DomFailure.CannotAppendChildNode();

                this.ChildNodes.Add(child);
            }

            return this;
        }

        public DomNode Append(string text) {
            if (string.IsNullOrEmpty(text))
                return this;

            using (var stringReader = new StringReader(text)) {
                using (DomReader reader = this.OwnerDocument.ProviderFactory.CreateReader(stringReader)) {

                    var writer = this.OwnerDocument.ProviderFactory.CreateWriter(this, null);
                    reader.CopyTo(writer);
                }
            }

            return this;
        }

        public DomNode AppendTo(DomNode parent) {
            if (parent == null)
                throw new ArgumentNullException("parent");

            parent.Append(this);
            return this;
        }

        public DomElement AppendElement(string name) {
            var e = this.OwnerDocumentOrSelf.CreateElement(name);
            this.Append(e);
            return e;
        }

        public DomText PrependText(string data) {
            var e = this.OwnerDocumentOrSelf.CreateText(data);
            this.Prepend(e);
            return e;
        }

        public DomText AppendText(string data) {
            var e = this.OwnerDocumentOrSelf.CreateText(data);
            this.Append(e);
            return e;
        }

        public DomNode Remove() {
            return RemoveSelf();
        }

        public DomNode RemoveAttributes() {
            if (this.Attributes != null)
                this.Attributes.Clear();

            return this;
        }

        public DomNode RemoveSelf() {
            if (this.siblingsContent != null) {
                this.siblingsContent.Remove(this);
            }

            return this;
        }

        // TODO Verify that we can't replace certain types with others

        public DomNode ReplaceWith(DomNode other) {
            if (other == null)
                throw new ArgumentNullException("other");

            return ReplaceWithCore(other);
        }

        public DomNode ReplaceWith(params DomNode[] others) {
            if (others == null)
                throw new ArgumentNullException("others");

            var current = this;
            foreach (var m in others) {
                current.After(m);
                current = m;
            }

            RemoveSelf();
            return others[0];
        }

        public DomNode ReplaceWith(IEnumerable<DomNode> others) {
            if (others == null)
                throw new ArgumentNullException("others");

            return ReplaceWith(others.ToArray());
        }

        // TODO Replace with text
        public DomNode ReplaceWith(string markup) {
            throw new NotImplementedException();
        }

        protected virtual DomNode ReplaceWithCore(DomNode other) {
            if (other.NodeType == DomNodeType.Attribute)
                throw DomFailure.CannotReplaceWithAttribute("other");

            this.RequireParent().ChildNodes[this.NodePosition] = other;
            return other;
        }

        public DomNode Prepend(DomNode child) {
            throw new NotImplementedException();
        }

        // TODO DomNode.Prepend implementation

        public DomNode Prepend(string text) {
            throw new NotImplementedException();
        }

        public DomNode PrependTo(DomNode parent) {
            if (parent == null)
                throw new ArgumentNullException("parent");

            parent.Prepend(this);
            return this;
        }

        private DomNode AddSiblingHtml(DomWriter writer, string text) {
            if (string.IsNullOrEmpty(text))
                return this;

            using (var stringReader = new StringReader(text)) {
                using (DomReader reader = this.OwnerDocument.ProviderFactory.CreateReader(stringReader)) {
                    reader.CopyTo(writer);
                }
            }

            return this;
        }
    }
}
