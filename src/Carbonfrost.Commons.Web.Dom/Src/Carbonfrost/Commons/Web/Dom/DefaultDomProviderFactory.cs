//
// - DefaultDomProviderFactory.cs -
//
// Copyright 2014 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.IO;
using System.Linq;

namespace Carbonfrost.Commons.Web.Dom {

    class DefaultDomProviderFactory : DomProviderFactory {

        // TODO Implement DefaultDomProviderFactory

        public override bool IsProviderObject(Type providerObjectType) {
            if (providerObjectType == null)
                throw new ArgumentNullException("providerObjectType");

            return providerObjectType.Assembly == typeof(DefaultDomProviderFactory).Assembly;
        }
    }
}
