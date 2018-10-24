// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Forms.UI.XamlHost
{
    /// <summary>
    /// WrapperLookup is a set of extension methods to extend <see cref="UIElement"/> to make it relatively easy
    /// to find its associated WindowsXamlHostBaseExt.
    /// (WPF Interop uses an attached DependencyProperty for this).
    /// </summary>
    public static class WrapperLookup
    {
        private static readonly IDictionary<UIElement, WeakReference<WindowsXamlHostBase>> _controlCollection = new Dictionary<UIElement, WeakReference<WindowsXamlHostBase>>();

        public static WindowsXamlHostBase GetWrapper(this UIElement control)
        {
            if (control == null)
            {
                return null;
            }

            _controlCollection.TryGetValue(control, out var weakRef);
            if (weakRef.TryGetTarget(out var result))
            {
                return result;
            }

            return null;
        }

        public static void SetWrapper(this UIElement control, WindowsXamlHostBase wrapper)
        {
            if (control == null || wrapper == null)
            {
                return;
            }

            _controlCollection[control] = new WeakReference<WindowsXamlHostBase>(wrapper);
        }

        public static void ClearWrapper(this UIElement control)
        {
            if (control == null)
            {
                return;
            }

            _controlCollection.Remove(control);
        }
    }
}
