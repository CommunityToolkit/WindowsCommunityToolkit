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
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The Blade is used as a child in the BladeView
    /// </summary>
    public partial class BladeItem
    {
        /// <summary>
        /// Identifies the <see cref="TitleBarVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleBarVisibilityProperty = DependencyProperty.Register(nameof(TitleBarVisibility), typeof(Visibility), typeof(BladeItem), new PropertyMetadata(default(Visibility)));

        /// <summary>
        /// Identifies the <see cref="TitleBarBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleBarBackgroundProperty = DependencyProperty.Register(nameof(TitleBarBackground), typeof(Brush), typeof(BladeItem), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// Identifies the <see cref="CloseButtonBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CloseButtonBackgroundProperty = DependencyProperty.Register(nameof(CloseButtonBackground), typeof(Brush), typeof(BladeItem), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// Identifies the <see cref="IsOpen"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(BladeItem), new PropertyMetadata(true, IsOpenChangedCallback));

        /// <summary>
        /// Identifies the <see cref="CloseButtonForeground"/> dependency property
        /// </summary>
        public static readonly DependencyProperty CloseButtonForegroundProperty = DependencyProperty.Register(nameof(CloseButtonForeground), typeof(Brush), typeof(BladeItem), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        /// <summary>
        /// Gets or sets the foreground color of the close button
        /// </summary>
        public Brush CloseButtonForeground
        {
            get { return (Brush)GetValue(CloseButtonForegroundProperty); }
            set { SetValue(CloseButtonForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the visibility of the title bar for this blade
        /// </summary>
        public Visibility TitleBarVisibility
        {
            get { return (Visibility)GetValue(TitleBarVisibilityProperty); }
            set { SetValue(TitleBarVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background color of the title bar
        /// </summary>
        public Brush TitleBarBackground
        {
            get { return (Brush)GetValue(TitleBarBackgroundProperty); }
            set { SetValue(TitleBarBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background color of the default close button in the title bar
        /// </summary>
        public Brush CloseButtonBackground
        {
            get { return (Brush)GetValue(CloseButtonBackgroundProperty); }
            set { SetValue(CloseButtonBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this blade is opened
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        private static void IsOpenChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            BladeItem bladeItem = (BladeItem)dependencyObject;
            bladeItem.Visibility = bladeItem.IsOpen ? Visibility.Visible : Visibility.Collapsed;
            bladeItem.VisibilityChanged?.Invoke(bladeItem, bladeItem.Visibility);
        }
    }
}
