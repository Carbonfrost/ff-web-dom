//
// - Utility.cs -
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
using Carbonfrost.Commons.Shared.Runtime.Components;

namespace Carbonfrost.Commons.Web.Dom {

    static class Utility {

        public static readonly ComponentCollection EmptyComponents = new ComponentCollection();

        static Utility() {
            EmptyComponents.MakeReadOnly();
        }

        internal delegate Exception TryParser<T>(string text, out T result);

        public static T Parse<T>(string text, TryParser<T> _TryParse) {
            T result;
            Exception ex = _TryParse(text, out result);
            if (ex == null)
                return result;
            else
                throw ex;
        }

        public static IEnumerable<T> Cons<T>(T item, IEnumerable<T> other) {
            return Enumerable.Concat(new T[] { item }, other);
        }

    }
}
