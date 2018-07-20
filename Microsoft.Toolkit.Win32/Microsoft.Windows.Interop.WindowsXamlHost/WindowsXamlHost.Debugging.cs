// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Interop
{
    using System.Diagnostics;
    using System.Windows.Interop;

    /// <summary>
    ///     A WPF control that can be used to host XAML content
    /// </summary>
    partial class WindowsXamlHost : HwndHost
    {
        /// <summary>
        /// Debug tracing component instance for WindowsXamlHost
        /// </summary>
        private static readonly TraceSource TraceSource = new TraceSource("WindowsXamlHost");
    }
}
