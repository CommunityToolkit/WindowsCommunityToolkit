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
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.SampleApp.Common
{
    public static class Constants
    {
        public const string TestBackgroundTaskName = "TestBackgroundTaskName";

        public static readonly Color ApplicationBackgroundColor = Color.FromArgb(255, 51, 51, 51);

        public static readonly Uri Square44x44Logo = new Uri("ms-appx:///Assets/Square44x44Logo.png");
        public static readonly Uri Square150x150Logo = new Uri("ms-appx:///Assets/Square150x150Logo.png");
        public static readonly Uri Wide310x150Logo = new Uri("ms-appx:///Assets/Wide310x150Logo.png");
        public static readonly Uri Square310x310Logo = new Uri("ms-appx:///Assets/Square310x310Logo.png");

        public static string ApplicationDisplayName { get; set; }
    }
}
