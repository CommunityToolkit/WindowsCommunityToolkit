// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Provides a method to translate a Uniform Resource I (URI) to a <see cref="Stream"/> for use by the <see cref="IWebView.NavigateToLocal(string)"/> method.
    /// </summary>
    /// <seealso cref="Windows.Web.IUriToStreamResolver"/>
    public interface IUriToStreamResolver
    {
        Stream UriToStream(Uri uri);
    }
}