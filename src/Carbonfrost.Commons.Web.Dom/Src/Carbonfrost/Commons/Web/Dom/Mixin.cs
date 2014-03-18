//
// - Mixin.cs -
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

namespace Carbonfrost.Commons.Web.Dom {

    static class Mixin {

        public static void AddRange<T>(this ICollection<T> self, IEnumerable<T> items) {
            foreach (var e in items) {
                self.Add(e);
            }
        }

        public static TValue GetValueOrCache<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TKey, TValue> func) {
            TValue value;
            if (source.TryGetValue(key, out value))
                return value;

            return (source[key] = func(key));
        }

        public static TValue FirstNonNull<T, TValue>(this IEnumerable<T> items, Func<T, TValue> func)
            where TValue : class
        {
            foreach (var m in items) {
                TValue result = func(m);
                if (result != null)
                    return result;
            }

            return null;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key) {
            TValue value;
            if (source.TryGetValue(key, out value))
                return value;

            return default(TValue);
        }

    }
}
