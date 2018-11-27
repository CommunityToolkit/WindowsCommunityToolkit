// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared
{
    public static class OperatingSystemAssertions
    {
#pragma warning disable RECS0154 // Parameter is never used
        public static void OSBuildShouldBeAtLeast(this Assert _, int buildNumber) => AssertOperatingSystemSinceBuild(buildNumber);
#pragma warning restore RECS0154 // Parameter is never used

#pragma warning disable RECS0154 // Parameter is never used
        public static void OSBuildShouldBeAtLeast(this Assert _, TestConstants.Windows10Builds build) => AssertOperatingSystemSinceBuild((int)build);
#pragma warning restore RECS0154 // Parameter is never used

        private static void AssertOperatingSystemSinceBuild(int buildNumber)
        {
            var os = NativeMethods.RtlGetVersion();
            if (os.BuildNumber < buildNumber)
            {
                Assert.Inconclusive($"Required build number {buildNumber}. OS is {os.BuildNumber}");
            }
        }
    }
}