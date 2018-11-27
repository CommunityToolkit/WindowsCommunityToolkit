// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.Forms.UI.XamlHost;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    /// <summary>
    /// Extension class for WindowsXamlHostBase to access properties of the associated UWP control or use an internal dictionary during design time
    /// </summary>
    internal static class UwpControlProperties
    {
        private const string DesignerPropertiesName = "DesignerProperties";

        private static Dictionary<string, object> GetDesignerProperties(WindowsXamlHostBase wrapper)
        {
            PropertyInfo designerProperty = wrapper.GetType().GetProperty(DesignerPropertiesName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (designerProperty == null)
            {
                throw new MissingMethodException("Wrapper class does not contain the required DesignerProperties dictionary");
            }

            Dictionary<string, object> propertiesDictionary = designerProperty.GetValue(wrapper) as Dictionary<string, object>;
            if (propertiesDictionary == null)
            {
                propertiesDictionary = new Dictionary<string, object>();
                designerProperty.SetValue(wrapper, propertiesDictionary);
            }

            return propertiesDictionary;
        }

        private static object GetDefaultValueForProperty(WindowsXamlHostBase wrapper, string propName)
        {
            PropertyDescriptor descriptor = TypeDescriptor.GetProperties(wrapper).Find(propName, false);
            if (descriptor == null)
            {
                throw new MissingMethodException("Wrapper class does not contain property " + propName.ToString());
            }

            DefaultValueAttribute attribute = descriptor.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;
            if (attribute == null)
            {
                throw new ArgumentException("Wrapper class does not define a DefaultValue attribute for property " + propName.ToString());
            }

            return attribute.Value;
        }

        internal static object GetUwpControlValue(this WindowsXamlHostBase wrapper, object defaultValue = null, [CallerMemberName]string propName = null)
        {
            Windows.UI.Xaml.UIElement control = wrapper.GetUwpInternalObject() as Windows.UI.Xaml.UIElement;
            if (control != null)
            {
                return control.GetType().GetRuntimeProperty(propName).GetValue(control);
            }
            else
            {
                Dictionary<string, object> properties = GetDesignerProperties(wrapper);
                if (properties.ContainsKey(propName))
                {
                    return properties[propName];
                }

                return defaultValue != null ? defaultValue : GetDefaultValueForProperty(wrapper, propName);
            }
        }

        internal static void SetUwpControlValue(this WindowsXamlHostBase wrapper, object value, [CallerMemberName]string propName = null)
        {
            Windows.UI.Xaml.UIElement control = wrapper.GetUwpInternalObject() as Windows.UI.Xaml.UIElement;
            if (control != null)
            {
                control.GetType().GetRuntimeProperty(propName).SetValue(control, value);
            }
            else
            {
                Dictionary<string, object> properties = GetDesignerProperties(wrapper);
                properties[propName] = value;
            }
        }
    }
}
