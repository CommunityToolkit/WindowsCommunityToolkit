// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using System;
using System.IO;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.NavigateToLocalStreamUri
{
    internal class TestStreamResolver : IUriToStreamResolver
    {
        private readonly string _path;

        public TestStreamResolver()
        {
            _path = Path.GetDirectoryName(GetType().Assembly.Location);
        }

        public Stream UriToStream(Uri uri)
        {
            var path = uri.AbsolutePath;
            path = path.TrimStart(PathUtilities.AltDirectorySeparatorChar);
            path = path.Replace(PathUtilities.AltDirectorySeparatorChar, PathUtilities.DirectorySeparatorChar);
            var fullPath = Path.Combine(_path, path);
            return File.Open(fullPath, FileMode.Open, FileAccess.Read);
        }
    }
}
