// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Toolkit.Win32.UI.Interop.WinForms;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    /// <summary>
    /// WrapperLookup is a set of extension methods to extend <see cref="FrameworkElement"/> to make it relatively easy
    /// to find its associated WindowsXamlHostBaseExt.
    /// (WPF Interop uses an attached DependencyProperty for this).
    /// </summary>
    public static class WrapperLookup
    {
        private static readonly IDictionary<FrameworkElement, WindowsXamlHostBaseExt> _controlCollection = new Dictionary<FrameworkElement, WindowsXamlHostBaseExt>();

        public static WindowsXamlHostBaseExt GetWrapper(this FrameworkElement control)
        {
            if (control == null)
            {
                return null;
            }

            _controlCollection.TryGetValue(control, out var result);
            return result;
        }

        public static void SetWrapper(this FrameworkElement control, WindowsXamlHostBaseExt wrapper)
        {
            if (control == null || wrapper == null)
            {
                return;
            }

            _controlCollection.Add(control, wrapper);
        }

        public static void ClearWrapper(this FrameworkElement control)
        {
            if (control == null)
            {
                return;
            }

            _controlCollection.Remove(control);
        }
    }
}
