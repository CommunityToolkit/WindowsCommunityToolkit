// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Input
{
  [TestClass]
  public class MousewheelTests : HostFormWebViewContextSpecification
  {
    private string _content = @"
<html class='fill'>
<head>
<style>.fill { width:100%; height:100%; border:0; padding:0; margin:0;}</style>
</head>
<body class='fill'>
<h1>Mouse Wheel Chaining Test</h1>
<div id='state'>starting</div>
<script>
document.body.parentElement.addEventListener('wheel', () => {
  document.getElementById('state').textContent = 'wheel found';
  // Use setImmediate to give the main loop time to process
  setImmediate(() => {
    window.external.notify('mouseWheelInputFound');
    document.getElementById('state').textContent = 'wheel reported';
  });
});
// Setup the event listener before signaling that we're ready to see mouse wheel input
window.external.notify('generatedMouseWheelInput');
document.getElementById('state').textContent = 'wheel requested';
</script>
</body>
</html>
";

    protected override void Given()
    {
      base.Given();

      WebView.IsScriptNotifyAllowed = true;
      WebView.IsJavaScriptEnabled = true;
      WebView.NavigationCompleted += (o, e) =>
      {
        if (!WebView.Focused)
        {
          WriteLine("Moving focus to WebView");
          Form.BringToFront();
          Form.Focus();
          WebView.Focus();
          WebView.MoveFocus(WebViewControlMoveFocusReason.Programmatic);
          Form.InputSimulator.Mouse.LeftButtonClick();
        }
        else
        {
          WriteLine("WebView already has focus");
        }
      };
      WebView.ScriptNotify += (o, e) =>
      {
        WriteLine($"ScriptNotify received: {e.Value ?? String.Empty}");

        if ("generatedMouseWheelInput".Equals(e.Value))
        {
          // When the page is loaded and attached its mouse wheel event listener it signals us to send mouse wheel input
          if (!WebView.Focused)
          {
            WriteLine("Moving focus to WebView");
            Form.BringToFront();
            WebView.Focus();
            WebView.MoveFocus(WebViewControlMoveFocusReason.Programmatic);
          }
          else
          {
            WriteLine("WebView already has focus");
          }
          Form.InputSimulator.Mouse.VerticalScroll(-500);

        }
        else if ("mouseWheelInputFound".Equals(e.Value))
        {
          Form.Close();
        }
        else
        {
          Verify.Fail("Unexpected ScriptNotify value: " + e.Value);
        }
      };
    }

    protected override void When()
    {
      NavigateToStringAndWaitForFormClose(_content);
    }

    [TestMethod]
    [Timeout(TestConstants.Timeouts.Short)]
    [Ignore("Cannot reliably focus")]
    public void MouseWheelEventsChained()
    {

    }
  }
}
