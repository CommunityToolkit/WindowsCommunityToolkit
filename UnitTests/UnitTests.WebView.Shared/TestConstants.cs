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