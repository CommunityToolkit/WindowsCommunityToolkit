// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="Windows.UI.Core.CoreInputDeviceTypes"/>
    /// </summary>
    [Flags]
    public enum CoreInputDeviceTypes : uint
    {
        None = 0,
        Touch = 1,
        Pen = 2,
        Mouse = 4,
    }
}