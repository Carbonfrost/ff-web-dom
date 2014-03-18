//
// - NullDomNodeFactory.cs -
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

    class NullDomNodeFactory : IDomNodeFactory {

        DomComment IDomNodeFactory.CreateComment() {
            return null;
        }

        DomText IDomNodeFactory.CreateText() {
            return null;
        }

        DomAttribute IDomNodeFactory.CreateAttribute(string name) {
            return null;
        }

        DomAttribute IDomNodeFactory.CreateAttribute(string name, IDomValue value) {
            return null;
        }

        DomAttribute IDomNodeFactory.CreateAttribute(string name, string value) {
            return null;
        }

        DomCDataSection IDomNodeFactory.CreateCData() {
            return null;
        }

        DomCDataSection IDomNodeFactory.CreateCData(string data) {
            return null;
        }

        DomComment IDomNodeFactory.CreateComment(string data) {
            return null;
        }

        DomDocumentType IDomNodeFactory.CreateDocumentType(string name, string publicId, string systemId) {
            return null;
        }

        DomElement IDomNodeFactory.CreateElement(string name) {
            return null;
        }

        DomEntityReference IDomNodeFactory.CreateEntityReference(string data) {
            return null;
        }

        DomProcessingInstruction IDomNodeFactory.CreateProcessingInstruction(string target) {
            return null;
        }

        DomProcessingInstruction IDomNodeFactory.CreateProcessingInstruction(string target, string data) {
            return null;
        }

        DomText IDomNodeFactory.CreateText(string data) {
            return null;
        }

        Type IDomNodeFactory.GetAttributeNodeType(string name) {
            return null;
        }

        Type IDomNodeFactory.GetCommentNodeType(string name) {
            return null;
        }

        Type IDomNodeFactory.GetElementNodeType(string name) {
            return null;
        }

        Type IDomNodeFactory.GetEntityReferenceNodeType(string name) {
            return null;
        }

        Type IDomNodeFactory.GetEntityNodeType(string name) {
            return null;
        }

        Type IDomNodeFactory.GetNotationNodeType(string name) {
            return null;
        }

        Type IDomNodeFactory.GetProcessingInstructionNodeType(string name) {
            return null;
        }

        Type IDomNodeFactory.GetTextNodeType(string name) {
            return null;
        }
    }
}


