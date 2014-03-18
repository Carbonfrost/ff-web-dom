//
// - DomNode.cs -
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
using System.ComponentModel;
using System.Linq;
using Carbonfrost.Commons.Shared;
using Carbonfrost.Commons.Web.Dom.Query;

namespace Carbonfrost.Commons.Web.Dom {

    public abstract partial class DomNode : ICloneable, IHierarchyObject, IHierarchyNavigable {

        private int siblingIndex;
        internal DomNode next;

        // Purely for the sake of reducing memory required by DomNode
        //   DomAttribute => IDomValue
        //   DomCharacterData => string
        //   DomContainer = > DomNode corresponding to the head
        internal object content;

        private IDomNodeCollection siblingsContent;

        internal DomAttributeCollection SiblingAttributes {
            get {
                return siblingsContent as DomAttributeCollection;
            }
        }

        private DomNodeCollection siblings {
            get {
                return siblingsContent as DomNodeCollection;
            }
        }


        public bool IsText {
            get {
                return this.NodeType == DomNodeType.Text;
            }
        }

        public bool IsCDataSection {
            get {
                return this.NodeType == DomNodeType.CDataSection;
            }
        }

        public bool IsCharacterData {
            get {
                return IsText || IsCDataSection;
            }
        }

        public bool IsElement {
            get {
                return this.NodeType == DomNodeType.Element;
            }
        }

        public virtual bool IsContainer {
            get {
                return false;
            }
        }

        public bool IsAttribute {
            get {
                return NodeType == DomNodeType.Attribute;
            }
        }

        protected DomNode() {
        }

        public DomAttribute AppendAttribute(string name, object value) {
            return this.Attributes.AddNew(name).SetTypedValue(value);
        }

        public string Attribute(string name) {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Length == 0)
                throw Failure.EmptyString("name");

            if (this.Attributes == null) {
                Traceables.IgnoredAttributes();
                return null;

            } else
                return this.Attributes[name];
        }

        public DomNode ChildNode(int index) {
            return this.ChildNodes[index];
        }

        public DomNode Clone() {
            return this.CloneCore();
        }

        protected virtual DomNode CloneCore() {
            DomNode clone = (DomNode) base.MemberwiseClone();
            this.OwnerDocument.UnlinkedNodes.Add(clone);
            return clone;
        }

        public DomNode Empty() {
            this.DomChildNodes.Clear();
            return this;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override bool Equals(object obj) {
            return object.ReferenceEquals(this, obj);
        }

        public bool HasAttribute(string name) {
            if (name == null)
                throw new ArgumentNullException("name");

            return this.Attributes.Contains(name);
        }

        public DomNode RemoveAttribute(string name) {
            this.Attributes.Remove(name);
            return this;
        }

        internal void SetSiblingNodes(IDomNodeCollection newSiblings, int index) {
            if (this.siblingsContent != null && newSiblings != this.siblingsContent) {
                var sc = this.siblingsContent;
                sc.UnsafeRemove(this);
            }

            this.siblingsContent = newSiblings;
            this.siblingIndex = index;
        }

        internal void Unlinked() {
            var unlinked = OwnerDocument.UnlinkedNodes;
            unlinked.UnsafeItems.Add(this);
            this.siblingsContent = unlinked;
            this.siblingIndex = -1;
        }

        internal void SetSiblingIndex(int value) {
            this.siblingIndex = value;
        }

        void AssertSiblings(IDomNodeCollection newSiblings) {
            if (this.siblingsContent != null && newSiblings != null && newSiblings != this.siblingsContent)
                throw new Exception("Already has siblings " + this.siblingsContent);
            this.siblingsContent = newSiblings;
        }

        public override string ToString() {
            return this.NodeValue;
        }

        internal DomNode Traverse(INodeVisitor nodeVisitor) {
            if (nodeVisitor == null)
                throw new ArgumentNullException("nodeVisitor");

            new NodeTraversor(nodeVisitor).Traverse(this);
            return this;
        }

        public bool HasAttributes {
            get {
                return this.DomAttributes != null;
            }
        }

        public DomAttributeCollection Attributes {
            get {
                return this.DomAttributes;
            }
        }

        protected abstract DomAttributeCollection DomAttributes {
            get;
        }

        public Uri BaseUri {
            get {
                var uc = this.Annotation<BaseUriAnnotation>();
                if (uc == null)
                    return ParentNode == null ? null: ParentNode.BaseUri;
                else
                    return uc.uri;
            }
            set {
                this.RemoveAnnotations<BaseUriAnnotation>();
                this.AddAnnotation(new BaseUriAnnotation(value));
            }
        }

        public DomNodeCollection ChildNodes {
            get {
                return this.DomChildNodes;
            }
        }

        protected abstract DomNodeCollection DomChildNodes { get; }

        public IEnumerable<DomNode> DescendantNodes {
            get {
                throw new NotImplementedException();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string InnerText {
            get {
                return null;
            }
            set
            {
            }
        }

        public DomNode NextSibling {
            get {
                if (this.ParentNode == null)
                    return null;
                else
                    return this.ParentNode.ChildNodes.GetNextSibling(this);
            }
        }

        public DomNode FirstChildNode {
            get {
                return this.ChildNodes.FirstOrDefault();
            }
        }

        public DomNode LastChildNode {
            get {
                return this.ChildNodes.LastOrDefault();
            }
        }

        public abstract string NodeName { get; }

        public virtual string LocalName {
            get {
                throw new NotImplementedException();
            }
        }

        public virtual string Prefix {
            get {
                return null;
            }
        }

        public virtual string NamespaceUri {
            get {
                return null;
            }
        }

        public int NodePosition {
            get { return this.siblingIndex; } }

        public abstract DomNodeType NodeType { get; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string NodeValue {
            get {
                return null;
            }
            set
            {
            }
        }

        public virtual string OuterText {
            get {
                return (new OuterTextVisitor()).ConvertToString(this);
            }
        }

        public DomDocument OwnerDocument {
            get {
                if (this.OwnerNode == null)
                    return null;

                if (this.OwnerNode.NodeType == DomNodeType.Document) {
                    return (DomDocument) this.OwnerNode;
                }

                return this.OwnerNode.OwnerDocument;
            }
        }

        private DomNode OwnerNode {
            get {
                if (this.SiblingAttributes != null)
                    return this.SiblingAttributes.OwnerElement;

                else if (this.siblings != null)
                    return siblings.OwnerNode;

                else
                    return null;
            }
        }

        public virtual DomNode ParentNode {
            get {
                var owner = OwnerNode;

                if (owner == null)
                    return null;
                else if (owner.NodeType == DomNodeType.Document && ((DomDocument) owner).UnlinkedNodes.Contains(this))
                    return null;
                else
                    return owner;
            }
        }

        public DomElement ParentElement {
            get {
                return ParentNode as DomElement;
            }
        }

        public DomNode PreviousSibling {
            get {
                if (this.ParentNode == null)
                    return null;
                else
                    return this.ParentNode.ChildNodes.GetPreviousSibling(this);
            }
        }

        public IReadOnlyList<DomNode> SiblingNodes {
            get {
                if (this.ParentNode == null)
                    return Empty<DomNode>.Array;

                return this.siblings;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string TextContent {
            get {
                return null;
            }
            set
            {
            }
        }

        private DomNode RequireParent() {
            if (this.ParentNode == null)
                throw DomFailure.ParentNodeRequired();

            return this.ParentNode;
        }

        object ICloneable.Clone()  {
            return Clone();
        }

        internal abstract void AcceptVisitor(IDomNodeVisitor visitor);
        internal abstract TResult AcceptVisitor<TArgument, TResult>(IDomNodeVisitor<TArgument, TResult> visitor, TArgument argument);

        public DomQuery Select(string cssQuery) {
            // TODO Revisit semantics of selecting on attributes, text, etc.
            DomElement e = this as DomElement;
            if (e == null)
                return DomQuery.Empty;
            else
                return new CssSelector(cssQuery, e).Select();
        }

        IHierarchyObject IHierarchyObject.ParentObject {
            get {
                return this.ParentNode;
            }
            set {
                throw Failure.ReadOnlyProperty();
            }
        }

        IEnumerable<IHierarchyObject> IHierarchyObject.ChildrenObjects {
            get {
                return this.ChildNodes;
            }
        }

        IHierarchyNavigable IHierarchyNavigable.SelectChild(string name) {
            throw new NotImplementedException();
        }

        IHierarchyNavigable IHierarchyNavigable.SelectChild(int index) {
            throw new NotImplementedException();
        }

        private IEnumerable<DomNode> GetDescendantsNodesCore(bool self) {
            Queue<DomNode> queue = new Queue<DomNode>();
            if (self) {
                queue.Enqueue(this);
            }

            while (queue.Count > 0) {
                var result = queue.Dequeue();
                yield return result;

                foreach (var child in result.ChildNodes) {
                    queue.Enqueue(child);
                }
            }
        }

        private class BaseUriAnnotation {

            public readonly Uri uri;

            public BaseUriAnnotation(Uri uri) {
                this.uri = uri;
            }

        }

    }
}
