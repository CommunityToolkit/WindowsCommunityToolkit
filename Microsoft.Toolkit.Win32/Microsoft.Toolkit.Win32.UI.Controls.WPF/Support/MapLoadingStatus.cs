// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="global::Windows.UI.Xaml.Controls.Maps.MapLoadingStatus"/>
    /// </summary>
    public enum MapLoadingStatus : int
    {
        Loading = 0,
        Loaded = 1,
        DataUnavailable = 2,
    }
}