//
// - DomAttribute.cs -
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
using System.Net;
using System.Text;
using Carbonfrost.Commons.Shared;

namespace Carbonfrost.Commons.Web.Dom {

    [Serializable]
    public class DomAttribute : DomNode, IEquatable<DomAttribute> {

        private readonly string name;

        public override DomNode ParentNode {
            get {
                return null; // per spec
            }
        }

        public DomElement OwnerElement {
            get {
                return (this.SiblingAttributes == null)
                    ? null
                    : this.SiblingAttributes.OwnerElement;
            }
        }

        public DomAttribute PreviousAttribute {
            get {
                if (this.SiblingAttributes == null)
                    return null;
                if (this.NodePosition == 0)
                    return null;
                else
                    return this.SiblingAttributes[this.NodePosition - 1];
            }
        }

        public DomAttribute NextAttribute {
            get {
                if (this.SiblingAttributes == null)
                    return null;
                if (this.NodePosition == this.SiblingAttributes.Count - 1)
                    return null;
                else
                    return this.SiblingAttributes[this.NodePosition + 1];
            }
        }

        protected override DomNodeCollection DomChildNodes {
            get {
                return DomNodeCollection.Empty;
            }
        }

        protected override DomAttributeCollection DomAttributes {
            get {
                return null;
            }
        }

        public string Name {
            get {
                return this.name;
            }
        }

        public override string NodeName { get { return "#attribute"; } }

        public override DomNodeType NodeType {
            get { return DomNodeType.Attribute; } }

        public override string TextContent {
            get { return this.Value; }
            set { this.Value = value; } }

        internal IDomValue DomValue {
            get {
                return (IDomValue) this.content;
            }
            set {
                this.content = value;
                value.Initialize(this);

                // TODO Should dispose old?  (IDomValue shouldn't be disposable  though)
            }
        }

        public string Value {
            get { return DomValue.Value; }
            set { DomValue.Value = value; }
        }

        protected internal DomAttribute(string name) {
            if (name == null)
                throw new ArgumentNullException("name");

            if (name.Length == 0)
                throw Failure.EmptyString("name");

            this.name = name.Trim();
            this.content = new BasicDomValue();
        }

        public new DomAttribute Clone() {
            return (DomAttribute) base.Clone();
        }

        protected override DomNode CloneCore() {
            var result = this.OwnerDocument.CreateAttribute(this.Name,
                                                            this.Value);
            result.DomValue = (IDomValue) this.DomValue.Clone();
            return result;
        }

        public new DomAttribute RemoveSelf() {
            return (DomAttribute) base.RemoveSelf();
        }

        protected override DomNode ReplaceWithCore(DomNode other) {
            if (other == null)
                throw new ArgumentNullException("other");

            if (other.IsAttribute) {
                var oldParent = this.OwnerElement;
                this.RemoveSelf();
                oldParent.Attributes.Add((DomAttribute) other);
                return other;

            } else
                throw DomFailure.CanReplaceOnlyWithAttribute("other");
        }

        internal DomAttribute SetTypedValue(object value) {
            IDomValue dv = value as IDomValue;
            if (dv == null)
                this.DomValue.Value = Convert.ToString(value);
            else
                this.DomValue = dv;

            return this;
        }

        internal override void AcceptVisitor(IDomNodeVisitor visitor) {
            visitor.Visit(this);
        }

        internal override TResult AcceptVisitor<TArgument, TResult>(IDomNodeVisitor<TArgument, TResult> visitor, TArgument argument) {
            return visitor.Visit(this, argument);
        }

        public bool Equals(DomAttribute other) {
            return StaticEquals(this, other);
        }

        public override bool Equals(object obj) {
            return StaticEquals(this, obj as DomAttribute);
        }

        public override int GetHashCode() {
            int hashCode = 0;
            unchecked {
                hashCode += 37 * this.name.GetHashCode();
            }
            return hashCode;
        }

        private static bool StaticEquals(DomAttribute lhs, DomAttribute rhs) {
            if (object.ReferenceEquals(lhs, rhs))
                return true;

            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null))
                return false;

            return lhs.name == rhs.name && lhs.content == rhs.content;
        }

        public override string ToString() {
            return string.Format("{0}={1}{2}{1}",
                                 this.name,
                                 '"',
                                 WebUtility.HtmlEncode(this.Value));
        }

        sealed class BasicDomValue : IDomValue {

            void IDomValue.Initialize(DomAttribute attribute) {}

            public bool IsReadOnly {
                get {
                    return false;
                }
            }

            public string Value { get; set; }

            object ICloneable.Clone() {
                return new BasicDomValue { Value = this.Value };
            }
        }
    }
}
