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

using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

using System;
using System.ComponentModel;

using WebViewControlProcess = Windows.Web.UI.Interop.WebViewControlProcess;
using WebViewControlProcessCapabilityState = Windows.Web.UI.Interop.WebViewControlProcessCapabilityState;
using WebViewControlProcessOptions = Windows.Web.UI.Interop.WebViewControlProcessOptions;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    public partial class WebView
    {
        // Initialization flag for ISupportInitialize
        private InitializationState _initializationState;

        internal WebView(WebViewControlHost webViewControl)
            : this()
        {
            _webViewControl = webViewControl ?? throw new ArgumentNullException(nameof(webViewControl));
            Process = _webViewControl.Process;

            EnsureInitialized();
        }

        private bool Initialized => _initializationState == InitializationState.IsInitialized;
        private bool Initializing => _initializationState == InitializationState.IsInitializing;
        private bool WebViewControlInitialized => _webViewControl != null;
        void ISupportInitialize.BeginInit()
        {
            if (Initialized)
            {
                // TODO: Message
                // Cannot initialize WebView since it is already completely initialized
                throw new InvalidOperationException();
            }

            if (Initializing)
            {
                //TODO: Message
                // Cannot initialize WebView since it is already being initialized
                throw new InvalidOperationException();
            }

            _initializationState = InitializationState.IsInitializing;
        }

        void ISupportInitialize.EndInit()
        {
            if (!Initializing)
            {
                //TODO: Message
                // Cannot complete WebView initialization that is not being initialized
                throw new InvalidOperationException();
            }

            Initialize();
        }

        private void CheckInitialized()
        {
            if (!Initialized)
            {
                // TODO: Message
                // Initialization incomplete
                throw new InvalidOperationException();
            }
        }

        // Ensures this class is initialized. Initialization involves using ISupportInitialize methods
        private void EnsureInitialized()
        {
            // If already disposed, do nothing
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            // If not already initialized and not already initializing
            if (!Initialized && !Initializing)
            {
                ((ISupportInitialize)this).BeginInit();
                ((ISupportInitialize)this).EndInit();
            }
        }

        private void Initialize()
        {
            Verify.AreEqual(_initializationState, InitializationState.IsInitializing);
            Verify.IsFalse(DesignMode);

            // This is causing freezing
            if (!DesignMode)
            {
                OSVersionHelper.ThrowIfBeforeWindows10RS4();

                if (!WebViewControlInitialized)
                {
                    Verify.IsNull(Process);

                    // Was not injected via ctor, create using defaults
                    Process = new WebViewControlProcess(new WebViewControlProcessOptions
                    {
                        // TODO: Set secure default
                        PrivateNetworkClientServerCapability = WebViewControlProcessCapabilityState.Enabled
                    });
                    _webViewControl = Process.CreateWebViewControlHost(Handle, ClientRectangle);
                    SubscribeEvents();
                    // Set values. They could have been changed in the designer
                    IsScriptNotifyAllowed = _delayedIsScriptNotifyAllowed;
                    IsIndexDBEnabled = _delayedIsIndexDbEnabled;
                    IsJavaScriptEnabled = _delayedIsJavaScriptEnabled;

                    // This will cause a navigation
                    Source = _delayedSource;
                }
                else
                {
                    // Already provided control
                    SubscribeEvents();
                }

                _webViewControl.IsVisible = true;
            }

            _initializationState = InitializationState.IsInitialized;
        }
        private void SubscribeEvents()
        {
            if (_webViewControl == null) return;

            _webViewControl.AcceleratorKeyPressed += OnAcceleratorKeyPressed;
            _webViewControl.ContainsFullScreenElementChanged += OnContainsFullScreenElementChanged;
            _webViewControl.ContentLoading += OnContentLoading;
            _webViewControl.DOMContentLoaded += OnDOMContentLoaded;
            _webViewControl.FrameContentLoading += OnFrameContentLoading;
            _webViewControl.FrameDOMContentLoaded += OnFrameDOMContentLoaded;
            _webViewControl.FrameNavigationCompleted += OnFrameNavigationCompleted;
            _webViewControl.FrameNavigationStarting += OnFrameNavigationStarting;
            _webViewControl.LongRunningScriptDetected += OnLongRunningScriptDetected;
            _webViewControl.MoveFocusRequested += OnMoveFocusRequested;
            _webViewControl.NavigationCompleted += OnNavigationCompleted;
            _webViewControl.NavigationStarting += OnNavigationStarting;
            _webViewControl.NewWindowRequested += OnNewWindowRequested;
            _webViewControl.PermissionRequested += OnPermissionRequested;
            _webViewControl.ScriptNotify += OnScriptNotify;
            _webViewControl.UnsafeContentWarningDisplaying += OnUnsafeContentWarningDisplaying;
            _webViewControl.UnsupportedUriSchemeIdentified += OnUnsupportedUriSchemeIdentified;
            _webViewControl.UnviewableContentIdentified += OnUnviewableContentIdentified;
        }

        private void UnsubscribeEvents()
        {
            if (_webViewControl == null) return;

            _webViewControl.AcceleratorKeyPressed -= OnAcceleratorKeyPressed;
            _webViewControl.ContainsFullScreenElementChanged -= OnContainsFullScreenElementChanged;
            _webViewControl.ContentLoading -= OnContentLoading;
            _webViewControl.DOMContentLoaded -= OnDOMContentLoaded;
            _webViewControl.FrameContentLoading -= OnFrameContentLoading;
            _webViewControl.FrameDOMContentLoaded -= OnFrameDOMContentLoaded;
            _webViewControl.FrameNavigationCompleted -= OnFrameNavigationCompleted;
            _webViewControl.FrameNavigationStarting -= OnFrameNavigationStarting;
            _webViewControl.LongRunningScriptDetected -= OnLongRunningScriptDetected;
            _webViewControl.MoveFocusRequested -= OnMoveFocusRequested;
            _webViewControl.NavigationCompleted -= OnNavigationCompleted;
            _webViewControl.NavigationStarting -= OnNavigationStarting;
            _webViewControl.NewWindowRequested -= OnNewWindowRequested;
            _webViewControl.PermissionRequested -= OnPermissionRequested;
            _webViewControl.ScriptNotify -= OnScriptNotify;
            _webViewControl.UnsafeContentWarningDisplaying -= OnUnsafeContentWarningDisplaying;
            _webViewControl.UnsupportedUriSchemeIdentified -= OnUnsupportedUriSchemeIdentified;
            _webViewControl.UnviewableContentIdentified -= OnUnviewableContentIdentified;
        }
    }
}