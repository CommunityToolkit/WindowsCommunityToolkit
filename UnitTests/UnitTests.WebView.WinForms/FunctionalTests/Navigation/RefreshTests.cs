// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Navigation
{
    [TestClass]
    [TestCategory(TestConstants.Categories.Nav)]
    public class RefreshTests : HostFormWebViewContextSpecification
    {
        private int _navigations;
        protected override void Given()
        {
            base.Given();
            WebView.NavigationCompleted += (o, e) =>
            {
                _navigations++;
                if (_navigations == 2)
                {
                    Form.Close();
                }
                else
                {
                    WebView.Refresh();
                }
            };
        }

        protected override void When()
        {
            NavigateAndWaitForFormClose(TestConstants.Uris.ExampleOrg);
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        public void RefreshCountsAsNavigation()
        {
            _navigations.ShouldEqual(2);
        }
    }
}
