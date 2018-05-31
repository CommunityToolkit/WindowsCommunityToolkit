// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Win32.UI.Controls
{
    /// <inheritdoc />
    public interface IWebView2 : IWebView
    {
        /// <summary>
        /// Gets or sets a partition for this process.
        /// </summary>
        /// <value>The partition of this process.</value>
        /// <remarks>Value can be set prior to the component being initialized.</remarks>
        /// <see cref="WebViewControlProcessOptions.Partition"/>
        string Partition { get; set; }
    }
}