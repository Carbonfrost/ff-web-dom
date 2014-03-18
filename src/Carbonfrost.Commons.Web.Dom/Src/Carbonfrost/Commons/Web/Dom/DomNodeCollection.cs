//
// - DomNodeCollection.cs -
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
using System.Collections.ObjectModel;
using System.Linq;

namespace Carbonfrost.Commons.Web.Dom {

    public class DomNodeCollection : Collection<DomNode>, IReadOnlyList<DomNode>, IDomNodeCollection {

        internal static readonly DomNodeCollection Empty = new DomNodeCollection(null, Empty<DomNode>.List);

        private readonly DomNode owner;

        protected internal DomNode OwnerNode {
            get { return owner; }
        }

        public virtual bool IsReadOnly {
            get {
                return false;
            }
        }

        internal IList<DomNode> UnsafeItems {
            get {
                return this.Items;
            }
        }

        public DomNodeCollection(DomNode ownerNode, IList<DomNode> items) : base(items) {
            this.owner = ownerNode;
        }

        public DomNodeCollection(DomNode ownerNode) : base(new List<DomNode>()) {
            this.owner = ownerNode;
        }

        internal DomNode GetNextSibling(DomNode other) {
            if (this.Items is LinkedDomNodeList)
                return other.next;

            var index = IndexOf(other) + 1;
            if (index >= Count)
                return null;
            else
                return Items[index];
        }

        internal DomNode GetPreviousSibling(DomNode other) {
            // TODO Faster computation of previous sibling for linked
            var index = IndexOf(other) - 1;
            if (index < 0)
                return null;
            else
                return Items[index];
        }

        public virtual void InsertRange(int index, IEnumerable<DomNode> items) {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var element in items) {
                Insert(index++, element);
            }
        }

        public virtual void AddRange(IEnumerable<DomNode> items) {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var element in items) {
                Add(element);
            }
        }

        DomNode IDomNodeCollection.OwnerNode {
            get {
                return this.OwnerNode;
            }
        }

        void IDomNodeCollection.UnsafeRemove(DomNode node) {
            this.Items.Remove(node);
        }

        bool IDomNodeCollection.Remove(DomNode node) {
            return Remove(node);
        }

        protected override void InsertItem(int index, DomNode item) {
            Entering(item, index);
            base.InsertItem(index, item);
        }

        protected override void ClearItems() {
            foreach (var m in Items)
                m.Unlinked();

            base.ClearItems();
        }

        protected override void RemoveItem(int index) {
            Items[index].Unlinked();
            base.RemoveItem(index);
            Reindex(index);
        }

        protected override void SetItem(int index, DomNode item) {
            var old = Items[index];
            Items[index] = item;
            old.Unlinked();
            Entering(item, index);
            base.SetItem(index, item);
        }

        private void Entering(DomNode item, int index) {
            item.SetSiblingNodes(this, index);
            Reindex(index);
        }

        private void Reindex(int pos) {
            for (int i = pos; i < Items.Count; i++) {
                this.Items[i].SetSiblingIndex(i);
            }
        }

    }
}
