// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.InvokeScript
{
    [TestClass]
    public class InvokeScriptAsyncNoArgumentsTests : HostFormWebViewContextSpecification
    {
        private static string _expected = "exampleReturnValue";
        private string _actual;
        private readonly string _content = @"
<h1>" + nameof(InvokeScriptAsyncNoArgumentsTests) + @"</h1>
<script>
var getTextContent = function() {
  return '" + _expected + @"';
}
</script>
";

        protected override void Given()
        {
            base.Given();

            WebView.NavigationCompleted += (a, b) =>
            {
                _actual = WebView.InvokeScriptAsync("getTextContent", null).GetAwaiter().GetResult();
                Form.Close();
            };
        }

        protected override void When()
        {
            NavigateToStringAndWaitForFormClose(_content);
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        [Ignore("Causing test run to abort")]
        public void InvokedScriptReturnedExpectedValue()
        {
            _actual.ShouldEqual(_expected);
        }
    }

    [TestClass]
    public class InvokeScriptNoArgumentsTests : HostFormWebViewContextSpecification
    {
        private static string _expected = "exampleReturnValue";
        private string _actual;
        private readonly string _content = @"
<h1>" + nameof(InvokeScriptNoArgumentsTests) + @"</h1>
<script>
var getTextContent = function() {
  return '" + _expected + @"';
}
</script>
";

        protected override void Given()
        {
            base.Given();

            WebView.NavigationCompleted += (a, b) =>
            {
                _actual = (string)WebView.InvokeScript("getTextContent", null);
                Form.Close();
            };
        }

        protected override void When()
        {
            NavigateToStringAndWaitForFormClose(_content);
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        [Ignore("Causing test run to abort")]
        public void InvokedScriptReturnedExpectedValue()
        {
            _actual.ShouldEqual(_expected);
        }
    }

    [TestClass]
    public class InvokeScriptAsyncOneArgumentTests : HostFormWebViewContextSpecification
    {
        private static string _expected = "exampleParameter";
        private string _actual;
        private readonly string _content = @"
<h1>" + nameof(InvokeScriptAsyncOneArgumentTests) + @"</h1>
<script>
function echoOneArgument(argument) {
  return argument;
}
</script>
";

        protected override void Given()
        {
            base.Given();

            WebView.NavigationCompleted += (a, b) =>
            {
                WriteLine($"Calling {nameof(WebView.InvokeScriptAsync)}");
#pragma warning disable 4014
                NewMethod(a as UI.Controls.WinForms.WebView);
#pragma warning restore 4014
            };
        }

        private async Task NewMethod(UI.Controls.WinForms.WebView a)
        {
            _actual = await a.InvokeScriptAsync("echoOneArgument", "exampleParameter").ConfigureAwait(false);
            Form.Close();
        }

        protected override void When()
        {
            NavigateToStringAndWaitForFormClose(_content);
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        [Ignore("Causing test run to abort")]
        public void InvokedScriptReturnedExpectedValue()
        {
            _actual.ShouldEqual(_expected);
        }
    }

    [TestClass]
    public class InvokeScriptOneArgumentTests : HostFormWebViewContextSpecification
    {
        private static string _expected = "exampleParameter";
        private string _actual;
        private readonly string _content = @"
<h1>" + nameof(InvokeScriptOneArgumentTests) + @"</h1>
<script>
function echoOneArgument(argument) {
  return argument;
}
</script>
";

        protected override void Given()
        {
            base.Given();

            WebView.NavigationCompleted += (a, b) =>
            {
                _actual = (string)WebView.InvokeScript("echoOneArgument", "exampleParameter");
                Form.Close();
            };
        }

        protected override void When()
        {
            NavigateToStringAndWaitForFormClose(_content);
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        [Ignore("Causing test run to abort")]
        public void InvokedScriptReturnedExpectedValue()
        {
            _actual.ShouldEqual(_expected);
        }
    }
}
