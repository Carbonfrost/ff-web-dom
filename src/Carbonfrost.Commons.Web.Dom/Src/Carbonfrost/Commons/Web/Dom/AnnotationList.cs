//
// - AnnotationList.cs -
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

    abstract class AnnotationList {

        public static readonly AnnotationList Empty = new EmptyAnnotationList();

        public abstract AnnotationList Add(object annotation);
        public abstract IEnumerable<T> OfType<T>() where T : class;
        public abstract IEnumerable<object> OfType(Type type);
        public abstract AnnotationList Remove(object annotation);
        public abstract AnnotationList RemoveOfType(Type type);
        public abstract bool Contains(object annotation);
    }

}
