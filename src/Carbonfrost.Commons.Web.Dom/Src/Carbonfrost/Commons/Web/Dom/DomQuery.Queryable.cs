//
// - DomQuery.Queryable.cs -
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Carbonfrost.Commons.Shared;

namespace Carbonfrost.Commons.Web.Dom {

    public partial class DomQuery : IQueryable<DomNode> {
        
        // TODO Support Dom Query - probably sufficient just to use Enumerable base
        
        Expression IQueryable.Expression {
            get {
                throw new NotImplementedException();
            }
        }

        Type IQueryable.ElementType {
            get {
                throw new NotImplementedException();
            }
        }

        IQueryProvider IQueryable.Provider {
            get {
                throw new NotImplementedException();
            }
        }

        public IEnumerator<DomNode> GetEnumerator() {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}


