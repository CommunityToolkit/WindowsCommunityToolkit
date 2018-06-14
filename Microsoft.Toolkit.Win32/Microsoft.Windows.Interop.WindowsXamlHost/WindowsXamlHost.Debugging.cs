// <copyright file="WindowsXamlHostDebugging.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>Microsoft</author>

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
