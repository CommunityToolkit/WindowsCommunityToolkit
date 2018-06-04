// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared
{
    public static partial class TestConstants
    {
        public static class Timeouts
        {
            /// <summary>
            /// A timeout of 30 seconds
            /// </summary>
            public const int Longest = Long * 3;
            /// <summary>
            /// A timeout of 20 seconds
            /// </summary>
            public const int Longer = Long * 2;
            /// <summary>
            /// A timeout of 10 seconds
            /// </summary>
            public const int Long = 10000;
            /// <summary>
            /// A timeout of 5 seconds
            /// </summary>
            public const int Medium = 5000;
            /// <summary>
            /// A timeout of 3 seconds
            /// </summary>
            public const int Short = Shorter * 3;
            /// <summary>
            /// A timeout of 1 second
            /// </summary>
            public const int Shorter = 1000;
        }
        public static class Categories
        {
            public const string Init = "Initialization";
            public const string Nav = "Navigation";
            public const string Des = "Designer";
            public const string Proc = "Process";
            public const string Wf = "WinForms";
        }
    }
}