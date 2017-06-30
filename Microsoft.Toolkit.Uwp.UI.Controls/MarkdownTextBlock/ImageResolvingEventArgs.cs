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
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Arguments for the <see cref="MarkdownTextBlock.ImageResolving"/> event which is called when a url needs to be resolved to a <see cref="ImageSource"/>.
    /// </summary>
    public class ImageResolvingEventArgs : EventArgs
    {
        private readonly IList<TaskCompletionSource<object>> _deferrals;

        internal ImageResolvingEventArgs(string url, string tooltip)
        {
            _deferrals = new List<TaskCompletionSource<object>>();
            Url = url;
            Tooltip = tooltip;
        }

        /// <summary>
        /// Gets the url of the image in the markdown document.
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Gets the tooltip of the image in the markdown document.
        /// </summary>
        public string Tooltip { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this event was handled successfully.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Gets or sets the image to display in the <see cref="MarkdownTextBlock"/>.
        /// </summary>
        public ImageSource Image { get; set; }

        /// <summary>
        /// Informs the <see cref="MarkdownTextBlock"/> that the event handler might run asynchronously.
        /// </summary>
        /// <returns>Deferral</returns>
        public Deferral GetDeferral()
        {
            var task = new TaskCompletionSource<object>();
            _deferrals.Add(task);

            return new Deferral(() =>
            {
                task.SetResult(null);
            });
        }

        /// <summary>
        /// Returns a <see cref="Task"/> that completes when all <see cref="Deferral"/>s have completed.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        internal Task WaitForDeferrals()
        {
            return Task.WhenAll(_deferrals.Select(f => f.Task));
        }
    }
}