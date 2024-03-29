//
// - DomElementDefinition.cs -
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

namespace Carbonfrost.Commons.Web.Dom {

    [Serializable]
    public class DomElementDefinition : DomNodeDefinition {

        private DomWhitespaceMode whitespaceMode;
        private bool isEmpty;
        private bool isSelfClosing;

        public DomWhitespaceMode WhitespaceMode {
            get {
                return whitespaceMode;
            }
            set {
                ThrowIfReadOnly();
                whitespaceMode = value;
            }
        }

        public bool IsEmpty {
            get {
                return isEmpty;
            }
            set {
                ThrowIfReadOnly();
                isEmpty = value;
            }
        }

        public bool IsSelfClosing {
            get {
                return isSelfClosing;
            }
            set {
                ThrowIfReadOnly();
                isSelfClosing = value;
            }
        }

        public bool IsSelfClosingOrEmpty {
            get {
                return IsEmpty || IsSelfClosing;
            }
        }

        public bool PreserveWhitespace {
            get {
                return WhitespaceMode == DomWhitespaceMode.Preserve;
            }
        }

        protected internal DomElementDefinition(string name) : base(name)
        {
        }
    }
}
