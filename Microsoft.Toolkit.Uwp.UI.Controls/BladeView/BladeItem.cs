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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The Blade is used as a child in the BladeView
    /// </summary>
    [TemplatePart(Name = "CloseButton", Type = typeof(Button))]
    [TemplatePart(Name = "EnlargeButton", Type = typeof(Button))]
    public partial class BladeItem : ContentControl
    {
        private Button _closeButton;
        private Button _enlargeButton;
        private double _normalModeWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="BladeItem"/> class.
        /// </summary>
        public BladeItem()
        {
            DefaultStyleKey = typeof(BladeItem);

            SizeChanged += OnSizeChanged;
        }

        /// <summary>
        /// Override default OnApplyTemplate to capture child controls
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _closeButton = GetTemplateChild("CloseButton") as Button;
            _enlargeButton = GetTemplateChild("EnlargeButton") as Button;

            if (_closeButton == null)
            {
                return;
            }

            _closeButton.Click -= CloseButton_Click;
            _closeButton.Click += CloseButton_Click;
            _enlargeButton.Click -= EnlargeButton_Click;
            _enlargeButton.Click += EnlargeButton_Click;

        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if(BladeItemMode == BladeItemMode.Normal)
                _normalModeWidth = Width;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        private void EnlargeButton_Click(object sender, RoutedEventArgs e)
        {
            BladeItemMode = BladeItemMode.Normal;
        }
    }
}