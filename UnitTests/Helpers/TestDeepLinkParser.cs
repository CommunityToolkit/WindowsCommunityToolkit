// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace UnitTests
{
    using System;
    using Microsoft.Toolkit.Uwp.Helpers;

    internal class TestDeepLinkParser : DeepLinkParser
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

    internal class TestCollectionCapableDeepLinkParser : CollectionFormingDeepLinkParser
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
