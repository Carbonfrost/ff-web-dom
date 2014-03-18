//
// - DomContainer.cs -
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

namespace Carbonfrost.Commons.Web.Dom {

    // Facilitates treating DomDocument sort of like an element, but
    // really, all containers are elements

    public abstract partial class DomContainer : DomNode {

        private readonly DomNodeCollection domChildNodes;

        protected DomContainer() {
            // TODO Replace with linked list (performance)
            this.domChildNodes = new DomNodeCollection(this, new List<DomNode>());
        }

        protected override DomNodeCollection DomChildNodes {
            get {
                return domChildNodes;
            }
        }

        public virtual IEnumerable<DomElement> AncestorsAndSelf {
            get {
                return GetAncestorsCore(true);
            }
        }

        public IEnumerable<DomElement> Ancestors {
            get {
                return GetAncestorsCore(false);
            }
        }

        public virtual IEnumerable<DomElement> DescendantsAndSelf {
            get {
                return GetDescendantsCore(true);
            }
        }

        public IEnumerable<DomElement> Descendants {
            get {
                return GetDescendantsCore(false);
            }
        }

        public DomElement Descendant(string name) {
            return this.GetDescendants(name).FirstOrDefault();
        }

        public DomElement Descendant(string name, string xmlns) {
            return this.GetDescendants(name, xmlns).FirstOrDefault();
        }

        public IEnumerable<DomElement> GetDescendants(string name) {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Length == 0)
                throw Failure.EmptyString("name");

            return FilterByName(Descendants, name);
        }

        public IEnumerable<DomElement> GetDescendants(string name, string xmlns) {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Length == 0)
                throw Failure.EmptyString("name");

            return FilterByName(Descendants, name, xmlns);
        }

        public virtual IReadOnlyList<DomElement> Elements {
            get {
                return new Buffer<DomElement>(this.ChildNodes.Where(t => t.IsElement).Cast<DomElement>());
            }
        }

        public IEnumerable<DomElement> GetElements(string name) {
            return FilterByName(Elements, name, null);
        }

        public IEnumerable<DomElement> GetElements(string name, string xmlns) {
            return FilterByName(Elements, name, xmlns);
        }

        public DomElement Element(int index) {
            return Elements.ElementAtOrDefault(index);
        }

        public DomElement Element(string name) {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Length == 0)
                throw Failure.EmptyString("name");

            return Elements.FirstOrDefault(t => t.Name == name);
        }

        public DomElement Element(string name, string xmlns) {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Length == 0)
                throw Failure.EmptyString("name");

            return Elements.FirstOrDefault(t => t.Name == name && t.NamespaceUri == xmlns);
        }

        public bool IsEmpty {
            get {
                return this.ChildNodes.Count == 0;
            }
        }

        public sealed override bool IsContainer {
            get {
                return true;
            }
        }

        private static IEnumerable<DomElement> FilterByName(IEnumerable<DomElement> elements,
                                                            string name,
                                                            string xmlns = null) {
            return elements.Where(t => t.Name == name && t.NamespaceUri == xmlns);
        }

        public DomElement FirstChild {
            get {
                return Elements.FirstOrDefault();
            }
        }

        public DomElement LastChild {
            get {
                return Elements.LastOrDefault();
            }
        }

        public DomElement Child(int index) {
            return this.Elements.ElementAtOrDefault(index);
        }

        public override string ToString() {
            return this.OuterText;
        }

        // TODO Support cloning

        private IEnumerable<DomElement> GetDescendantsCore(bool self) {
            var queue = new Queue<DomElement>();
            var ele = this as DomElement;
            if (self && ele != null) {
                queue.Enqueue(ele);

            } else {
                this.QueueChildren(queue);
            }

            while (queue.Count > 0) {
                var result = queue.Dequeue();
                yield return result;
                result.QueueChildren(queue);
            }
        }

        private void QueueChildren(Queue<DomElement> queue) {
            foreach (var child in this.Elements)
                queue.Enqueue(child);
        }

        private IEnumerable<DomElement> GetAncestorsCore(bool self) {
            if (self && this is DomElement) {
                yield return (DomElement) this;
            }

            DomElement current = this.ParentElement;
            while (current != null) {
                yield return current;
                current = this.ParentElement;
            }
        }

    }

}
