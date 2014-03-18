//
// - DomDocumentFragment.cs -
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
using System.IO;
using System.Linq;

using Carbonfrost.Commons.Shared.Runtime;

namespace Carbonfrost.Commons.Web.Dom {

    public class DomDocumentFragment : DomContainer {

        public override DomNodeType NodeType {
            get {
                return DomNodeType.DocumentFragment;
            }
        }

        public override string NodeName {
            get {
                return "#fragment";
            }
        }

        protected override DomAttributeCollection DomAttributes {
            get {
                return null;
            }
        }

        internal override TResult AcceptVisitor<TArgument, TResult>(IDomNodeVisitor<TArgument, TResult> visitor, TArgument argument) {
            return visitor.Visit(this, argument);
        }

        internal override void AcceptVisitor(IDomNodeVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
