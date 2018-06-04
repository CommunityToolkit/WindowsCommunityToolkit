// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared
{
    public static partial class TestConstants
    {
        public static class Uris
        {
            /*
             * As described in RFC 2606 and RFC 6761, the following domains are maintained for documentation purposes
             */
            public static readonly Uri ExampleCom = new Uri("http://example.com", UriKind.Absolute);
            public static readonly Uri ExampleNet = new Uri("http://example.net", UriKind.Absolute);
            public static readonly Uri ExampleOrg = new Uri("http://example.org", UriKind.Absolute);

            // Local navigation: when using a null value for Source the uri is about:blank
            public static readonly Uri AboutBlank = new Uri("about:blank");
        }
    }
}
