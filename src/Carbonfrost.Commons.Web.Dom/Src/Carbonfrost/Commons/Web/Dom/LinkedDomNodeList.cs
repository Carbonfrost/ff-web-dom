//
// - LinkedDomNodeList.cs -
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Shared;

namespace Carbonfrost.Commons.Web.Dom {

    class LinkedDomNodeList : IList<DomNode> {

        private int version;
        private int count;
        private readonly DomNode head;

        [NonSerialized]
        private DomNode cache;

        public int IndexOf(DomNode item) {
            if (item == null)
                throw new ArgumentNullException("item");

            // TODO Might not be in this list
            return item.NodePosition;
        }

        public void Insert(int index, DomNode item) {
            if (index < 0 || index >= this.Count)
                throw Failure.IndexOutOfRange("index", index, 0, Count - 1);
            if (item == null)
                throw new ArgumentNullException("item");

            InsertAfter(GetNodeAt(index), item);
        }

        public void RemoveAt(int index) {
            throw new NotImplementedException();
        }

        public DomNode this[int index] {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public void Add(DomNode item) {
            throw new NotImplementedException();
        }

        public void Clear() {
            throw new NotImplementedException();
        }

        public bool Contains(DomNode item) {
            throw new NotImplementedException();
        }

        public void CopyTo(DomNode[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public bool Remove(DomNode item) {
            throw new NotImplementedException();
        }

        public int Count {
            get {
                return count;
            }
        }
        public bool IsReadOnly {
            get {
                return false;
            }
        }

        public IEnumerator<DomNode> GetEnumerator() {
            return new Enumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public struct Enumerator : IEnumerator<DomNode> {

            private readonly LinkedDomNodeList list;
            private DomNode node;
            private readonly int version;
            private int index;

            internal Enumerator(LinkedDomNodeList list) {
                this.list = list;
                this.version = list.version;
                this.node = list.head;
                this.index = 0x0;
            }

            public DomNode Current {
                get {
                    if (index == 0 || index == list.Count)
                        throw Failure.CollectionModified();

                    return this.node;
                }
            }

            object IEnumerator.Current {
                get {
                    return Current;
                }
            }

            public bool MoveNext() {
                if (this.version != this.list.version)
                    throw Failure.CollectionModified();

                if (this.node == null) {
                    this.index = this.list.Count + 0x1;
                    return false;
                }

                this.index++;
                this.node = this.node.next;
                if (this.node == this.list.head) {
                    this.node = null;
                }

                return true;
            }

            void IEnumerator.Reset() {
                if (this.version != this.list.version)
                    throw Failure.CollectionModified();

                this.node = this.list.head;
                this.index = 0x0;
            }

            public void Dispose() {}

        }


        void InsertAfter(DomNode which, DomNode item) {
            throw new NotImplementedException();
        }

        DomNode GetNodeAt(int index) {
            return null;
        }

    }
}


