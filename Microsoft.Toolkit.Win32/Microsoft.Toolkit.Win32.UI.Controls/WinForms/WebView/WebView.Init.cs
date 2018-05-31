// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using Microsoft.Toolkit.Win32.UI.Controls.Interop;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using WebViewControlProcess = Windows.Web.UI.Interop.WebViewControlProcess;
using WebViewControlProcessCapabilityState = Windows.Web.UI.Interop.WebViewControlProcessCapabilityState;
using WebViewControlProcessOptions = Windows.Web.UI.Interop.WebViewControlProcessOptions;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    /// <inheritdoc cref="ISupportInitialize"/>
    public partial class WebView : ISupportInitialize
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
                // Cannot initialize WebView since it is already completely initialized
                throw new InvalidOperationException(DesignerUI.E_WEBVIEW_ALREADY_INITIALIZED);
            }

            if (Initializing)
            {
                // Cannot initialize WebView since it is already being initialized
                throw new InvalidOperationException(DesignerUI.E_WEBVIEW_ALREADY_INITIALIZING);
            }

            _initializationState = InitializationState.IsInitializing;
        }

        void ISupportInitialize.EndInit()
        {
            if (!Initializing)
            {
                // Cannot complete WebView initialization that is not being initialized
                throw new InvalidOperationException(DesignerUI.E_WEBVIEW_NOT_INITIALIZING);
            }

            if (!DesignMode)
            {
                OSVersionHelper.ThrowIfBeforeWindows10April2018();
            }

            try
            {
                Initialize();
            }
            catch (TypeLoadException)
            {
                // Some types are exposed that the designer tries to reflect over, throwing TypeLoadException
                // We're okay to ignore this if we're not in design mode
                if (!DesignMode)
                {
                    throw;
                }
            }
        }

        private void CheckInitialized()
        {
            if (!Initialized)
            {
                // Initialization incomplete
                throw new InvalidOperationException(DesignerUI.E_WEBVIEW_NOT_INITIALIZED);
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
                OSVersionHelper.ThrowIfBeforeWindows10April2018();

                if (!WebViewControlInitialized)
                {
                    Verify.IsNull(Process);

                    // Was not injected via ctor, create using defaults
                    Process = new WebViewControlProcess(new WebViewControlProcessOptions
                    {
                        PrivateNetworkClientServerCapability = _delayedPrivateNetworkEnabled
                                                                    ? WebViewControlProcessCapabilityState.Enabled
                                                                    : WebViewControlProcessCapabilityState.Disabled,
                        EnterpriseId = _delayedEnterpriseId
                    });
                    _webViewControl = Process.CreateWebViewControlHost(Handle, ClientRectangle);
                    SubscribeEvents();

                    // Set values. They could have been changed in the designer
                    IsScriptNotifyAllowed = _delayedIsScriptNotifyAllowed;
                    IsIndexedDBEnabled = _delayedIsIndexDbEnabled;
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
            if (_webViewControl == null)
            {
                return;
            }

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
            if (_webViewControl == null)
            {
                return;
            }

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