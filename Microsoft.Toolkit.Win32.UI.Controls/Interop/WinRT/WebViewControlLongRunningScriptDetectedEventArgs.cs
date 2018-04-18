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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "StopPage")]
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlLongRunningScriptDetectedEventArgs(Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs args) => new WebViewControlLongRunningScriptDetectedEventArgs(args);
    }
}