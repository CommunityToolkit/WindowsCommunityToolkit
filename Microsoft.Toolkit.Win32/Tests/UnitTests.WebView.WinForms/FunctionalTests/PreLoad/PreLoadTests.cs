// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;
using System.IO;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.PreLoad
{
    [TestClass]
    public class NullPreLoadScript : HostFormWebViewContextSpecification
    {
        protected override void Given()
        {
            base.Given();
            WebView.IsJavaScriptEnabled = true;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotPassNullForPreLoadScript()
        {
            WebView.AddPreLoadedScript(null);
        }
    }

    [TestClass]
    public class EmptyPreLoadScript : HostFormWebViewContextSpecification
    {
        private bool _navSuccess;

        protected override void Given()
        {
            base.Given();
            WebView.IsJavaScriptEnabled = true;
            WebView.AddPreLoadedScript(string.Empty);

            WebView.NavigationCompleted += (o, e) =>
            {
                _navSuccess = e.IsSuccess;
                Form.Close();
            };
        }

        protected override void When()
        {
            NavigateAndWaitForFormClose(TestConstants.Uris.ExampleCom);
        }

        [TestMethod]
        public void NavigationCompletesForEmptyPreLoadScript()
        {
            _navSuccess.ShouldBeTrue();
        }
    }

    [TestClass]
    public class NonExistentPreLoadScript : HostFormWebViewContextSpecification
    {
        private bool _navSuccess;

        protected override void Given()
        {
            base.Given();
            WebView.IsJavaScriptEnabled = true;
            WebView.AddPreLoadedScript($"./non-exist.js");

            WebView.NavigationCompleted += (o, e) =>
            {
                _navSuccess = e.IsSuccess;
                Form.Close();
            };
        }

        protected override void When()
        {
            NavigateAndWaitForFormClose(TestConstants.Uris.ExampleCom);
        }

        [TestMethod]
        public void NavigateCompletesWithoutError()
        {
            _navSuccess.ShouldBeTrue();
        }
    }

    [TestClass]
    [DeploymentItem("FunctionalTests/PreLoad/preload.js")]
    public class RelativePreLoadScript : HostFormWebViewContextSpecification
    {
        private bool _scriptNotifyCalled;

        protected override void Given()
        {
            base.Given();
            WebView.IsScriptNotifyAllowed = true;
            WebView.IsJavaScriptEnabled = true;

            // Not sure how to get this to execute
            WebView.AddPreLoadedScript(Path.Combine(TestContext.TestDeploymentDir, "preload.js"));

            // Set up the event handler
            WebView.ScriptNotify += (o, e) =>
            {
                _scriptNotifyCalled = true;
                Form.Close();
            };
        }

        protected override void When()
        {
            NavigateAndWaitForFormClose(TestConstants.Uris.ExampleCom);
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        public void ScriptNotifyRaised()
        {
            _scriptNotifyCalled.ShouldBeTrue();
        }
    }
}
