// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using WindowsInput.Native;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.AcceleratorKeys
{
    [TestClass]
    public class WebControlReceiveTunnelAndBubble : HostFormWebViewContextSpecification
    {
        private int _inputCount;
        private RoutedInputInfo[] _expectedRoutedInput;
        private RoutedInputInfo[] _actualRoutedInput;
        private string _content = @"
<html>
<head>
<script>
document.addEventListener('DOMContentLoaded', () => {
  document.addEventListener('keydown', (e) => {
    document.getElementById('state').textContent = 'keydown found';
    setImmediate(() => {
      window.external.notify('keyboardInputFound');
      document.getElementById('state').textContent = 'keydown reported';
    });
  });
  document.addEventListener('keyup', (e) => {
    document.getElementById('state').textContent = 'keyup found';
    setImmediate(() => {
      window.external.notify('keyboardInputFound');
      document.getElementById('state').textContent = 'keyup reported';
    });
  });
  window.addEventListener('focus', (e) => {
    window.external.notify('generateKeyPress');
  });
});
</script>
</head>
<body>
  <div id='state'>starting</div>
</body>
</html>
";

        protected override void Given()
        {
            _expectedRoutedInput = new[]
            {
                new RoutedInputInfo{RoutingStage = WebViewControlAcceleratorKeyRoutingStage.Tunneling, Key = VirtualKey.Control, IsKeyUp = false, ShouldHandle = false},
                new RoutedInputInfo{RoutingStage = WebViewControlAcceleratorKeyRoutingStage.Bubbling, Key = VirtualKey.Control, IsKeyUp = false, ShouldHandle = false},
                new RoutedInputInfo{RoutingStage = WebViewControlAcceleratorKeyRoutingStage.Tunneling, Key = VirtualKey.A, IsKeyUp = false, ShouldHandle = false},
                // The WebView actually handles the KeyDown for CTRL+A, so the app will not see the bubble phase
                // Including it here in a comment for completeness and to make it clear that this is intentional
                //new RoutedInputInfo{routingStage = WebViewControlAcceleratorKeyRoutingStage.Bubbling, virtualKey = VirtualKey.A, isKeyUp = false, shouldHandle = false},
                new RoutedInputInfo{RoutingStage = WebViewControlAcceleratorKeyRoutingStage.Tunneling, Key = VirtualKey.A, IsKeyUp = true, ShouldHandle = false},
                new RoutedInputInfo{RoutingStage = WebViewControlAcceleratorKeyRoutingStage.Bubbling, Key = VirtualKey.A, IsKeyUp = true, ShouldHandle = false},
                new RoutedInputInfo{RoutingStage = WebViewControlAcceleratorKeyRoutingStage.Tunneling, Key = VirtualKey.Control, IsKeyUp = true, ShouldHandle = false},
                new RoutedInputInfo{RoutingStage = WebViewControlAcceleratorKeyRoutingStage.Bubbling, Key = VirtualKey.Control, IsKeyUp = true, ShouldHandle = false}
            };

            _actualRoutedInput = new RoutedInputInfo[_expectedRoutedInput.Length];

            base.Given();
            WebView.IsScriptNotifyAllowed = true;
            WebView.IsJavaScriptEnabled = true;
            WebView.NavigationCompleted += (o, e) =>
            {
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
            };
            WebView.ScriptNotify += (o, e) =>
            {
                WriteLine($"ScriptNotify received: '{e.Value ?? string.Empty}'");

                if ("generateKeyPress".Equals(e.Value))
                {
                    Form.InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A);
                    WriteLine("Keyboard input generated.");
                }
                else if ("keyboardInputFound".Equals(e.Value))
                {
                    WriteLine("Keyboard input received in WebView");
                }
            };
            WebView.AcceleratorKeyPressed += (o, e) =>
            {
                WriteLine("Received routed accelerator event");
                _actualRoutedInput[_inputCount] = new RoutedInputInfo()
                {
                    RoutingStage = (WebViewControlAcceleratorKeyRoutingStage)e.RoutingStage,
                    Key = (VirtualKey)e.VirtualKey,
                    IsKeyUp = e.EventType == CoreAcceleratorKeyEventType.KeyUp
                };

                _inputCount++;

                if (_expectedRoutedInput.Length == _inputCount)
                {
                    Form.Close();
                }

            };

        }

        protected override void When()
        {
            NavigateToStringAndWaitForFormClose(_content);
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longer)]
        [Ignore("Cannot reliably focus")]
        public void InputsAreInTheExpectedOrder()
        {
            _actualRoutedInput.ShouldEqual(_expectedRoutedInput);
        }
    }

    public struct RoutedInputInfo
    {
        public WebViewControlAcceleratorKeyRoutingStage RoutingStage { get; set; }
        public VirtualKey Key { get; set; }
        public bool IsKeyUp { get; set; }
        public bool ShouldHandle { get; set; }

        public override string ToString()
        {
            return $"Stage: {RoutingStage}, VirtualKey: {Key}, IsDown: {!IsKeyUp}, ShouldHandle: {ShouldHandle}";
        }
    }
}
