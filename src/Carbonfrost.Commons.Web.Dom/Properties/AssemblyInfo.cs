//
// - AssemblyInfo.cs -
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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Carbonfrost.Commons.ComponentModel.Annotations;
using Carbonfrost.Commons.Shared.Runtime;
using Carbonfrost.Commons.Web.Dom;

[assembly: AssemblyTitle("Carbonfrost.Commons.Web.Dom")]
[assembly: AssemblyDescription("Carbonfrost.Commons.Web.Dom")]

[assembly: Provides(typeof(IDomNodeFactory))]
[assembly: Provides(typeof(DomProviderFactory))]

#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#endif

[assembly: AssemblyCompany("Carbonfrost Systems, Inc. and contributors.")]
[assembly: AssemblyProduct("The F5 Project")]
[assembly: AssemblyCopyright("Copyright (c) 2013 Carbonfrost Systems, Inc. and contributors")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]

[assembly: InternalsVisibleTo("Carbonfrost.Commons.Web.Dom.Test, PublicKey=00240000048000009400000006020000002400005253413100040000010001005D816AF902913A3381795785638DC3E2B9CB19D83EC2AAD8764E215F7D65CD24D35638F707D9D0AB4AD47CFE847BD19D1694782F5F547F69A6FD02EC5358BBB1D2BDE36688C923A3E32CC1EACF196C8F4A49554F180B6F9F600F9CAD688DEAA8572482E7263D20318222FA3A79740A495451A0C74F5BEE14B4BFD43B3D2928C9")]
