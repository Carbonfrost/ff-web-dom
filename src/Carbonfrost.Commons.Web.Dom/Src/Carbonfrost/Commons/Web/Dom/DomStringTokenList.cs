//
// - DomStringTokenList.cs -
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

using Carbonfrost.Commons.Shared;

namespace Carbonfrost.Commons.Web.Dom {

    [TypeConverter(typeof(DomStringTokenListConverter))]
    public class DomStringTokenList : IList<string>, IMakeReadOnly, IDomValue {

        private IList<string> items = new List<string>();

        public string Value {
            get {
                return ToString();
            }
            set {
                this.items.Clear();
                AddRange(ParseItems(value));
            }
        }

        public DomStringTokenList() {}

        public DomStringTokenList(IEnumerable<string> items) {
            if (items == null)
                return;
            if (items.Any(t => Regex.IsMatch(t, @"\s")))
                throw DomFailure.NoItemCanContainWhitespace("items");

            AddRange(items);
        }

        public static DomStringTokenList Parse(string text) {
            return Utility.Parse<DomStringTokenList>(text, _TryParse);
        }

        public static bool TryParse(string text, out DomStringTokenList result) {
            return _TryParse(text, out result) == null;
        }

        static Exception _TryParse(string text, out DomStringTokenList result) {
            result = null;

            if (text == null)
                return null;

            text = text.Trim();
            if (text.Length == 0)
                return null;

            result = new DomStringTokenList(ParseItems(text));
            return null;
        }

        static string[] ParseItems(string text) {
            return Regex.Split(text, "\\s+");
        }

        public static implicit operator DomStringTokenList(string text) {
            return Parse(text);
        }

        public string this[int index] {
            get {
                return this.items[index];
            }
            set {
                this.items[index] = value;
            }
        }

        public bool Add(string item) {
            if (this.GetIndexOfToken(item) < 0) {
                this.items.Add(item);
                return true;
            }
            return false;
        }

        public void Clear() {
            this.items.Clear();
        }

        public bool Contains(string item) {
            return this.items.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex) {
            this.items.CopyTo(array, arrayIndex);
        }

        private int GetIndexOfToken(string item) {
            if (item == null)
                throw new ArgumentNullException("item");

            if (item.Length == 0)
                throw Failure.EmptyString("className");

            if (Regex.IsMatch(item, @"\s"))
                throw DomFailure.CannotContainWhitespace("className");

            return this.items.IndexOf(item);
        }

        public IEnumerator<string> GetEnumerator() {
            return this.items.GetEnumerator();
        }

        public int IndexOf(string item) {
            return this.GetIndexOfToken(item);
        }

        public void Insert(int index, string item) {
            this.items.Insert(index, item);
        }

        public bool Remove(string item) {
            int index = this.GetIndexOfToken(item);
            if (index >= 0) {
                this.items.RemoveAt(index);
                return true;
            }

            return false;
        }

        public void RemoveAt(int index) {
            this.items.RemoveAt(index);
        }

        void ICollection<string>.Add(string item) {
            this.Add(item);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public void Toggle(string item) {
            if (this.GetIndexOfToken(item) < 0) {
                this.Add(item);

            } else {
                this.Remove(item);
            }
        }

        public override string ToString() {
            return string.Join(" ", this.items);
        }

        internal virtual void UpdateValue() {
        }

        object ICloneable.Clone() {
            return Clone();
        }

        public DomStringTokenList Clone() {
            return new DomStringTokenList(this.items);
        }

        public int Count {
            get { return this.items.Count; } }


        public bool IsReadOnly { get; private set; }

        public void MakeReadOnly() {
            this.IsReadOnly = true;
            this.items = new ReadOnlyCollection<string>(this.items.ToArray());
        }

        private void AddRange(IEnumerable<string> all) {
            if (this.IsReadOnly)
                throw Failure.ReadOnlyCollection();

            foreach (var e in all) {
                this.items.Add(e);
            }
        }

        void IDomValue.Initialize(DomAttribute attribute) {}
    }

}
