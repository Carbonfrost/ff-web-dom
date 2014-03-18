//
// - IDomNodeFactory.cs -
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

namespace Carbonfrost.Commons.Web.Dom {

    public interface IDomNodeFactory {

        DomAttribute CreateAttribute(string name);
        DomAttribute CreateAttribute(string name, IDomValue value);
        DomAttribute CreateAttribute(string name, string value);
        DomCDataSection CreateCData();
        DomCDataSection CreateCData(string data);
        DomComment CreateComment();
        DomComment CreateComment(string data);

        DomDocumentType CreateDocumentType(string name, string publicId, string systemId);
        DomElement CreateElement(string name);

        DomEntityReference CreateEntityReference(string data);
        DomProcessingInstruction CreateProcessingInstruction(string target);
        DomProcessingInstruction CreateProcessingInstruction(string target, string data);
        DomText CreateText();
        DomText CreateText(string data);

        Type GetAttributeNodeType(string name);
        Type GetCommentNodeType(string name);
        Type GetElementNodeType(string name);
        Type GetEntityReferenceNodeType(string name);
        Type GetEntityNodeType(string name);
        Type GetNotationNodeType(string name);
        Type GetProcessingInstructionNodeType(string name);
        Type GetTextNodeType(string name);
    }
}
