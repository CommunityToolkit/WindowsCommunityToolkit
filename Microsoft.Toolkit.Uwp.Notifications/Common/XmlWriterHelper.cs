// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#if WINDOWS_UWP

#endif

namespace Microsoft.Toolkit.Uwp.Notifications
{
    internal static class XmlWriterHelper
    {
        public static void Write(System.Xml.XmlWriter writer, object element)
        {
            // If it isn't an XML element, don't write anything
            if (element is not IHaveXmlName xmlElement)
            {
                return;
            }

            writer.WriteStartElement(xmlElement.Name);

            IEnumerable<PropertyInfo> properties = GetProperties(element.GetType());

            List<object> elements = new List<object>();
            object content = null;

            // Write the attributes first
            foreach (PropertyInfo p in properties)
            {
                IEnumerable<Attribute> attributes = GetCustomAttributes(p);

                NotificationXmlAttributeAttribute attr = attributes.OfType<NotificationXmlAttributeAttribute>().FirstOrDefault();

                object propertyValue = GetPropertyValue(p, element);

                // If it's the additional properties item
                if (p.Name == nameof(IHaveXmlAdditionalProperties.AdditionalProperties) && element is IHaveXmlAdditionalProperties && p.PropertyType == typeof(IReadOnlyDictionary<string, string>))
                {
                    continue;
                }

                // If it's an attribute
                else if (attr != null)
                {
                    continue;
                }

                // If it's a content attribute
                else if (attributes.OfType<NotificationXmlContentAttribute>().Any())
                {
                    continue;
                }

                // Otherwise it's an element or collection of elements
                else
                {
                    if (propertyValue != null)
                    {
                        elements.Add(propertyValue);
                    }
                }
            }

            content = (element as IHaveXmlText)?.Text;

            foreach (var property in (element as IHaveXmlAdditionalProperties)?.AdditionalProperties ?? Enumerable.Empty<KeyValuePair<string, string>>())
            {
                writer.WriteAttributeString(property.Key, property.Value);
            }

            foreach (var property in (element as IHaveXmlNamedProperties)?.EnumerateNamedProperties() ?? Enumerable.Empty<KeyValuePair<string, object>>())
            {
                if (property.Value is not null)
                {
                    writer.WriteAttributeString(property.Key, PropertyValueToString(property.Value));
                }
            }

            // Then write children
            foreach (object el in elements)
            {
                // If it's a collection of children
                if (el is IEnumerable)
                {
                    foreach (object child in el as IEnumerable)
                    {
                        Write(writer, child);
                    }

                    continue;
                }

                // Otherwise just write the single element
                Write(writer, el);
            }

            // Then write any content if there is content
            if (content != null)
            {
                string contentString = content.ToString();
                if (!string.IsNullOrWhiteSpace(contentString))
                {
                    writer.WriteString(contentString);
                }
            }

            writer.WriteEndElement();
        }

        private static object GetPropertyValue(PropertyInfo propertyInfo, object obj)
        {
#if NETFX_CORE
            return propertyInfo.GetValue(obj);
#else
            return propertyInfo.GetValue(obj, null);
#endif
        }

        private static string PropertyValueToString(object propertyValue)
        {
            Type type = propertyValue.GetType();

            if (IsEnum(type))
            {
                EnumStringAttribute enumStringAttr = GetEnumStringAttribute(propertyValue as Enum);

                if (enumStringAttr != null)
                {
                    return enumStringAttr.String;
                }
            }
            else if (propertyValue is bool)
            {
                if ((bool)propertyValue)
                {
                    return "true";
                }

                return "false";
            }
            else if (propertyValue is DateTimeOffset?)
            {
                DateTimeOffset? dateTime = propertyValue as DateTimeOffset?;
                if (dateTime.HasValue)
                {
                    // ISO 8601 format
                    return System.Xml.XmlConvert.ToString(dateTime.Value);
                }
                else
                {
                    return null;
                }
            }

            return propertyValue.ToString();
        }

        private static EnumStringAttribute GetEnumStringAttribute(Enum enumValue)
        {
#if NETFX_CORE
            return enumValue.GetType().GetTypeInfo().GetDeclaredField(enumValue.ToString()).GetCustomAttribute<EnumStringAttribute>();
#else
            MemberInfo[] memberInfo = enumValue.GetType().GetMember(enumValue.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(EnumStringAttribute), false);

                if (attrs != null && attrs.Length > 0)
                    return attrs[0] as EnumStringAttribute;
            }

            return null;
#endif
        }

        private static bool IsEnum(Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().IsEnum;
#else
            return type.IsEnum;
#endif
        }

        private static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
#if NETFX_CORE
            return type.GetTypeInfo().DeclaredProperties;
#else
            return type.GetProperties();
#endif
        }

        private static IEnumerable<Attribute> GetCustomAttributes(PropertyInfo propertyInfo)
        {
#if NETFX_CORE
            return propertyInfo.GetCustomAttributes();
#else
            return propertyInfo.GetCustomAttributes(true).OfType<Attribute>();
#endif
        }

        /// <summary>
        /// Gets the provided binding value, if it exists. Otherwise, falls back to the absolute value.
        /// </summary>
        /// <typeparam name="T">The type of the enum of the class properties.</typeparam>
        /// <param name="bindings">The collection of data-bound values.</param>
        /// <param name="bindableProperty">The property to obtain.</param>
        /// <param name="absoluteValue">The absolute value, if any.</param>
        /// <returns>The provided binding value, if it exists. Otherwise, falls back to the absolute value.</returns>
        internal static string GetBindingOrAbsoluteXmlValue<T>(IDictionary<T, string> bindings, T bindableProperty, string absoluteValue)
        {
            // If a binding is provided, use the binding value
            string bindingValue;
            if (bindings.TryGetValue(bindableProperty, out bindingValue))
            {
                return "{" + bindingValue + "}";
            }

            // Otherwise fallback to the absolute value
            return absoluteValue;
        }
    }
}