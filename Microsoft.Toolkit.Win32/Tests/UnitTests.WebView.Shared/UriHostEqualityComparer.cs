// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared
{
    internal class UriHostEqualityComparer : IEqualityComparer<Uri>
    {
        private UriHostEqualityComparer()
        {
        }

        internal static UriHostEqualityComparer Default => Nested.Instance;

        public bool Equals(Uri x, Uri y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;

            return Uri.Compare(x, y, UriComponents.Host, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) ==
                   0;
        }

        public int GetHashCode(Uri obj)
        {
            return obj.GetHashCode();
        }

        private class Nested
        {
            internal static readonly UriHostEqualityComparer Instance = new UriHostEqualityComparer();

            // Explict static ctor to tell compiler not to mark type as beforefieldinit
            static Nested() { }
        }
    }
}