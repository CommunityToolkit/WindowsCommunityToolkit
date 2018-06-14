// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace UnitTests
{
    using System;
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
