// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using WebViewControlAcceleratorKeyPressedEventArgs = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlAcceleratorKeyPressedEventArgs;
using WebViewControlContentLoadingEventArgs = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlContentLoadingEventArgs;
using WebViewControlDOMContentLoadedEventArgs = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlDOMContentLoadedEventArgs;
using WebViewControlLongRunningScriptDetectedEventArgs = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlLongRunningScriptDetectedEventArgs;
using WebViewControlMoveFocusRequestedEventArgs = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlMoveFocusRequestedEventArgs;
using WebViewControlNavigationStartingEventArgs = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationStartingEventArgs;
using WebViewControlNewWindowRequestedEventArgs = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNewWindowRequestedEventArgs;
using WebViewControlPermissionRequestedEventArgs = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlPermissionRequestedEventArgs;
using WebViewControlScriptNotifyEventArgs = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlScriptNotifyEventArgs;
using WebViewControlUnsupportedUriSchemeIdentifiedEventArgs = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs;
using WebViewControlUnviewableContentIdentifiedEventArgs = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlUnviewableContentIdentifiedEventArgs;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    /// <inheritdoc cref="IWebView" />
    public partial class WebView : IWebView
    {
        /// <inheritdoc />
        public event EventHandler<WebViewControlAcceleratorKeyPressedEventArgs> AcceleratorKeyPressed = (sender, args) => { };

        /// <inheritdoc />
        /// <summary>
        /// Occurs when the status of whether the <see cref="T:Microsoft.Toolkit.Win32.UI.Controls.WinForms.WebView" /> current contains a full screen element or not changes.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Declaration of WinRT type")]
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewContainsFullScreenElement)]
        public event EventHandler<object> ContainsFullScreenElementChanged = (sender, args) => { };

        /// <summary>
        /// Occurs when the <see cref="WebView" /> has started loading new content.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewContentLoading)]
        public event EventHandler<WebViewControlContentLoadingEventArgs> ContentLoading = (sender, args) => { };

        /// <summary>
        /// Occurs when the <see cref="WebView"/> finished parsing the current content.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewDomContentLoaded)]
        public event EventHandler<WebViewControlDOMContentLoadedEventArgs> DOMContentLoaded = (sender, args) => { };

        /// <summary>
        /// Occurs when a frame in the <see cref="WebView"/> has started loading new content.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewFrameContentLoading)]
        public event EventHandler<WebViewControlContentLoadingEventArgs> FrameContentLoading = (sender, args) => { };

        /// <summary>
        /// Occurs when a frame in the <see cref="WebView"/> finished parsing its current content.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DOM", Justification ="Name of WinRT type")]
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewFrameDomContentLoaded)]
        public event EventHandler<WebViewControlDOMContentLoadedEventArgs> FrameDOMContentLoaded = (sender, args) => { };

        /// <summary>
        /// Occurs when a frame in the <see cref="WebView"/> finished navigating to new content.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewFrameNavigationCompleted)]
        public event EventHandler<WebViewControlNavigationCompletedEventArgs> FrameNavigationCompleted = (sender, args) => { };

        /// <summary>
        /// Occurs when a frame in the <see cref="WebView"/> navigates to new content.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewFrameNavigationStarting)]
        public event EventHandler<WebViewControlNavigationStartingEventArgs> FrameNavigationStarting = (sender, args) => { };

        /// <summary>
        /// Occurs periodically while the <see cref="WebView"/> executes JavaScript, letting you halt the script.
        /// </summary>
        /// <remarks>
        /// Your app might appear unresponsive while scripts are running. This event provides an opportunity to interrupt a long-running
        /// script. To determine how long the script has been running, check the <see cref="WebViewControlLongRunningScriptDetectedEventArgs.ExecutionTime"/>
        /// property of the <see cref="WebViewControlLongRunningScriptDetectedEventArgs"/> object. To halt the script, set the event args
        /// <see cref="WebViewControlLongRunningScriptDetectedEventArgs.StopPageScriptExecution"/> property to true. The halted script will
        /// not execute again unless it is reloaded during a subseqent <see cref="WebView"/> navigation.
        /// </remarks>
        /// <seealso cref="WebViewControlLongRunningScriptDetectedEventArgs"/>
        /// <seealso cref="WebViewControlLongRunningScriptDetectedEventArgs.ExecutionTime"/>
        /// <seealso cref="WebViewControlLongRunningScriptDetectedEventArgs.StopPageScriptExecution"/>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewLongRunningScriptDetected)]
        public event EventHandler<WebViewControlLongRunningScriptDetectedEventArgs> LongRunningScriptDetected = (sender, args) => { };

        /// <summary>
        /// Occurs when a focus move is requested.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewMoveFocusRequested)]
        public event EventHandler<WebViewControlMoveFocusRequestedEventArgs> MoveFocusRequested = (sender, args) => { };

        /// <summary>
        /// Occurs when the <see cref="WebView"/> control finished navigating to new content.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewNavigationCompleted)]
        public event EventHandler<WebViewControlNavigationCompletedEventArgs> NavigationCompleted = (sender, args) => { };

        /// <summary>
        /// Occurs before the <see cref="WebView"/> navigates to new content.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewNavigationStarting)]
        public event EventHandler<WebViewControlNavigationStartingEventArgs> NavigationStarting = (sender, args) => { };

        /// <summary>eds
        /// Occurs when an action is performed that causes content to be opened in a new window.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewNewWindowRequested)]
        public event EventHandler<WebViewControlNewWindowRequestedEventArgs> NewWindowRequested = (sender, args) => { };

        /// <summary>
        /// Occurs when an action in a <see cref="WebView"/> requires that permission be granted.
        /// </summary>
        /// <remarks>
        /// The types of permission that can be requested are defined in the <see cref="Windows.Web.UI.WebViewControlPermissionType"/> enumeration.
        ///
        /// If you don't handle the <see cref="PermissionRequested"/> event, the <see cref="WebView"/> denies permission by default.
        ///
        /// When you handle a permission request in <see cref="WebView"/>, you get a <see cref="Windows.Web.UI.WebViewControlPermissionRequest"/> object as
        /// the value of the <see cref="M:WebViewControlPermissionRequestedEventArgs.PermissionRequest"/> property. You can call Allow to grant the request,
        /// Deny to deny the request, or Defer to defer the request until a later time.
        /// </remarks>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewPermissionRequested)]
        public event EventHandler<WebViewControlPermissionRequestedEventArgs> PermissionRequested = (sender, args) => { };

        /// <summary>
        /// Occurs when the content contained in the <see cref="WebView"/> control passes a string to the application by using JavaScript.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewScriptNotify)]
        public event EventHandler<WebViewControlScriptNotifyEventArgs> ScriptNotify = (sender, args) => { };

        /// <summary>
        /// Occurs when <see cref="WebView"/> shows a warning page for content that was reported as unsafe by SmartScreen filter.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification ="Declaration of WinRT type")]
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewUnsafeContentWarningDisplaying)]
        public event EventHandler<object> UnsafeContentWarningDisplaying = (sender, args) => { };

        /// <summary>
        /// Occurs when an attempt is made to navigate to a <see cref="Source"/> using a scheme that <see cref="WebView"/> does not support.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewUnsupportedUriSchemeIdentified)]
        public event EventHandler<WebViewControlUnsupportedUriSchemeIdentifiedEventArgs> UnsupportedUriSchemeIdentified = (sender, args) => { };

        /// <summary>
        /// Occurs when <see cref="WebView"/> attempts to download an unsupported file.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unviewable", Justification ="Name from WinRT type")]
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewUnviewableContentIdentified)]
        public event EventHandler<WebViewControlUnviewableContentIdentifiedEventArgs> UnviewableContentIdentified = (sender, args) => { };

        private void OnAcceleratorKeyPressed(object sender, WebViewControlAcceleratorKeyPressedEventArgs args)
        {
            var handler = AcceleratorKeyPressed;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnContainsFullScreenElementChanged(object sender, object args)
        {
            var handler = ContainsFullScreenElementChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnContentLoading(object sender, WebViewControlContentLoadingEventArgs args)
        {
            var handler = ContentLoading;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnDOMContentLoaded(object sender, WebViewControlDOMContentLoadedEventArgs args)
        {
            var handler = DOMContentLoaded;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnFrameContentLoading(object sender, WebViewControlContentLoadingEventArgs args)
        {
            var handler = FrameContentLoading;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnFrameDOMContentLoaded(object sender, WebViewControlDOMContentLoadedEventArgs args)
        {
            var handler = FrameDOMContentLoaded;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnFrameNavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs args)
        {
            var handler = FrameNavigationCompleted;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnFrameNavigationStarting(object sender, WebViewControlNavigationStartingEventArgs args)
        {
            var handler = FrameNavigationStarting;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnLongRunningScriptDetected(object sender, WebViewControlLongRunningScriptDetectedEventArgs args)
        {
            var handler = LongRunningScriptDetected;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnMoveFocusRequested(object sender, WebViewControlMoveFocusRequestedEventArgs args)
        {
            var handler = MoveFocusRequested;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnNavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs args)
        {
            // We could have used
            // if (NavigationCompleted != null) NavigationCompleted(this, args);
            // However, if there is a subscriber and the moment the null check and the call to
            // the event handler by the method is invoked, the subscriber may unsubscribe
            // (e.g. on a different thread) and cause a NullReferenceException.
            // To work around this create a temporarly local variable to store the reference and check that
            var handler = NavigationCompleted;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnNavigationStarting(object sender, WebViewControlNavigationStartingEventArgs args)
        {
            var handler = NavigationStarting;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnNewWindowRequested(object sender, WebViewControlNewWindowRequestedEventArgs args)
        {
            var handler = NewWindowRequested;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnPermissionRequested(object sender, WebViewControlPermissionRequestedEventArgs args)
        {
            var handler = PermissionRequested;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnScriptNotify(object sender, WebViewControlScriptNotifyEventArgs args)
        {
            var handler = ScriptNotify;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnUnsafeContentWarningDisplaying(object sender, object args)
        {
            var handler = UnsafeContentWarningDisplaying;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnUnsupportedUriSchemeIdentified(object sender, WebViewControlUnsupportedUriSchemeIdentifiedEventArgs args)
        {
            var handler = UnsupportedUriSchemeIdentified;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnUnviewableContentIdentified(object sender, WebViewControlUnviewableContentIdentifiedEventArgs args)
        {
            var handler = UnviewableContentIdentified;
            if (handler != null)
            {
                handler(this, args);
            }
        }
    }
}