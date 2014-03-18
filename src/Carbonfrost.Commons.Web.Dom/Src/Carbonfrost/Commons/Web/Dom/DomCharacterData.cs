//
// - DomCharacterData.cs -
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

    public abstract class DomCharacterData : DomNode {

        public string Data {
            get {
                return (string) this.content;
            }
            set {
                this.content = value;
            }
        }

        public override string TextContent {
            get {
                return this.Data;
            }
            set {
                this.Data = value;
            }
        }

        protected internal DomCharacterData() {}

        public override string NodeValue {
            get {
                return Data;
            }
            set {
                Data = value;
            }
        }

        public void AppendData(string data) {
            // TODO Allow caching a StringBuilder to optimize adjacent calls (performance)
            this.Data += data;
        }

    }
}
