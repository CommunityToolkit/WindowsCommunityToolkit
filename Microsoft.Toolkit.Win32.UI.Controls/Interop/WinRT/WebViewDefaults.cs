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

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    internal static class WebViewDefaults
    {
        public const string AboutBlank = "about:blank";
        public const bool IsIndexedDBEnabled = true;
        public const bool IsJavaScriptEnabled = true;
        public const bool IsPrivateNetworkEnabled = false;
        public const bool IsScriptNotifyEnabled = false;
        public static readonly Uri AboutBlankUri = new Uri(AboutBlank);
    }
}