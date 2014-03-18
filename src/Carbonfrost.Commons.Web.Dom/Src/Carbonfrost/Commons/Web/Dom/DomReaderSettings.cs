//
// - DomNodeReaderSettings.cs -
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
using Carbonfrost.Commons.Shared;

namespace Carbonfrost.Commons.Web.Dom {

    public class DomReaderSettings : ICloneable, IMakeReadOnly {

        public static readonly DomReaderSettings Empty;

        public bool IsReadOnly { get; private set; }

        static DomReaderSettings() {
            Empty = new DomReaderSettings();
            Empty.MakeReadOnly();
        }

        protected DomReaderSettings() {}

        object ICloneable.Clone() {
            return this.CloneCore();
        }

        public DomReaderSettings Clone() {
            return CloneCore();
        }

        protected virtual DomReaderSettings CloneCore() {
            return (DomReaderSettings) MemberwiseClone();
        }

        protected void ThrowIfReadOnly() {
            if (this.IsReadOnly)
                throw Failure.Sealed();
        }

        public void MakeReadOnly() {
            this.IsReadOnly = true;
        }
    }
}
