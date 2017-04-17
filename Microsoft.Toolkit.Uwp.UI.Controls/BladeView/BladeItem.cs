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

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The Blade is used as a child in the BladeView
    /// </summary>
    [TemplatePart(Name = "CloseButton", Type = typeof(Button))]
    public partial class BladeItem : ContentControl
    {
        private Button _closeButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="BladeItem"/> class.
        /// </summary>
        public BladeItem()
        {
            DefaultStyleKey = typeof(BladeItem);
        }

        /// <summary>
        /// Override default OnApplyTemplate to capture child controls
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _closeButton = GetTemplateChild("CloseButton") as Button;

            if (_closeButton == null)
            {
                return;
            }

            _closeButton.Click -= CloseButton_Click;
            _closeButton.Click += CloseButton_Click;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }
    }
}