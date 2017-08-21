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

namespace UnitTests
{
    using System;
    using Microsoft.Toolkit.Uwp;
    using Microsoft.Toolkit.Uwp.Helpers;

#pragma warning disable SA1649 // File name must match first type name
    internal class TestDeepLinkParser : DeepLinkParser
#pragma warning restore SA1649 // File name must match first type name
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Call stack reviewed.")]
        public TestDeepLinkParser(string uri)
            : base(uri)
        {
        }

        public TestDeepLinkParser(Uri uri)
            : base(uri)
        {
        }
    }

#pragma warning disable SA1402 // File may only contain a single class
    internal class TestCollectionCapableDeepLinkParser : CollectionFormingDeepLinkParser
#pragma warning restore SA1402 // File may only contain a single class
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Call stack reviewed.")]
        public TestCollectionCapableDeepLinkParser(string uri)
            : base(uri)
        {
        }

        public TestCollectionCapableDeepLinkParser(Uri uri)
            : base(uri)
        {
        }
    }
}
