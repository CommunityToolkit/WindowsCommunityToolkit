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

using Microsoft.Toolkit.Uwp.Notifications.Adaptive.Elements;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// New in Creators Update: A progress bar. Only supported on toasts on Desktop, build 15007 or newer.
    /// </summary>
    public sealed class AdaptiveProgressBar : IToastBindingGenericChild
    {
        /// <summary>
        /// Gets or sets an optional title string. Supports data binding.
        /// </summary>
        public BindableString Title { get; set; }

        /// <summary>
        /// Gets or sets the value of the progress bar. Supports data binding. Defaults to 0.
        /// </summary>
        public BindableProgressBarValue Value { get; set; } = AdaptiveProgressBarValue.FromValue(0);

        /// <summary>
        /// Gets or sets an optional string to be displayed instead of the default percentage string. If this isn't provided, something like "70%" will be displayed.
        /// </summary>
        public BindableString ValueStringOverride { get; set; }

        /// <summary>
        /// Gets or sets an optional status string, which is displayed underneath the progress bar. If provided, this string should reflect the status of the download, like "Downloading..." or "Installing..."
        /// </summary>
        public BindableString Status { get; set; }

        internal Element_AdaptiveProgressBar ConvertToElement()
        {
            // If Value not provided, we use 0
            var val = Value;
            if (val == null)
            {
                val = AdaptiveProgressBarValue.FromValue(0);
            }

            return new Element_AdaptiveProgressBar()
            {
                Title = Title?.ToXmlString(),
                Value = val.ToXmlString(),
                ValueStringOverride = ValueStringOverride?.ToXmlString(),
                Status = Status?.ToXmlString()
            };
        }
    }
}
