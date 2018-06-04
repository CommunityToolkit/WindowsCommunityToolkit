// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.NavigateToLocalStreamUri
{
    [TestClass]
    [TestCategory(TestConstants.Categories.Nav)]
    [DeploymentItem("FunctionalTests\\NavigateToLocalStreamUri\\async.htm")]
    public class Given_a_local_htm_file : HostFormWebViewContextSpecification
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
            NavigateToLocalAndWaitForFormClose("async.htm", new TestStreamResolver());
        }

        [TestMethod]
        public void LocalNavigationCompleted()
        {
            _success.ShouldBeTrue();
        }
    }

    [TestClass]
    [TestCategory(TestConstants.Categories.Nav)]
    [DeploymentItem("FunctionalTests\\NavigateToLocalStreamUri\\async.htm")]
    public class Given_a_local_htm_file_with_async_XHR_for_local_content : HostFormWebViewContextSpecification
    {
        private string _scriptNotifyResult;

        protected override void Given()
        {
            base.Given();
            WebView.IsJavaScriptEnabled = true;
            WebView.IsScriptNotifyAllowed = true;


            WebView.ScriptNotify += (o, e) =>
            {
                _scriptNotifyResult = e.Value;
                Form.Close();
            };
        }

        protected override void When()
        {
            NavigateToLocalAndWaitForFormClose("async.htm", new TestStreamResolver());
        }

        [TestMethod]
        public void LocalNavigationCompleted()
        {
            _scriptNotifyResult.ShouldEqual("Success");
        }
    }

    [TestClass]
    [TestCategory(TestConstants.Categories.Nav)]
    [DeploymentItem("FunctionalTests\\NavigateToLocalStreamUri\\sync.htm")]
    public class Given_a_local_htm_file_with_sync_XHR_for_local_content : HostFormWebViewContextSpecification
    {
        private string _scriptNotifyResult;

        protected override void Given()
        {
            base.Given();
            WebView.IsJavaScriptEnabled = true;
            WebView.IsScriptNotifyAllowed = true;


            WebView.ScriptNotify += (o, e) =>
            {
                _scriptNotifyResult = e.Value;
                Form.Close();
            };
        }

        protected override void When()
        {
            NavigateToLocalAndWaitForFormClose("sync.htm", new TestStreamResolver());
        }

        [TestMethod]
        public void LocalNavigationCompleted()
        {
            _scriptNotifyResult.ShouldEqual("Success");
        }
    }
}
