// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Provides data for the <see cref="IWebView.LongRunningScriptDetected"/> event. This class cannot be inherited.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs"/>
    /// <seealso cref="EventArgs"/>
    public sealed class WebViewControlLongRunningScriptDetectedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs _args;

        internal WebViewControlLongRunningScriptDetectedEventArgs(Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs args)
        {
            _args = args ?? throw new ArgumentNullException(nameof(args));
        }

        /// <summary>
        /// Gets the amount of time that the <see cref="IWebView"/> has been executing a long-running script.
        /// </summary>
        /// <value>The execution time of a long-running script.</value>
        public TimeSpan ExecutionTime => _args.ExecutionTime;

        /// <summary>
        /// Gets or sets a value indicating whether a long-running script executing in a <see cref="IWebView"/> should halt.
        /// </summary>
        /// <value><see langword="true" /> if the long-running script should halt; otherwise, <see langword="false" />.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "StopPage", Justification = "Name from WinRT type")]
        public bool StopPageScriptExecution
        {
            get => _args.StopPageScriptExecution;
            set => _args.StopPageScriptExecution = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs"/> to <see cref="WebViewControlLongRunningScriptDetectedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator WebViewControlLongRunningScriptDetectedEventArgs(Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs args) => ToWebViewControlLongRunningScriptDetectedEventArgs(args);

        /// <summary>
        /// Creates a <see cref="WebViewControlLongRunningScriptDetectedEventArgs"/> from <see cref="Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs"/> instance containing the event data.</param>
        /// <returns>WebViewControlLongRunningScriptDetectedEventArgs.</returns>
        public static WebViewControlLongRunningScriptDetectedEventArgs
            ToWebViewControlLongRunningScriptDetectedEventArgs(
                Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs args) =>
            new WebViewControlLongRunningScriptDetectedEventArgs(args);
    }
}