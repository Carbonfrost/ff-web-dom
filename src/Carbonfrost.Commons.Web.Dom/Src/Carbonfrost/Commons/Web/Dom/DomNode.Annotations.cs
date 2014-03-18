//
// - DomNode.Annotations.cs -
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
using System.Linq;

namespace Carbonfrost.Commons.Web.Dom {

    partial class DomNode {

        private AnnotationList annotations = AnnotationList.Empty;

        public void AddAnnotation(object annotation) {
            if (annotation == null)
                throw new ArgumentNullException("annotation");

            this.annotations = this.annotations.Add(annotation);
        }

        public bool HasAnnotation<T>() where T : class {
            return annotations.OfType<T>().Any();
        }

        public bool HasAnnotation(object instance) {
            return annotations.Contains(instance);
        }

        public T Annotation<T>() where T : class {
            return annotations.OfType<T>().FirstOrDefault();
        }

        public object Annotation(Type type) {
            if (type == null)
                throw new ArgumentNullException("type");

            return annotations.OfType(type).FirstOrDefault();
        }

        public IEnumerable<T> Annotations<T>() where T : class {
            return annotations.OfType<T>();
        }

        public IEnumerable<object> Annotations(Type type) {
            return annotations.OfType(type);
        }

        public void RemoveAnnotations<T>() where T : class {
            this.annotations = this.annotations.RemoveOfType(typeof(T));
        }

        public void RemoveAnnotations(Type type) {
            if (type == null)
                throw new ArgumentNullException("type");

            this.annotations = this.annotations.RemoveOfType(type);
        }

        public void RemoveAnnotation(object value) {
            if (value == null)
                throw new ArgumentNullException("value");

            this.annotations = this.annotations.Remove(value);
        }
    }
}
