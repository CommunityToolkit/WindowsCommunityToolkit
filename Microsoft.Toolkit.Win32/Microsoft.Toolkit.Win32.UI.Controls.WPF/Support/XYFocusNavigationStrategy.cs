// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Input.XYFocusNavigationStrategy"/>
    /// </summary>
    public enum XYFocusNavigationStrategy : int
    {
        Auto = 0,
        Projection = 1,
        NavigationDirectionDistance = 2,
        RectilinearDistance = 3,
    }
}