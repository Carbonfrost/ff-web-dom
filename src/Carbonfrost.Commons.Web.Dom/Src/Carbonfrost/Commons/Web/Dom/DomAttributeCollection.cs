//
// - DomAttributeCollection.cs -
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

using Carbonfrost.Commons.Shared;

namespace Carbonfrost.Commons.Web.Dom {

    public class DomAttributeCollection : IList<DomAttribute>, IReadOnlyList<DomAttribute>, IDomNodeCollection {

        internal static readonly DomAttributeCollection ReadOnly = new DomAttributeCollection(null, Empty<DomAttribute>.List);

        private readonly DomElement owner;
        private readonly IDictionary<string, DomAttribute> map;
        private readonly IList<DomAttribute> items;

        protected internal DomElement OwnerElement {
            get { return owner; }
        }

        protected IList<DomAttribute> Items {
            get {
                return this.items;
            }
        }

        public DomAttributeCollection(DomElement owner)
            : this(owner, new List<DomAttribute>()) {
        }

        public DomAttributeCollection(DomElement owner, IList<DomAttribute> items) {
            if (items == null)
                throw new ArgumentNullException("items");

            this.owner = owner;
            this.items = items;
            this.map = new Dictionary<string, DomAttribute>();
        }

        public virtual bool IsReadOnly {
            get {
                return Items.IsReadOnly;
            }
        }

        public string this[string name] {
            get {
                DomAttribute result;
                if (TryGetValue(RequireName(name), out result))
                    return result.Value;
                else
                    return null;
            }
            set {
                int index = IndexOf(RequireName(name));
                if (index < 0) {
                    var attr = this.OwnerElement.OwnerDocument.CreateAttribute(RequireName(name), value);
                    Items.Add(attr);
                } else
                    Items[index].Value = value;
            }
        }

        public DomAttribute GetByName(string name) {
            DomAttribute result;
            if (TryGetValue(RequireName(name), out result)) {
                return result;
            } else {
                return null;
            }
        }

        public DomAttribute GetOrAdd(string name) {
            return GetByNameOrCreate(name);
        }

        public DomAttribute AddNew(string name) {
            if (Contains(name))
                throw DomFailure.AttributeWithGivenNameExists(name, "name");

            var attr = OwnerElement.OwnerDocument.CreateAttribute(name);
            Add(attr);
            return attr;
        }

        public bool Remove(string name) {
            DomAttribute attr;

            if (TryGetValue(RequireName(name), out attr)) {
                RemoveAt(IndexOf(attr));
                return true;
            }

            return false;
        }

        public int IndexOf(string name) {
            RequireName(name);

            for (int i = 0; i < Items.Count; i++) {
                if (Items[i].Name == name)
                    return i;
            }
            return -1;
        }

        internal DomAttribute GetByNameOrCreate(string name) {
            var attr = GetByName(name);
            if (attr == null) {
                attr = OwnerElement.OwnerDocument.CreateAttribute(name);
                Add(attr);
            }

            return attr;
        }

        internal bool TryGetValue(string name, out DomAttribute result) {
            return map.TryGetValue(name, out result);
        }

        public virtual void InsertRange(int index, IEnumerable<DomAttribute> items) {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var element in items) {
                Insert(index++, element);
            }
        }

        public virtual bool Contains(string name) {
            return IndexOf(name) >= 0;
        }

        static DomAttribute RequireAttribute(DomNode item) {
            if (item == null)
                throw new ArgumentNullException("item");

            DomAttribute node = item as DomAttribute;
            if (node == null)
                throw Failure.NotInstanceOf("item", item, typeof(DomAttribute));
            return node;
        }

        internal static string RequireName(string name) {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Length == 0)
                throw Failure.EmptyString("name");

            return name;
        }

        // TODO Comparer should be by name

        void IDomNodeCollection.UnsafeRemove(DomNode node) {
            var attr = node as DomAttribute;
            if (attr != null)
                this.Items.Remove(attr);
        }

        bool IDomNodeCollection.Remove(DomNode node) {
            var attr = node as DomAttribute;
            if (attr != null && this.Items.Remove(attr)) {
                attr.Unlinked();
                return true;
            }
            return false;
        }

        DomNode IDomNodeCollection.OwnerNode {
            get {
                return this.OwnerElement;
            }
        }

        private void InsertItem(int index, DomAttribute item) {
            int existing = IndexOf(item.Name);

            if (existing < 0) {

            } else if (this.Items[existing] == item)
                return;
            else
                throw DomFailure.AttributeWithGivenNameExists(item.Name, "item");

            Entering(item, index);
            this.map.Add(item.Name, item);
            this.Items.Insert(index, item);
        }

        private void ClearItems() {
            foreach (var m in Items)
                m.Unlinked();

            this.map.Clear();
            this.items.Clear();
        }

        private void RemoveItem(int index) {
            var m = Items[index];

            m.Unlinked();
            this.map.Remove(m.Name);
            this.items.RemoveAt(index);
        }

        private void SetItem(int index, DomAttribute item) {
            var old = Items[index];
            Leaving(old);
            Entering(item, index);
            this.map.Add(item.Name, item);
            this.map.Remove(Items[index].Name);
            this.items[index] = item;
        }

        private void Entering(DomNode item, int index) {
            item.SetSiblingNodes(this, index);
            Reindex(index);
        }

        private void Leaving(DomNode item) {
            int old = item.NodePosition;
            item.SetSiblingNodes((DomNodeCollection) null, -1);
            Reindex(old);
        }

        private void Reindex(int pos) {
            for (int i = pos; i < Items.Count; i++) {
                this.Items[i].SetSiblingIndex(i);
            }
        }

        public int IndexOf(DomAttribute item) {
            if (item == null)
                throw new ArgumentNullException("item");

            if (item.SiblingAttributes != this)
                return -1;
            if (Items[item.NodePosition] == item)
                return item.NodePosition;
            else
                return this.Items.IndexOf(item);
        }

        public void Insert(int index, DomAttribute item) {
            if (item == null)
                throw new ArgumentNullException("item");

            InsertItem(index, item);
        }

        public void RemoveAt(int index) {
            if (index < 0 || index >= Count)
                throw Failure.IndexOutOfRange("index", index, 0, Count - 1);

            RemoveItem(index);
        }

        public DomAttribute this[int index] {
            get {
                return items[index];
            }
            set {
                if (value == null)
                    throw new ArgumentNullException("value");

                SetItem(index, value);
            }
        }

        public void Add(DomAttribute item) {
            if (item == null)
                throw new ArgumentNullException("item");

            InsertItem(Count, item);
        }

        public void Clear() {
            ClearItems();
        }

        public bool Contains(DomAttribute item) {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(DomAttribute[] array, int arrayIndex) {
            Items.CopyTo(array, arrayIndex);
        }

        public bool Remove(DomAttribute item) {
            if (item == null)
                throw new ArgumentNullException("item");

            int index = IndexOf(item);
            bool bounds = index >= 0;
            if (bounds)
                RemoveItem(index);

            return bounds;
        }

        public int Count {
            get {
                return items.Count;
            }
        }

        public IEnumerator<DomAttribute> GetEnumerator() {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

}
