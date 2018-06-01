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
