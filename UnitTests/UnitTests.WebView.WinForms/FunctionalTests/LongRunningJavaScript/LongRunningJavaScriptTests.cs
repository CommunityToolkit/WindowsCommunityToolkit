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

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.LongRunningJavaScript
{
    [TestClass]
    public class LongRunningJavaScriptTests : HostFormWebViewContextSpecification
    {
        private string _result;
        private bool _slowEventRaised;
        private string _content = @"
<h1>TEST</h1>
<script>
function mySlowFunction(baseNumber) {
  console.time('mySlowFunction');
  var result = 0,
      i;

  for(i = Math.pow(baseNumber, 10); i >= 0; i--) {
    result += Math.atan(i) * Math.tan(i);
  }

  console.timeEnd('mySlowFunction');
  return result;
};
</script>
";

        protected override void Given()
        {
            base.Given();
            WebView.IsJavaScriptEnabled = true;

            // BUG: The content causes browsers to show prompt when loaded externally without a problem
            WebView.LongRunningScriptDetected += (o, e) =>
            {
                WriteLine($"LongRunningScriptDetected: {e.ExecutionTime}");
                _slowEventRaised = true;
                e.StopPageScriptExecution = true;

                Form.Close();
            };

            WebView.NavigationCompleted += (o, e) =>
                {
                    _result = WebView.InvokeScriptAsync("mySlowFunction", "3000000").Result;
                    Form.Close();
                };
        }

        protected override void When()
        {
            NavigateToStringAndWaitForFormClose(_content);
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        [Ignore("LongRunningScriptDetected event is not raised")]
        public void LongRunningJavaScriptEventRaised()
        {
            _slowEventRaised.ShouldBeTrue();
            _result.ShouldNotBeNull();
        }
    }
}
