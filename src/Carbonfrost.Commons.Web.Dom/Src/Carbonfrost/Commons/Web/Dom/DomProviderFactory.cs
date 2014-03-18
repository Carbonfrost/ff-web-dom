//
// - DomProviderFactory.cs -
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Carbonfrost.Commons.Shared.Runtime;

namespace Carbonfrost.Commons.Web.Dom {

    [Providers]
    public abstract class DomProviderFactory {

        public static readonly DomProviderFactory Default
            = new DefaultDomProviderFactory();

        internal static IEnumerable<DomProviderFactory> All {
            get {
                return AppDomain.CurrentDomain.GetProviders<DomProviderFactory>();
            }
        }

        public virtual IDomNodeFactory NodeFactory {
            get {
                return DomNodeFactory.Default;
            }
        }

        public static DomProviderFactory FromName(string name) {
            return AppDomain.CurrentDomain.GetProvider<DomProviderFactory>(name);
        }

        public static DomProviderFactory ForProviderObject(object instance) {
            if (instance == null)
                throw new ArgumentNullException("instance");

            // TODO These should be ordered according to provider likelihood for security
            return All.FirstOrDefault(t => t.IsProviderObject(instance));
        }

        public virtual bool IsProviderObject(Type providerObjectType) {
            if (providerObjectType == null)
                throw new ArgumentNullException("providerObjectType");

            return providerObjectType.Assembly == GetType().Assembly;
        }

        public bool IsProviderObject(object providerObject) {
            if (providerObject == null)
                throw new ArgumentNullException("providerObject");

            return IsProviderObject(providerObject.GetType());
        }

        public static DomProviderFactory ForProviderObject(Type providerObjectType) {
            if (providerObjectType == null)
                throw new ArgumentNullException("providerObjectType");

            // TODO Some providers are checked twice (performance, rare)
            var e = AppDomain.CurrentDomain.GetProviders<DomProviderFactory>(
                new {
                    Assembly = providerObjectType.Assembly,
                }).Concat(AppDomain.CurrentDomain.GetProviders<DomProviderFactory>());

            return e.FirstOrDefault(t => t.IsProviderObject(providerObjectType));
        }

        public DomNodeWriter CreateWriter(DomNode node, DomNodeWriterSettings settings) {
            return CreateDomWriter(node);
        }

        public DomNodeReader CreateReader(DomNode node) {
            return CreateDomReader(node);
        }

        protected virtual DomNodeWriter CreateDomWriter(DomNode node) {
            if (node == null)
                throw new ArgumentNullException("node");

            return new DomNodeWriter(node);
        }

        protected virtual DomNodeReader CreateDomReader(DomNode node) {
            if (node == null)
                throw new ArgumentNullException("node");

            return new DomNodeReader(node, null);
        }

        public DomReader CreateReader(string path) {
            return CreateReader(path, null);
        }

        public DomReader CreateReader(string path, DomReaderSettings settings) {
            return CreateDomReader(File.OpenText(path), settings);
        }

        public DomReader CreateReader(TextReader reader) {
            return CreateReader(reader, null);
        }

        public DomReader CreateReader(TextReader reader, DomReaderSettings settings) {
            if (reader == null)
                throw new ArgumentNullException("reader");

            return CreateDomReader(reader, settings);
        }

        protected virtual DomReader CreateDomReader(TextReader reader, DomReaderSettings settings) {
            return DomReader.CreateXml(reader);
        }
    }
}
