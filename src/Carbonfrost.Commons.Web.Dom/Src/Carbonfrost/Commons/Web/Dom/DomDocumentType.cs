//
// - DomDocumentType.cs -
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
using System.Linq;
using Carbonfrost.Commons.Shared;

namespace Carbonfrost.Commons.Web.Dom {

    [Serializable]
    public class DomDocumentType : DomNode {

        // TODO These should be read-only like w3c

        public string PublicId { get; set; }
        public string SystemId { get; set; }

        protected internal DomDocumentType(string name) {
            if (name == null)
                throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(name))
                throw Failure.EmptyString("name");

            this.Name = name;
        }

        public override DomNodeType NodeType {
            get { return DomNodeType.DocumentType; } }

        public string Name {
            get; private set;
        }

        public override string NodeName {
            get { return "#doctype"; } }

        protected override DomAttributeCollection DomAttributes {
            get {
                return null;
            }
        }

        protected override DomNodeCollection DomChildNodes {
            get {
                return DomNodeCollection.Empty;
            }
        }

        internal override void AcceptVisitor(IDomNodeVisitor visitor) {
            visitor.Visit(this);
        }

        internal override TResult AcceptVisitor<TArgument, TResult>(IDomNodeVisitor<TArgument, TResult> visitor, TArgument argument) {
            return visitor.Visit(this, argument);
        }

    }
}
