// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Navigation
{
    [TestClass]
    [TestCategory(TestConstants.Categories.Nav)]
    public class AfterNavigatingMoreThanOnceThenGoingBack : HostFormWebViewContextSpecification
    {
        private int _navigationCount;

        protected override void Given()
        {
            base.Given();

            WebView.NavigationCompleted += (o, e) =>
            {
                _navigationCount++;
                WriteLine($"NavCompleted: {e.Uri}; NavCount: {_navigationCount}");
                e.IsSuccess.ShouldBeTrue($"Navigation failure: {e.WebErrorStatus}");

                if (WebView.CanGoBack)
                {
                    WebView.GoBack();
                } else if (!WebView.CanGoBack && WebView.CanGoForward)
                {
                    Form.Close();
                }
                else
                {
                    WebView.Navigate(new Uri(TestConstants.Uris.ExampleCom, "?" + _navigationCount));
                }

                
            };

        }

        protected override void When()
        {
            NavigateAndWaitForFormClose(TestConstants.Uris.ExampleCom);
        }

        [TestMethod]
        //[Timeout(TestConstants.Timeouts.Long)]
        public void CanGoForwardIsTrue()
        {
            // NOTE: Due to asynchronous nature the actual assert is in Given()
        }
    }
}
