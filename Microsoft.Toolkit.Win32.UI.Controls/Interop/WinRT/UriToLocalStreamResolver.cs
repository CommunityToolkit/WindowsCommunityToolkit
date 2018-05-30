// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Web;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    internal sealed class UriToLocalStreamResolver : IUriToStreamResolver
    {
        public IAsyncOperation<IInputStream> UriToStreamAsync(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            var path = uri.AbsolutePath;
            return GetContentAsync(path).AsAsyncOperation();
        }

        private bool IsRelative(string path)
        {
            return path != null && path[0] == '/';
        }

        private static readonly Lazy<string> BasePath = new Lazy<string>(() =>
#pragma warning disable SA1129 // Do not use default value type constructor
            Path.GetDirectoryName(UnsafeNativeMethods.GetModuleFileName(new HandleRef())));
#pragma warning restore SA1129 // Do not use default value type constructor

        private async Task<IInputStream> GetContentAsync(string path)
        {
            if (IsRelative(path))
            {
                // Clean up path
                while (path[0] == '/')
                {
                    path = path.TrimStart('/');
                }

                // Translate forward slash into backslash for use in file paths
                path = path.Replace(@"/", @"\");

                var fullPath = Path.Combine(BasePath.Value, path);
                return File.Open(fullPath, FileMode.Open, FileAccess.Read).AsInputStream();
            }

            throw new NotSupportedException();
        }
    }
}