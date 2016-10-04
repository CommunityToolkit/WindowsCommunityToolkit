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
    /// The Blade is used as a child in the BladeControl
    /// </summary>
    [TemplatePart(Name = "CloseButton", Type = typeof(Button))]
    public partial class Blade : Control
    {
        private Button _closeButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="Blade"/> class.
        /// </summary>
        public Blade()
        {
            DefaultStyleKey = typeof(Blade);
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

            _closeButton.Tapped -= CloseButtonOnTap;
            _closeButton.Tapped += CloseButtonOnTap;
        }

        private void CloseButtonOnTap(object sender, RoutedEventArgs routedEventArgs)
        {
            IsOpen = false;
        }
    }
}