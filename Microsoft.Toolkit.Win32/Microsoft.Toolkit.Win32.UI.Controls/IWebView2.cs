// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Win32.UI.Controls
{
    /// <summary>
    /// Provides a control that hosts HTML content in an app.
    /// </summary>
    /// <seealso cref="IWebView"/>
    public interface IWebView2
    {
        /// <summary>
        /// Loads local web content at the specified Uniform Resource Identifier (URI) using an <see cref="IUriToStreamResolver"/>.
        /// </summary>
        /// <param name="relativePath">A path identifying the local HTML content to load.</param>
        /// <param name="streamResolver">A <see cref="IUriToStreamResolver"/> instance that converts a Uniform Resource Identifier (URI) into a stream to load.</param>
        void NavigateToLocalStreamUri(Uri relativePath, IUriToStreamResolver streamResolver);
    }
}