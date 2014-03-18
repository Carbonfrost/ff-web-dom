//
// - DomWriter.cs -
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
using Carbonfrost.Commons.Shared.Runtime;

namespace Carbonfrost.Commons.Web.Dom {

    public abstract class DomWriter : DisposableObject {

        public void Close() {
            Dispose(true);
        }

        public abstract void WriteStartElement(string name, string namespaceUri);
        public abstract void WriteStartAttribute(string name, string namespaceUri);
        public abstract void WriteEndAttribute();

        public abstract void WriteValue(string value);

        public abstract void WriteEndDocument();

        public abstract void WriteDocumentType(string name, string publicId, string systemId);

        public abstract void WriteEntityReference();
        public abstract void WriteProcessingInstruction(string target, string data);

        public abstract void WriteNotation();

        public abstract void WriteComment(string data);
        public abstract void WriteCDataSection(string data);
        public abstract void WriteText(string data);

        public abstract void WriteStartDocument();

        public abstract void WriteEndElement();
    }

}
