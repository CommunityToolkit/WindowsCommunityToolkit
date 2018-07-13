// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// An adapter converting <see cref="IUriToStreamResolver"/> to <see cref="global::Windows.Web.IUriToStreamResolver"/>.
    /// </summary>
    internal sealed class GenericUriToStreamResolver : global::Windows.Web.IUriToStreamResolver, IUriToStreamResolver
    {
        private readonly IUriToStreamResolver _streamResolver;

        public GenericUriToStreamResolver(IUriToStreamResolver streamResolver)
        {
            _streamResolver = streamResolver ?? throw new ArgumentNullException(nameof(streamResolver));
        }

        public Stream UriToStream(Uri uri)
        {
            return _streamResolver.UriToStream(uri);
        }

        public IAsyncOperation<IInputStream> UriToStreamAsync(Uri uri)
        {
            var streamOp = UriToStream(uri);
            if (streamOp == null)
            {
                return null;
            }

            return Task.FromResult(streamOp.AsInputStream()).AsAsyncOperation();
        }
    }
}