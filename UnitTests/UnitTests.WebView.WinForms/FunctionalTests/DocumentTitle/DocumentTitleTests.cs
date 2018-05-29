// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.DocumentTitle
{
    [TestClass]
    public class DocumentTitleNavigateToStringTest : HostFormWebViewContextSpecification
    {
        public string Expected { get; set; } = "Hello World!";
        private string _actual;
        private string _content;

        protected override void Given()
        {
            _content = $@"
<html>
<head><title>{Expected}</title></head>
<body>
<h1>DocumentTitleNavigateToStringTest</h1>
</body>
</html>
";

            base.Given();
            WebView.NavigationCompleted += (o, e) =>
            {
                _actual = WebView.DocumentTitle;
                Form.Close();
            };
        }

        protected override void When()
        {
            NavigateToStringAndWaitForFormClose(_content);
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        public void DocumentTitleIsExpectedValue()
        {
            _actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    public class DocumentTitleNavigateTest : DocumentTitleNavigateToStringTest
    {
        protected override void Given()
        {
            Expected = "Example Domain";

            base.Given();
        }

        protected override void When()
        {
            NavigateAndWaitForFormClose(TestConstants.Uris.ExampleCom);
        }
    }
}
