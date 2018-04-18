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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Threading.Tasks;

using Windows.Web.UI;
using Windows.Web.UI.Interop;

using Rect = Windows.Foundation.Rect;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Provides a control that hosts HTML content in an app.
    /// </summary>
    /// <remarks>
    /// <see cref="WebViewControlHost"/> navigation events occur in the following order:
    /// <list type="bullet">
    /// <item><see cref="NavigationStarting"/></item>
    /// <item><see cref="ContentLoading"/></item>
    /// <item><see cref="DOMContentLoaded"/></item>
    /// <item><see cref="NavigationCompleted"/></item>
    /// </list>
    /// Similar events occur in the same order for each iframe in the <see cref="WebViewControlHost"/> content:
    /// <list type="bullet">
    /// <item><see cref="FrameNavigationStarting"/></item>
    /// <item><see cref="FrameContentLoading"/></item>
    /// <item><see cref="FrameDOMContentLoaded"/></item>
    /// <item><see cref="FrameNavigationCompleted"/></item>
    /// </list>
    /// </remarks>
    internal sealed class WebViewControlHost : IDisposable
    {
        [SecurityCritical]
        private WebViewControl _webViewControl;

        private bool _webViewControlClosed;

        internal WebViewControlHost(WebViewControl webViewControl)
        {
            Verify.IsNotNull(webViewControl);

            _webViewControl = webViewControl ?? throw new ArgumentNullException(nameof(webViewControl));
            Process = _webViewControl.Process;
            SubscribeEvents();
            SubscribeProcessExited();
        }

        ~WebViewControlHost()
        {
            Dispose(false);
        }

        public event EventHandler<WebViewControlAcceleratorKeyPressedEventArgs> AcceleratorKeyPressed = (sender, args) => { };

        /// <summary>
        /// Occurs when the status of whether the <see cref="WebViewControlHost"/> current contains a full screen element or not changes.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event EventHandler<object> ContainsFullScreenElementChanged = (sender, args) => { };

        /// <summary>
        /// Occurs when the <see cref="WebViewControlHost"/> has started loading new content.
        /// </summary>
        public event EventHandler<WebViewControlContentLoadingEventArgs> ContentLoading = (sender, args) => { };

        /// <summary>
        /// Occurs when the <see cref="WebViewControlHost"/> finished parsing the current content.
        /// </summary>
        // ReSharper disable InconsistentNaming
        public event EventHandler<WebViewControlDOMContentLoadedEventArgs> DOMContentLoaded = (sender, args) => { };

        /// <summary>
        /// Occurs when a frame in the <see cref="WebViewControlHost"/> has started loading new content.
        /// </summary>
        public event EventHandler<WebViewControlContentLoadingEventArgs> FrameContentLoading = (sender, args) => { };

        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Occurs when a frame in the <see cref="WebViewControlHost"/> finished parsing its current content.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DOM")]

        // ReSharper disable InconsistentNaming
        public event EventHandler<WebViewControlDOMContentLoadedEventArgs> FrameDOMContentLoaded = (sender, args) => { };

        /// <summary>
        /// Occurs when a frame in the <see cref="WebViewControlHost"/> finished navigating to new content.
        /// </summary>
        public event EventHandler<WebViewNavigationCompletedEventArgs> FrameNavigationCompleted = (sender, args) => { };

        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Occurs when a frame in the <see cref="WebViewControlHost"/> navigates to new content.
        /// </summary>
        public event EventHandler<WebViewControlNavigationStartingEventArgs> FrameNavigationStarting = (sender, args) => { };

        /// <summary>
        /// Occurs periodically while the <see cref="WebViewControlHost"/> executes JavaScript, letting you halt the script.
        /// </summary>
        /// <remarks>
        /// Your app might appear unresponsive while scripts are running. This event provides an opportunity to interrupt a long-running
        /// script. To determine how long the script has been running, check the <see cref="Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs.ExecutionTime"/>
        /// property of the <see cref="Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs"/> object. To halt the script, set the event args
        /// <see cref="Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs.StopPageScriptExecution"/> property to true. The halted script will
        /// not execute again unless it is reloaded during a subsequent <see cref="WebViewControlHost"/> navigation.
        /// </remarks>
        /// <seealso cref="Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs"/>
        /// <seealso cref="Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs.ExecutionTime"/>
        /// <seealso cref="Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs.StopPageScriptExecution"/>
        public event EventHandler<WebViewControlLongRunningScriptDetectedEventArgs> LongRunningScriptDetected = (sender, args) => { };

        /// <summary>
        /// Occurs when a focus move is requested.
        /// </summary>
        public event EventHandler<WebViewControlMoveFocusRequestedEventArgs> MoveFocusRequested = (sender, args) => { };

        /// <summary>
        /// Occurs when the <see cref="WebViewControlHost"/> control finished navigating to new content.
        /// </summary>
        public event EventHandler<WebViewNavigationCompletedEventArgs> NavigationCompleted = (sender, args) => { };

        /// <summary>
        /// Occurs before the <see cref="WebViewControlHost"/> navigates to new content.
        /// </summary>
        public event EventHandler<WebViewControlNavigationStartingEventArgs> NavigationStarting = (sender, args) => { };

        /// <summary>eds
        /// Occurs when an action is performed that causes content to be opened in a new window.
        /// </summary>
        public event EventHandler<WebViewControlNewWindowRequestedEventArgs> NewWindowRequested = (sender, args) => { };

        /// <summary>
        /// Occurs when an action in a <see cref="WebViewControlHost"/> requires that permission be granted.
        /// </summary>
        /// <remarks>
        /// The types of permission that can be requested are defined in the <see cref="Windows.Web.UI.WebViewControlPermissionType"/> enumeration.
        ///
        /// If you don't handle the <see cref="PermissionRequested"/> event, the <see cref="WebViewControlHost"/> denies permission by default.
        ///
        /// When you handle a permission request in <see cref="WebViewControlHost"/>, you get a <see cref="Windows.Web.UI.WebViewControlPermissionRequest"/> object as
        /// the value of the <see cref="M:WebViewControlPermissionRequestedEventArgs.PermissionRequest"/> property. You can call Allow to grant the request,
        /// Deny to deny the request, or Defer to defer the request until a later time.
        /// </remarks>
        public event EventHandler<WebViewControlPermissionRequestedEventArgs> PermissionRequested = (sender, args) => { };

        /// <summary>
        /// Occurs when the content contained in the <see cref="WebViewControlHost"/> control passes a string to the application by using JavaScript.
        /// </summary>
        public event EventHandler<WebViewControlScriptNotifyEventArgs> ScriptNotify = (sender, args) => { };

        /// <summary>
        /// Occurs when <see cref="WebViewControlHost"/> shows a warning page for content that was reported as unsafe by SmartScreen filter.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event EventHandler<object> UnsafeContentWarningDisplaying = (sender, args) => { };

        /// <summary>
        /// Occurs when an attempt is made to navigate to a <see cref="Source"/> using a scheme that <see cref="WebViewControlHost"/> does not support.
        /// </summary>
        public event EventHandler<WebViewControlUnsupportedUriSchemeIdentifiedEventArgs> UnsupportedUriSchemeIdentified = (sender, args) => { };

        /// <summary>
        /// Occurs when <see cref="WebViewControlHost"/> attempts to download an unsupported file.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unviewable")]
        public event EventHandler<WebViewControlUnviewableContentIdentifiedEventArgs> UnviewableContentIdentified = (sender, args) => { };

        public bool CanGoBack
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);

                if (!IsDisposed && _webViewControl != null)
                {
                    return _webViewControl.CanGoBack;
                }

                return false;
            }
        }

        public bool CanGoForward
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);

                if (!IsDisposed && _webViewControl != null)
                {
                    return _webViewControl.CanGoForward;
                }

                return false;
            }
        }

        public bool ContainsFullScreenElement
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);

                if (!IsDisposed && _webViewControl != null)
                {
                    return _webViewControl.ContainsFullScreenElement;
                }

                return false;
            }
        }

        public string DocumentTitle
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);

                return _webViewControl?.DocumentTitle;
            }
        }

        public bool IsDisposed { get; private set; }

        public bool IsDisposing { get; private set; }

        public bool IsVisible
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);

                return _webViewControl?.IsVisible ?? false;
            }

            set
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);

                _webViewControl.IsVisible = value;
            }
        }

        public Windows.Web.UI.Interop.WebViewControlProcess Process { get; private set; }

        public WebViewControlSettings Settings
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);
                return new WebViewControlSettings(_webViewControl?.Settings);
            }
        }

        public Uri Source
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);

                Uri url = null;

                if (_webViewControl != null)
                {
                    // Current url
                    url = _webViewControl.Source;

                    // When Source set to null or navigating to stream/string, we navigate to "about:blank"
                    // internally. Make sure we return null in those cases.
                    // Note that the current Source may not be "about:blank" yet
                    // Also, we'll (inconsistently) return "about:blank" in some cases
                    if (NavigatingToAboutBlank)
                    {
                        url = null;
                    }
                }

                return url;
            }

            set
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);

                if (_webViewControl != null)
                {
                    Navigate(value);
                }
            }
        }

        /// <summary>
        /// Gets the version of EDGEHTML.DLL used by the control.
        /// </summary>
        /// <value>The version of EDGEHTML.DLL used by the control.</value>
        public Version Version
        {
            get
            {
                if (OSVersionHelper.EdgeExists)
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "edgehtml.dll"));
                    return new Version(
                        versionInfo.FileMajorPart,
                        versionInfo.FileMinorPart,
                        versionInfo.FileBuildPart,
                        versionInfo.FilePrivatePart);
                }

                // Reuse the message, close enough
                throw new InvalidOperationException(DesignerUI.NotSup_Win10RS4);
            }
        }

        internal Guid LastNavigation
        {
            get;
            [SecurityCritical]
            set;
        }

        // Indicates whether we are navigating to "about:blank" internally because Source is set to null or navigating to string
        // Set is SecurityCritical because it is involved in making security decisions
        internal bool NavigatingToAboutBlank
        {
            get;
            [SecurityCritical]
            set;
        }

        public void Close()
        {
            var webViewControlAlreadyClosed = _webViewControlClosed;
            _webViewControlClosed = true;

            // Unsubscribe all events:
            UnsubscribeEvents();
            UnsubscribeProcessExited();

            if (!webViewControlAlreadyClosed)
            {
                Verify.IsNotNull(_webViewControl);

                _webViewControl?.Close();
            }

            _webViewControl = null;
            Process = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public WebViewControlDeferredPermissionRequest GetDeferredPermissionRequestById(uint id)
        {
            Verify.IsFalse(IsDisposed);
            Verify.IsNotNull(_webViewControl);
            if (_webViewControl == null)
            {
                return null;
            }

            _webViewControl.GetDeferredPermissionRequestById(id, out var retval);
            return retval;
        }

        public bool GoBack()
        {
            Verify.IsFalse(IsDisposed);
            Verify.IsNotNull(_webViewControl);

            var retval = _webViewControl != null;
            try
            {
                _webViewControl?.GoBack();
            }
            catch (Exception e)
            {
                if (e.IsSecurityOrCriticalException())
                {
                    throw;
                }

                retval = false;
            }

            return retval;
        }

        public bool GoForward()
        {
            Verify.IsFalse(IsDisposed);
            Verify.IsNotNull(_webViewControl);

            var retval = _webViewControl != null;
            try
            {
                _webViewControl?.GoForward();
            }
            catch (Exception e)
            {
                if (e.IsSecurityOrCriticalException())
                {
                    throw;
                }

                retval = false;
            }

            return retval;
        }

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebViewControl"/> is not yet initialized.</exception>
        public object InvokeScript(string scriptName) => InvokeScriptAsync(scriptName).GetAwaiter().GetResult();

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebViewControl"/> is not yet initialized.</exception>
        public object InvokeScript(string scriptName, params string[] arguments) =>
            InvokeScriptAsync(scriptName, arguments).GetAwaiter().GetResult();

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebViewControl"/> is not yet initialized.</exception>
        public object InvokeScript(string scriptName, IEnumerable<string> arguments) =>
            InvokeScriptAsync(scriptName, arguments).GetAwaiter().GetResult();

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebViewControl"/> is not yet initialized.</exception>
        public Task<string> InvokeScriptAsync(string scriptName) => InvokeScriptAsync(scriptName, null);

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebViewControl"/> is not yet initialized.</exception>
        public Task<string> InvokeScriptAsync(string scriptName, params string[] arguments) => InvokeScriptAsync(scriptName, (IEnumerable<string>)arguments);

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebViewControl"/> is not yet initialized.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scriptName"/> is <see langword="null"/></exception>
        public Task<string> InvokeScriptAsync(string scriptName, IEnumerable<string> arguments)
        {
            Verify.IsFalse(IsDisposed);
            Verify.IsNotNull(_webViewControl);
            Verify.IsNeitherNullNorEmpty(scriptName);

            if (string.IsNullOrEmpty(scriptName))
            {
                throw new ArgumentNullException(nameof(scriptName));
            }

            // TODO: Error message
            if (_webViewControl == null)
            {
                throw new InvalidOperationException();
            }

            // Protect against the cross domain scripting attacks
            // If it is our internal navigation to blank for navigating to null or load string or before navigation has happened, Source will be null
            var currentSource = Source;
            if (currentSource != null)
            {
                Security.DemandWebPermission(currentSource);
            }

            if (_webViewControl != null)
            {
                return _webViewControl.InvokeScriptAsync(scriptName, arguments).AsTask();
            }

            // TODO: Message
            // Cannot invoke script
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Moves the focus.
        /// </summary>
        /// <param name="reason">The reason.</param>
        public void MoveFocus(WebViewControlMoveFocusReason reason)
        {
            _webViewControl?.MoveFocus((Windows.Web.UI.Interop.WebViewControlMoveFocusReason)reason);
        }

        /// <summary>
        /// Loads the document at the location indicated by the specified <see cref="Source"/> into the <see cref="WebViewControlHost"/> control, replacing the previous document.
        /// </summary>
        /// <param name="source">A <see cref="Source"/> representing the URL of the document to load.</param>
        /// <exception cref="ArgumentException">The provided <paramref name="source"/> is a relative URI.</exception>
        public void Navigate(Uri source)
        {
            Verify.IsNotNull(_webViewControl);

            if (_webViewControl != null)
            {
                // Cancel any outstanding navigation
                // TODO: Does this show a cancel page? Can we suppress that?
                _webViewControl.Stop();

                LastNavigation = Guid.NewGuid();

                if (source == null)
                {
                    NavigatingToAboutBlank = true;
                    source = WebViewDefaults.AboutBlankUri;
                }
                else
                {
                    CleanInternalState();
                }

                // Absolute URI only. Not sure what the host would be if using relative
                if (!source.IsAbsoluteUri)
                {
                    // TODO: Error message
                    throw new ArgumentException();
                }

                // TODO: Handle POPUP window
                // TODO: Handle navigation for frame

                // TODO: Security for partial trust (e.g. about:blank is not allowed)
                // If we are navigating to "about:blank" internally as a result of setting source to null
                // or navigating to string, do not demand WebPermission
                if (!NavigatingToAboutBlank)
                {
                    Security.DemandWebPermission(source);
                }

                // TODO: Sanitize URI containing invalid UTF-8 sequences
                try
                {
                    _webViewControl.Navigate(source);
                }
                catch (Exception)
                {
                    // Clear internal state if navigation fails
                    CleanInternalState();

                    throw;
                }
            }
        }

        /// <exception cref="UriFormatException">
        ///                 In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.FormatException" />, instead.
        ///               <paramref name="source" /> is empty.-or- The scheme specified in <paramref name="source" /> is not correctly formed. See <see cref="M:System.Uri.CheckSchemeName(System.String)" />.-or-
        ///               <paramref name="source" /> contains too many slashes.-or- The password specified in <paramref name="source" /> is not valid.-or- The host name specified in <paramref name="source" /> is not valid.-or- The file name specified in <paramref name="source" /> is not valid. -or- The user name specified in <paramref name="source" /> is not valid.-or- The host or authority name specified in <paramref name="source" /> cannot be terminated by backslashes.-or- The port number specified in <paramref name="source" /> is not valid or cannot be parsed.-or- The length of <paramref name="source" /> exceeds 65519 characters.-or- The length of the scheme specified in <paramref name="source" /> exceeds 1023 characters.-or- There is an invalid character sequence in <paramref name="source" />.-or- The MS-DOS path specified in <paramref name="source" /> must start with c:\\.</exception>
        public void Navigate(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                source = WebViewDefaults.AboutBlank;
            }

            if (Uri.TryCreate(source, UriKind.Absolute, out Uri result))
            {
                Navigate(result);
            }
            else
            {
                // TODO: Message
                // Unrecognized URI
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Loads the specified HTML content as a new document.
        /// </summary>
        /// <param name="text">The HTML content to display in the control.</param>
        /// <exception cref="ArgumentNullException"><paramref name="text"/> is <see langword="null"/></exception>
        public void NavigateToString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            _webViewControl?.NavigateToString(text);
        }

        public void Refresh()
        {
            try
            {
                _webViewControl?.Refresh();
            }
            catch (Exception e)
            {
                if (e.IsSecurityOrCriticalException())
                {
                    throw;
                }
            }
        }

        public void Stop()
        {
            try
            {
                _webViewControl?.Stop();
            }
            catch (Exception e)
            {
                if (e.IsSecurityOrCriticalException())
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Updates the location and size of the web view.
        /// </summary>
        /// <param name="bounds">A <see cref="Rect"/> containing numerical values that represent the location and size of the control.</param>
        /// <remarks>Sets the <seealso cref="WebViewControl.Bounds"/> property.</remarks>
        public void UpdateBounds(Rect bounds)
        {
            if (_webViewControl != null)
            {
                _webViewControl.Bounds = bounds;
            }
        }

        [SecurityCritical] // Resets NavigatingToAboutBlank which is used in security decisions
        internal void CleanInternalState()
        {
            NavigatingToAboutBlank = false;
        }

        private void Dispose(bool disposing)
        {
            IsDisposing = true;
            try
            {
                if (disposing)
                {
                    Close();
                }
            }
            finally
            {
                IsDisposing = false;
                IsDisposed = true;
            }
        }

        private void OnAcceleratorKeyPressed(WebViewControlAcceleratorKeyPressedEventArgs args)
        {
            var handler = AcceleratorKeyPressed;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnAcceleratorKeyPressed(WebViewControl sender, Windows.Web.UI.Interop.WebViewControlAcceleratorKeyPressedEventArgs args) => OnAcceleratorKeyPressed(args);

        private void OnContainsFullScreenElementChanged(object args)
        {
            var handler = ContainsFullScreenElementChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnContainsFullScreenElementChanged(IWebViewControl sender, object args) => OnContainsFullScreenElementChanged(args);

        private void OnContentLoading(WebViewControlContentLoadingEventArgs args)
        {
            var handler = ContentLoading;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnContentLoading(IWebViewControl sender, Windows.Web.UI.WebViewControlContentLoadingEventArgs args) => OnContentLoading(args);

        private void OnDOMContentLoaded(WebViewControlDOMContentLoadedEventArgs args)
        {
            var handler = DOMContentLoaded;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnDOMContentLoaded(IWebViewControl sender, Windows.Web.UI.WebViewControlDOMContentLoadedEventArgs args)
        {
            // When Source set to null or navigating to stream/string, we navigate to "about:blank" internally.
            if (NavigatingToAboutBlank)
            {
                Verify.Implies(NavigatingToAboutBlank, Source == null || Source == WebViewDefaults.AboutBlankUri);

                // Make sure we pass null in the event args
                var a = new WebViewControlDOMContentLoadedEventArgs((Uri)null);
                OnDOMContentLoaded(a);
            }
            else
            {
                OnDOMContentLoaded(args);
            }
        }

        private void OnFrameContentLoading(WebViewControlContentLoadingEventArgs args)
        {
            var handler = FrameContentLoading;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnFrameContentLoading(IWebViewControl sender, Windows.Web.UI.WebViewControlContentLoadingEventArgs args) => OnFrameContentLoading(args);

        private void OnFrameDOMContentLoaded(WebViewControlDOMContentLoadedEventArgs args)
        {
            var handler = FrameDOMContentLoaded;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnFrameDOMContentLoaded(IWebViewControl sender, Windows.Web.UI.WebViewControlDOMContentLoadedEventArgs args) => OnFrameDOMContentLoaded(args);

        private void OnFrameNavigationCompleted(WebViewNavigationCompletedEventArgs args)
        {
            var handler = FrameNavigationCompleted;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnFrameNavigationCompleted(IWebViewControl sender, WebViewControlNavigationCompletedEventArgs args)
        {
            // TODO: Need to handle frame navigation like NavigationCompleted?
            OnFrameNavigationCompleted(args);
        }

        private void OnFrameNavigationStarting(WebViewControlNavigationStartingEventArgs args)
        {
            var handler = FrameNavigationStarting;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnFrameNavigationStarting(IWebViewControl sender, Windows.Web.UI.WebViewControlNavigationStartingEventArgs args) => OnFrameNavigationStarting(args);

        private void OnLongRunningScriptDetected(WebViewControlLongRunningScriptDetectedEventArgs args)
        {
            var handler = LongRunningScriptDetected;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnLongRunningScriptDetected(IWebViewControl sender, Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs args) => OnLongRunningScriptDetected(args);

        private void OnMoveFocusRequested(WebViewControlMoveFocusRequestedEventArgs args)
        {
            var handler = MoveFocusRequested;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnMoveFocusRequested(WebViewControl sender, Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs args) => OnMoveFocusRequested(args);

        private void OnNavigationCompleted(WebViewNavigationCompletedEventArgs args)
        {
            // We could have used
            // if (NavigationCompleted != null) NavigationCompleted(this, args);
            // However, if there is a subscriber and the moment the null check and the call to
            // the event handler by the method is invoked, the subscriber may unsubscribe
            // (e.g. on a different thread) and cause a NullReferenceException.
            // To work around this create a temporarily local variable to store the reference and check that
            var handler = NavigationCompleted;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnNavigationCompleted(IWebViewControl sender, WebViewControlNavigationCompletedEventArgs args)
        {
            // When Source set to null or navigating to stream/string, we navigate to "about:blank" internally.
            if (NavigatingToAboutBlank)
            {
                Verify.Implies(NavigatingToAboutBlank, Source == null || Source == WebViewDefaults.AboutBlankUri);

                // Make sure we pass null in the event args
                var a = new WebViewNavigationCompletedEventArgs(args, null);
                OnNavigationCompleted(a);
            }
            else
            {
                OnNavigationCompleted(args);
            }
        }

        private void OnNavigationStarting(WebViewControlNavigationStartingEventArgs args)
        {
            var handler = NavigationStarting;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnNavigationStarting(IWebViewControl sender, Windows.Web.UI.WebViewControlNavigationStartingEventArgs args)
        {
            var newNavigationRequested = false;
            var cancelRequested = false;

            try
            {
                var url = args.Uri;

                // The NavigatingToAboutBlank property indicates whether we are navigating to "about:blank" as a result of navigating
                // to a null source, or stream/string navigation.
                // We set the NavigatingToAboutBlank bit to true in the void Navigate(Uri) method. When the above conditions are true,
                // the NavigatingToAboutBlank is true and the source must be "about:blank"
                if (NavigatingToAboutBlank && url != null && url != new Uri("about:blank"))
                {
                    NavigatingToAboutBlank = false;
                }

                if (!NavigatingToAboutBlank && !Security.CallerHasWebPermission(url))
                {
                    cancelRequested = true;
                }
                else
                {
                    // When Source is set to null or navigating to stream/string, we navigate to "about:blank" internally.
                    // Make sure we pass null in the event args
                    if (NavigatingToAboutBlank)
                    {
                        url = null;
                    }

                    var a = new WebViewControlNavigationStartingEventArgs(args, url);

                    // Launching a navigation from the NavigationStarting event handler causes re-entrancy
                    var lastNavigation = LastNavigation;

                    // Fire navigating event
                    OnNavigationStarting(a);

                    if (LastNavigation != lastNavigation)
                    {
                        newNavigationRequested = true;
                    }

                    cancelRequested = a.Cancel;
                }
            }

            // Disable to suppress FXCop warning since we really do want to catch all exceptions
#pragma warning disable 6502
            catch
            {
                cancelRequested = true;
            }
#pragma warning restore 6502
            finally
            {
                if (cancelRequested && !newNavigationRequested)
                {
                    CleanInternalState();
                }

                if (cancelRequested || newNavigationRequested)
                {
                    args.Cancel = true;
                }
            }
        }

        private void OnNewWindowRequested(WebViewControlNewWindowRequestedEventArgs args)
        {
            var handler = NewWindowRequested;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnNewWindowRequested(IWebViewControl sender, Windows.Web.UI.WebViewControlNewWindowRequestedEventArgs args) => OnNewWindowRequested(args);

        private void OnOnScriptNotify(IWebViewControl sender, Windows.Web.UI.WebViewControlScriptNotifyEventArgs args) => OnScriptNotify(args);

        private void OnPermissionRequested(WebViewControlPermissionRequestedEventArgs args)
        {
            var handler = PermissionRequested;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnPermissionRequested(IWebViewControl sender, Windows.Web.UI.WebViewControlPermissionRequestedEventArgs args) => OnPermissionRequested(args);

        private void OnProcessExited(object sender, object e)
        {
            _webViewControlClosed = true;
            Close();
        }

        private void OnScriptNotify(WebViewControlScriptNotifyEventArgs args)
        {
            var handler = ScriptNotify;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnUnsafeContentWarningDisplaying(object args)
        {
            var handler = UnsafeContentWarningDisplaying;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnUnsafeContentWarningDisplaying(IWebViewControl sender, object args) => OnUnsafeContentWarningDisplaying(args);

        private void OnUnsupportedUriSchemeIdentified(WebViewControlUnsupportedUriSchemeIdentifiedEventArgs args)
        {
            var handler = UnsupportedUriSchemeIdentified;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnUnsupportedUriSchemeIdentified(IWebViewControl sender, Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs args) => OnUnsupportedUriSchemeIdentified(args);

        private void OnUnviewableContentIdentified(WebViewControlUnviewableContentIdentifiedEventArgs args)
        {
            var handler = UnviewableContentIdentified;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnUnviewableContentIdentified(IWebViewControl sender, Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs args) => OnUnviewableContentIdentified(args);

        [SecurityCritical]
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
            _webViewControl.ScriptNotify += OnOnScriptNotify;
            _webViewControl.UnsafeContentWarningDisplaying += OnUnsafeContentWarningDisplaying;
            _webViewControl.UnsupportedUriSchemeIdentified += OnUnsupportedUriSchemeIdentified;
            _webViewControl.UnviewableContentIdentified += OnUnviewableContentIdentified;
        }

        [SecurityCritical]
        private void SubscribeProcessExited()
        {
            if (Process == null)
            {
                return;
            }

            Process.ProcessExited += OnProcessExited;
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
            _webViewControl.ScriptNotify -= OnOnScriptNotify;
            _webViewControl.UnsafeContentWarningDisplaying -= OnUnsafeContentWarningDisplaying;
            _webViewControl.UnsupportedUriSchemeIdentified -= OnUnsupportedUriSchemeIdentified;
            _webViewControl.UnviewableContentIdentified -= OnUnviewableContentIdentified;
        }

        private void UnsubscribeProcessExited()
        {
            if (Process == null)
            {
                return;
            }

            Process.ProcessExited -= OnProcessExited;
        }
    }
}