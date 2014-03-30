//
// - DomElement.cs -
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
using System.Linq;
using System.ComponentModel;
using System.Text;
using Carbonfrost.Commons.Shared;

namespace Carbonfrost.Commons.Web.Dom {

    [Serializable]
    public class DomElement : DomContainer {
        
        private readonly DomAttributeCollection attributes;
        private readonly string name;

        public bool HasElements {
            get {
                return this.Elements.Any();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Id {
            get {
                return Attribute("id");
            }
            set {
                Attribute("id", value);
            }
        }

        protected override DomAttributeCollection DomAttributes {
            get {
                return this.attributes;
            }
        }

        public DomAttribute FirstAttribute {
            get {
                if (this.attributes.Count == 0)
                    return null;
                else
                    return this.attributes[0];
            }
        }

        public DomAttribute LastAttribute {
            get {
                if (this.attributes.Count == 0)
                    return null;
                else
                    return this.attributes[attributes.Count - 1];
            }
        }

        // TODO Set up element position

        public DomElement PreviousElementSibling {
            get {
                var answer = this.PreviousSibling;
                while (answer != null) {

                    if (answer.IsElement)
                        return (DomElement) answer;

                    answer = this.PreviousSibling;
                }

                return null;
            }
        }

        public DomElement NextElementSibling {
            get {
                var answer = this.NextSibling;
                while (answer != null) {

                    if (answer.IsElement)
                        return (DomElement) answer;

                    answer = this.NextSibling;
                }

                return null;
            }
        }

        // TODO Memoize this lookup (performance)

        public int ElementPosition {
            get {
                if (this.ParentElement == null)
                    return -1;

                int index = 0;
                foreach (var e in ParentElement.Elements) {
                    if (e == this)
                        return index;
                    index++;
                }

                return -1;
            }
        }

        public string Name {
            get {
                return name;
            }
        }
        
        internal string OwnText {
            get {
                // TODO Technically, should use Html StringUtil.GetOwnText, which
                // needs to be added here to preserve ws rules
                return this.InnerText;
            }
        }

        protected internal DomElement(string name) {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Length == 0)
                throw Failure.EmptyString("name");

            this.name = name;
            this.attributes = new DomAttributeCollection(this);
        }

        public override IEnumerable<DomElement> DescendantsAndSelf {
            get {
                return Utility.Cons(this, base.Descendants);
            }
        }

        public override string NodeName {
            get {
                return this.name;
            }
        }

        public DomElementDefinition ElementDefinition {
            get {
                return this.OwnerDocument.GetElementDefinition(this.Name);
            }
        }

        public new DomElement Clone() {
            DomElement result = OwnerDocument.CreateElement(this.Name);
            foreach (var m in this.Attributes)
                result.Attributes.Add(m.Clone());
            foreach (var m in this.ChildNodes)
                result.Append(m.Clone());

            return result;
        }

        public new DomElement Empty() {
            return (DomElement) base.Empty();
        }

        public new DomElement Attribute(string name, object value) {
            return (DomElement) base.Attribute(name, value);
        }

        public new DomElement Append(DomNode child) {
            return (DomElement) base.Append(child);
        }

        public new DomElement RemoveAttribute(string name) {
            return (DomElement) base.RemoveAttribute(name);
        }

        public new DomElement RemoveSelf() {
            return (DomElement) base.RemoveSelf();
        }

        public new DomElement Remove() {
            return (DomElement) base.Remove();
        }

        internal override void AcceptVisitor(IDomNodeVisitor visitor) {
            visitor.Visit(this);
        }

        internal override TResult AcceptVisitor<TArgument, TResult>(IDomNodeVisitor<TArgument, TResult> visitor, TArgument argument) {
            return visitor.Visit(this, argument);
        }

        public override DomNodeType NodeType {
            get { return DomNodeType.Element; }
        }

    }
}

