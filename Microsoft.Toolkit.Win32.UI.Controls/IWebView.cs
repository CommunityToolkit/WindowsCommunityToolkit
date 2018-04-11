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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Win32.UI.Controls
{
    /// <summary>
    /// Provides a control that hosts HTML content in an app.
    /// </summary>
    /// <seealso cref="IDisposable" />
    /// <remarks>
    /// Subset of functionality from <see cref="Windows.Web.UI.IWebViewControl"/>
    /// </remarks>
    public interface IWebView : IDisposable
    {
        event EventHandler<WebViewControlAcceleratorKeyPressedEventArgs> AcceleratorKeyPressed;

        /// <summary>
        /// Occurs when the status of whether the <see cref="IWebView"/> currently contains a full screen element or not changes.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Matches IWebViewControl interface")]
        event EventHandler<object> ContainsFullScreenElementChanged;

        /// <summary>
        /// Occurs when the <see cref="IWebView"/> has started loading new content.
        /// </summary>
        event EventHandler<WebViewControlContentLoadingEventArgs> ContentLoading;

        /// <summary>
        /// Occurs when the <see cref="IWebView"/> finished parsing the current content.
        /// </summary>
        // ReSharper disable InconsistentNaming
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DOM")]
        event EventHandler<WebViewControlDOMContentLoadedEventArgs> DOMContentLoaded;

        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Occurs when a frame in the <see cref="IWebView"/> has started loading new content.
        /// </summary>
        event EventHandler<WebViewControlContentLoadingEventArgs> FrameContentLoading;

        /// <summary>
        /// Occurs when a frame in the <see cref="IWebView"/> finished parsing its current content.
        /// </summary>
        // ReSharper disable InconsistentNaming
        event EventHandler<WebViewControlDOMContentLoadedEventArgs> FrameDOMContentLoaded;

        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Occurs when a frame in the <see cref="IWebView"/> finished navigating to new content.
        /// </summary>
        event EventHandler<WebViewNavigationCompletedEventArgs> FrameNavigationCompleted;

        /// <summary>
        /// Occurs when a frame in the <see cref="IWebView"/> navigates to new content.
        /// </summary>
        event EventHandler<WebViewControlNavigationStartingEventArgs> FrameNavigationStarting;

        /// <summary>
        /// Occurs periodically while the <see cref="IWebView"/> executes JavaScript, letting you halt the script.
        /// </summary>
        /// <remarks>
        /// Your app might appear unresponsive while scripts are running. This event provides an opportunity to interrupt a long-running
        /// script. To determine how long the script has been running, check the <see cref="WebViewControlLongRunningScriptDetectedEventArgs.ExecutionTime"/>
        /// property of the <see cref="WebViewControlLongRunningScriptDetectedEventArgs"/> object. To halt the script, set the event args
        /// <see cref="WebViewControlLongRunningScriptDetectedEventArgs.StopPageScriptExecution"/> property to true. The halted script will
        /// not execute again unless it is reloaded during a subseqent <see cref="IWebView"/> navigation.
        /// </remarks>
        /// <seealso cref="WebViewControlLongRunningScriptDetectedEventArgs"/>
        /// <seealso cref="WebViewControlLongRunningScriptDetectedEventArgs.ExecutionTime"/>
        /// <seealso cref="WebViewControlLongRunningScriptDetectedEventArgs.StopPageScriptExecution"/>
        event EventHandler<WebViewControlLongRunningScriptDetectedEventArgs> LongRunningScriptDetected;

        /// <summary>
        /// Occurs when a focus move is requested.
        /// </summary>
        event EventHandler<WebViewControlMoveFocusRequestedEventArgs> MoveFocusRequested;

        /// <summary>
        /// Occurs when the <see cref="IWebView"/> control finished navigating to new content.
        /// </summary>
        event EventHandler<WebViewNavigationCompletedEventArgs> NavigationCompleted;

        /// <summary>
        /// Occurs before the <see cref="IWebView"/> navigates to new content.
        /// </summary>
        event EventHandler<WebViewControlNavigationStartingEventArgs> NavigationStarting;

        /// <summary>eds
        /// Occurs when an action is performed that causes content to be opened in a new window.
        /// </summary>
        event EventHandler<WebViewControlNewWindowRequestedEventArgs> NewWindowRequested;

        /// <summary>
        /// Occurs when an action in a <see cref="IWebView"/> requires that permission be granted.
        /// </summary>
        /// <remarks>
        /// The types of permission that can be requested are defined in the <see cref="Windows.Web.UI.WebViewControlPermissionType"/> enumeration.
        ///
        /// If you don't handle the <see cref="PermissionRequested"/> event, the <see cref="IWebView"/> denies permission by default.
        ///
        /// When you handle a permission request in <see cref="IWebView"/>, you get a <see cref="WebViewControlPermissionRequest"/> object as
        /// the value of the <see cref="WebViewControlPermissionRequestedEventArgs.PermissionRequest"/> property. You can call Allow to grant the request,
        /// Deny to deny the request, or Defer to defer the request until a later time.
        /// </remarks>
        event EventHandler<WebViewControlPermissionRequestedEventArgs> PermissionRequested;

        /// <summary>
        /// Occurs when the content contained in the <see cref="IWebView"/> control passes a string to the application by using JavaScript.
        /// </summary>
        event EventHandler<WebViewControlScriptNotifyEventArgs> ScriptNotify;

        /// <summary>
        /// Occurs when <see cref="IWebView"/> shows a warning page for content that was reported as unsafe by SmartScreen filter.
        /// </summary>
        event EventHandler<object> UnsafeContentWarningDisplaying;

        /// <summary>
        /// Occurs when an attempt is made to navigate to a <see cref="Source"/> using a scheme that <see cref="IWebView"/> does not support.
        /// </summary>
        event EventHandler<WebViewControlUnsupportedUriSchemeIdentifiedEventArgs> UnsupportedUriSchemeIdentified;

        /// <summary>
        /// Occurs when <see cref="IWebView"/> attempts to download an unsupported file.
        /// </summary>
        event EventHandler<WebViewControlUnviewableContentIdentifiedEventArgs> UnviewableContentIdentified;

        /// <summary>
        /// Gets a value indicating whether there is at least one page in the backward navigation history.
        /// </summary>
        /// <value><c>true</c> if the <see cref="IWebView"/> can navigate backward; otherwise, <c>false</c>.</value>
        bool CanGoBack { get; }

        /// <summary>
        /// Gets a value indicating whether there is at least one page in the forward navigation history.
        /// </summary>
        /// <value><c>true</c> if the <see cref="IWebView"/> can navigate forward; otherwise, <c>false</c>.</value>
        bool CanGoForward { get; }

        bool ContainsFullScreenElement { get; }

        bool IsIndexDBEnabled { get; set; }

        bool IsJavaScriptEnabled { get; set; }

        bool IsScriptNotifyAllowed { get; set; }

        // TODO: Process
        // WebViewControlProcess Process { get; }

        WebViewControlSettings Settings { get; }

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) source of the HTML content to display in the <see cref="IWebView"/>.
        /// </summary>
        /// <value>
        /// The Uniform Resource Identifier (URI) source of the HTML content to display in the <see cref="IWebView"/>.
        /// </value>
        Uri Source { get; set; }

        /// <summary>
        /// Gets the version of EDGEHTML.DLL used by <see cref="IWebView"/>.
        /// </summary>
        /// <value>The version of EDGEHTML.DLL used by <see cref="IWebView"/>.</value>
        Version Version { get; }

        void Close();

        WebViewControlDeferredPermissionRequest GetDeferredPermissionRequestById(uint id);

        /// <summary>
        /// Navigates the <see cref="IWebView"/> to the previous page in the navigation history.
        /// </summary>
        /// <returns><c>true</c> if the <see cref="IWebView"/> navigation to the previous page in the navigation history is successful; otherwise, <c>false</c>.</returns>
        bool GoBack();

        /// <summary>
        /// Navigates the <see cref="IWebView"/> to the next page in the navigation history.
        /// </summary>
        /// <returns><c>true</c> if the <see cref="IWebView"/> navigation to the next page in the navigation history is successful; otherwise, <c>false</c>.</returns>
        bool GoForward();

        /// <summary>
        /// Executes the specified script function from the currently loaded HTML, with no arguments, as a synchronous action.
        /// </summary>
        /// <param name="scriptName">Name of the script function to invoke.</param>
        /// <returns>When this method returns, the <see cref="object"/> result of the script invocation.</returns>
        /// <remarks>
        /// To prevent malicious code from exploiting your app, be sure to call this method to invoke only scripts that you trust.
        /// </remarks>
        object InvokeScript(string scriptName);

        /// <summary>
        /// Executes the specified script function from the currently loaded HTML, with no arguments, as a synchronous action.
        /// </summary>
        /// <param name="scriptName">Name of the script function to invoke.</param>
        /// <param name="arguments">A string array that packages arguments to the script function.</param>
        /// <returns>When this method returns, the <see cref="object"/> result of the script invocation.</returns>
        /// <remarks>
        /// To prevent malicious code from exploiting your app, be sure to call this method to invoke only scripts that you trust.
        /// </remarks>
        object InvokeScript(string scriptName, params string[] arguments);

        /// <summary>
        /// Executes the specified script function from the currently loaded HTML, with no arguments, as a synchronous action.
        /// </summary>
        /// <param name="scriptName">Name of the script function to invoke.</param>
        /// <param name="arguments">A string array that packages arguments to the script function.</param>
        /// <returns>When this method returns, the <see cref="object"/> result of the script invocation.</returns>
        /// <remarks>
        /// To prevent malicious code from exploiting your app, be sure to call this method to invoke only scripts that you trust.
        /// </remarks>
        object InvokeScript(string scriptName, IEnumerable<string> arguments);

        /// <summary>
        /// Executes the specified script function from the currently loaded HTML, with no arguments, as an asynchronous action.
        /// </summary>
        /// <param name="scriptName">Name of the script function to invoke.</param>
        /// <returns>When this method returns, the <see cref="object"/> result of the script invocation.</returns>
        /// <remarks>
        /// To prevent malicious code from exploiting your app, be sure to call this method to invoke only scripts that you trust.
        /// The invoked script can only return string values.
        /// </remarks>
        Task<string> InvokeScriptAsync(string scriptName);

        /// <summary>
        /// Executes the specified script function from the currently loaded HTML, with no arguments, as an asynchronous action.
        /// </summary>
        /// <param name="scriptName">Name of the script function to invoke.</param>
        /// <param name="arguments">A string array that packages arguments to the script function.</param>
        /// <returns>When this method returns, the <see cref="object"/> result of the script invocation.</returns>
        /// <remarks>
        /// To prevent malicious code from exploiting your app, be sure to call this method to invoke only scripts that you trust.
        /// The invoked script can only return string values.
        /// </remarks>
        Task<string> InvokeScriptAsync(string scriptName, params string[] arguments);

        /// <summary>
        /// Executes the specified script function from the currently loaded HTML, with no arguments, as an asynchronous action.
        /// </summary>
        /// <param name="scriptName">Name of the script function to invoke.</param>
        /// <param name="arguments">A string array that packages arguments to the script function.</param>
        /// <returns>When this method returns, the <see cref="object"/> result of the script invocation.</returns>
        /// <remarks>
        /// To prevent malicious code from exploiting your app, be sure to call this method to invoke only scripts that you trust.
        /// The invoked script can only return string values.
        /// </remarks>
        Task<string> InvokeScriptAsync(string scriptName, IEnumerable<string> arguments);

        /// <summary>
        /// Moves the focus.
        /// </summary>
        /// <param name="reason">The reason.</param>
        void MoveFocus(WebViewControlMoveFocusReason reason);

        /// <summary>
        /// Loads the HTML content at the specified Uniform Resource Identifier (URI).
        /// </summary>
        /// <param name="source">The Uniform Resource Identifier (URI) to load.</param>
        /// <seealso cref="Uri"/>
        /// <see cref="Navigate(Uri)"/> is asynchronous. Use the <see cref="NavigationCompleted"/> event to detect when
        /// navigation has completed.
        void Navigate(Uri source);

        /// <summary>
        /// Loads the HTML content at the specified Uniform Resource Identifier (URI).
        /// </summary>
        /// <param name="source">The Uniform Resource Identifier (URI) to load.</param>
        /// <see cref="Navigate(System.String)"/> is asynchronous. Use the <see cref="NavigationCompleted"/> event to detect when
        /// navigation has completed.
        void Navigate(string source);

        /// <summary>
        /// Loads the specified HTML content as a new document.
        /// </summary>
        /// <param name="text">The HTML content to display in the <see cref="IWebView"/>.</param>
        /// <remarks>
        /// <see cref="NavigateToString"/> is asynchronous. Use the <see cref="NavigationCompleted"/> event to detect when
        /// navigation has completed.
        ///
        /// <see cref="NavigateToString"/> supports content with references to external files such as CSS, scripts, images,
        ///  and fonts. However, it does not provide a way to generate or provide these resources programatically.
        /// </remarks>
        void NavigateToString(string text);

        /// <summary>
        /// Reloads the current <see cref="Source"/> in the <see cref="IWebView"/>.
        /// </summary>
        /// <remarks>If the current source was loaded via <see cref="Navigate(Uri)"/>, this method reloads the file without forced cache validation by sending a <c>Pragma:no-cache</c> header to the server.</remarks>
        void Refresh();

        /// <summary>
        /// Halts the current <see cref="IWebView"/> navigation or download.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Stop")]
        void Stop();

        // TODO: ObjectForScripting
        // TODO: Refresh with option (e.g. bypass cache)
        // TODO: Navigate with new window
        // TODO: Navigate target frame
        // TODO: Navigate with POST
        // TODO: Navigate with headers
        // TODO: Print
    }
}