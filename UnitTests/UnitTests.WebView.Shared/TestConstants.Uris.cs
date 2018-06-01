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
