// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.ElementHighContrastAdjustment"/>
    /// </summary>
    [Flags]
    public enum ElementHighContrastAdjustment : uint
    {
        None = 0,
        Application = 2147483648,
        Auto = 4294967295,
    }
}