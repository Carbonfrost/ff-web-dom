//
// - DomStringTokenListConverter.cs -
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
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;

namespace Carbonfrost.Commons.Web.Dom {

    public sealed class DomStringTokenListConverter : TypeConverter {

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            return typeof(string) == destinationType
                || typeof(InstanceDescriptor) == destinationType
                || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            if (value == null)
                throw base.GetConvertFromException(value);

            string s = value as string;
            if (s == null)
                throw base.GetConvertFromException(value);

            return this.Parse(context, culture, s);
        }

        public sealed override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            DomStringTokenList hcl = value as DomStringTokenList;

            if (destinationType != null && hcl != null) {
                if (destinationType == typeof(string))
                    return hcl.ToString();

                if (destinationType == typeof(InstanceDescriptor))
                    return this.CreateInstanceDescriptor(context, culture, hcl);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        private InstanceDescriptor CreateInstanceDescriptor(ITypeDescriptorContext context, CultureInfo culture, DomStringTokenList value) {
            return new InstanceDescriptor(typeof(DomStringTokenList).GetConstructor(new [] { typeof(string) }),
                                          new object[] { value.ToString() });
        }

        private DomStringTokenList Parse(ITypeDescriptorContext context, CultureInfo culture, string text) {
            return DomStringTokenList.Parse(text);
        }
    }
}
