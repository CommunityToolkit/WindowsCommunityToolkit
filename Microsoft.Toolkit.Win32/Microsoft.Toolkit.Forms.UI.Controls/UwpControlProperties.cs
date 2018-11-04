// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.Forms.UI.XamlHost;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    /// <summary>
    /// Extension class for WindowsXamlHostBase to access properties of the associated UWP control or use an internal field during design time
    /// </summary>
    internal static class UwpControlProperties
    {
        internal static object GetUwpControlValue(this WindowsXamlHostBase wrapper, [CallerMemberName]string propName = null)
        {
            Windows.UI.Xaml.UIElement control = wrapper.GetUwpInternalObject() as Windows.UI.Xaml.UIElement;
            if (control != null)
            {
                return control.GetType().GetRuntimeProperty(propName).GetValue(control);
            }
            else
            {
                return wrapper.GetType().GetField(new string('_', 1) + propName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(wrapper);
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
                wrapper.GetType().GetField(new string('_', 1) + propName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(wrapper, value);
            }
        }
    }
}
