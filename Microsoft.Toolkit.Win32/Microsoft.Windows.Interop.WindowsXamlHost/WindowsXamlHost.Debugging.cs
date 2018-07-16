// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Windows.Interop
{
    using System.Diagnostics;
    using System.Windows.Interop;

    /// <summary>
    ///     A WPF control that can be used to host XAML content
    /// </summary>
    partial class WindowsXamlHost : HwndHost
    {
        #region DEBUG
        /// <summary>
        /// Debug tracing component instance for WindowsXamlHost
        /// </summary>
        private static TraceSource traceSource = new TraceSource("WindowsXamlHost");
        #endregion DEBUG
    }
}
