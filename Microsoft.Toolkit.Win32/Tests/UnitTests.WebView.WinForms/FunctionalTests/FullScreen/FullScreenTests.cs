// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using WindowsInput.Native;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.FullScreen
{
    [TestClass]
    public class WhenFullScreenElement : HostFormWebViewContextSpecification
    {
        private bool _fullScreenEventCalled;
        private bool _fullScreenBeforeEvent;
        private bool _fullScreenAfterEvent;
        private string _content;
        protected override void Given()
        {
            _content = @"
<!DOCTYPE html>
<head><title>WhenFullScreenElement</title></head>
<body>
    <h1>WhenFullScreenElement</h1>
    <button id='fullscreen-button' onclick='requestFullScreen();'>Click For Full Screen</button>
    <div id='state'>starting</div>
    <script>
    window.addEventListener('focus', (e) => {
        window.external.notify('generateKeyPress');
    });

    var requestFullScreen = function() {
                var element;
            var requestMethod;
            var isInFullScreen = document.fullScreenElement || document.mozFullScreen || document.webkitIsFullScreen || document.msIsFullScreen;

            if (!isInFullScreen) {
                element = document.body;
                requestMethod = element.requestFullScreen || element.webkitRequestFullScreen || element.mozRequestFullScreen || element.msRequestFullscreen || element.msRequestFullScreen;
            } else {
                element = document;
                requestMethod = element.cancelFullScreen || element.webkitCancelFullScreen || element.mozCancelFullScreen || element.msCancelFullscreen || element.msCancelFullScreen;
            }

            if (requestMethod) { // Native full screen.
                requestMethod.call(element);
            } else if (typeof window.ActiveXObject !== 'undefined') { // Older IE.
                var wscript = new ActiveXObject('WScript.Shell');
                if (wscript !== null) {
                    wscript.SendKeys('{F11}');
                }
            }
    }
    </script>
</body>
</html>
";

            base.Given();

            WebView.IsJavaScriptEnabled = true;
            WebView.IsScriptNotifyAllowed = true;

            _fullScreenBeforeEvent = WebView.ContainsFullScreenElement;

            WebView.ScriptNotify += (o, e) =>
            {
                WriteLine($"ScriptNotify received: '{e.Value ?? string.Empty}");

                if ("generateKeyPress".Equals(e.Value))
                {
                    Form.Focus();
                    Form.InputSimulator.Keyboard.KeyPress(VirtualKeyCode.TAB);
                    Form.InputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                    Form.InputSimulator.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
                }
            };

            WebView.DOMContentLoaded += (o, e) =>
            {
                WebView.MoveFocus(WebViewControlMoveFocusReason.Programmatic);
            };

            WebView.ContainsFullScreenElementChanged += (o, e) =>
            {
                WriteLine($"{nameof(WebView.ContainsFullScreenElementChanged)}: Args: {e ?? string.Empty}");
                _fullScreenEventCalled = true;
                _fullScreenAfterEvent = WebView.ContainsFullScreenElement;
                Form.Close();
            };
        }

        protected override void When()
        {
            NavigateToStringAndWaitForFormClose(_content);
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Medium)]
        [Ignore("WebView focus unreliable")]
        public void FullScreenEventRaisedWhenFullScreenRequested()
        {
            _fullScreenEventCalled.ShouldBeTrue();
            _fullScreenBeforeEvent.ShouldBeFalse();
            _fullScreenAfterEvent.ShouldBeTrue();
        }
    }
}
