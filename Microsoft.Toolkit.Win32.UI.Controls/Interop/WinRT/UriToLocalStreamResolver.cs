// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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