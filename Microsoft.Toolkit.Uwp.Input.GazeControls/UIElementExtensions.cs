// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.Input.GazeControls
{
    internal static class UIElementExtensions
    {
        public static T FindControl<T>(this UIElement parent, string controlName)
            where T : FrameworkElement
        {
            if (parent == null)
            {
                return null;
            }

            if (parent.GetType() == typeof(T) && ((T)parent).Name == controlName)
            {
                return (T)parent;
            }

            T result = null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                UIElement child = (UIElement)VisualTreeHelper.GetChild(parent, i);

                if (FindControl<T>(child, controlName) != null)
                {
                    result = FindControl<T>(child, controlName);
                    break;
                }
            }

            return result;
        }
    }
}
