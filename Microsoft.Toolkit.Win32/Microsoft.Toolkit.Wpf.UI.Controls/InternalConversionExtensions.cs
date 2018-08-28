// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;

namespace Microsoft.Toolkit.Wpf.UI.Controls
{
    internal static class InternalConversionExtensions
    {
        public static Rect ToWpf(this Windows.Foundation.Rect uwp)
        {
            return new Rect(uwp.X, uwp.Y, uwp.Width, uwp.Height);
        }

        public static Windows.Foundation.Point ToUwp(this Point wpf)
        {
            return new Windows.Foundation.Point(wpf.X, wpf.Y);
        }
    }
}
