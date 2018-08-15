// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Navigation
{
    [TestClass]
    [TestCategory(TestConstants.Categories.Nav)]
    public class HTTP_GET : HostFormWebViewContextSpecification
    {
        private bool _success;

        protected override void Given()
        {
            base.Given();
            WebView.NavigationCompleted += (o, e) =>
            {
                _success = e.IsSuccess;
                Form.Close();
            };
        }

        protected override void When()
        {
            NavigateAndWaitForFormClose(TestConstants.Uris.ExampleCom, HttpMethod.Get);
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        public void NavigationShouldComplete()
        {
            _success.ShouldBeTrue();
        }
    }

    [TestClass]
    [TestCategory(TestConstants.Categories.Nav)]
    public class HTTP_POST : HostFormWebViewContextSpecification
    {
        private bool _success;
        private Uri _uri = new Uri(TestConstants.Uris.HttpBin, "/post");

        protected override void Given()
        {
            base.Given();
            WebView.NavigationCompleted += (o, e) =>
            {
                _success = e.IsSuccess;
                Form.Close();
            };
        }

        protected override void When()
        {

            NavigateAndWaitForFormClose(
                _uri,
                HttpMethod.Post,
                "{\"prop\":\"content\"}",
                new []{new KeyValuePair<string, string>("accept", "application/json"), });
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        public void NavigationShouldComplete()
        {
            _success.ShouldBeTrue();
        }
    }
}
