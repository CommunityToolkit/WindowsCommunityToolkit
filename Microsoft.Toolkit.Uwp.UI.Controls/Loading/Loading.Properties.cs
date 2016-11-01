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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Loading control allows to show an loading animation with some xaml in it.
    /// </summary>
    public sealed partial class Loading
    {
        public static readonly DependencyProperty LoadingVerticalAlignmentProperty = DependencyProperty.Register(
            nameof(LoadingVerticalAlignment), typeof(VerticalAlignment), typeof(Loading), new PropertyMetadata(default(VerticalAlignment)));

        public static readonly DependencyProperty LoadingHorizontalAlignmentProperty = DependencyProperty.Register(
            nameof(LoadingHorizontalAlignment), typeof(HorizontalAlignment), typeof(Loading), new PropertyMetadata(default(HorizontalAlignment)));

        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(
            nameof(IsLoading), typeof(bool), typeof(Loading), new PropertyMetadata(default(bool), IsLoadingPropertyChanged));

        public static readonly DependencyProperty LoadingOpacityProperty = DependencyProperty.Register(
            nameof(LoadingOpacity), typeof(double), typeof(Loading), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty LoadingBackgroundProperty = DependencyProperty.Register(
            nameof(LoadingBackground), typeof(SolidColorBrush), typeof(Loading), new PropertyMetadata(default(SolidColorBrush)));

        private Grid _rootGrid;
        private Grid _backgroundGrid;
        private ContentPresenter _contentGrid;

        /// <summary>
        /// Gets or sets loadingVerticalAlignment
        /// </summary>
        public VerticalAlignment LoadingVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(LoadingVerticalAlignmentProperty); }
            set { SetValue(LoadingVerticalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets loadingHorizontalAlignment
        /// </summary>
        public HorizontalAlignment LoadingHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(LoadingHorizontalAlignmentProperty); }
            set { SetValue(LoadingHorizontalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether isLoading
        /// </summary>
        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        /// <summary>
        /// Gets or sets loadingOpacity
        /// </summary>
        public double LoadingOpacity
        {
            get { return (double)GetValue(LoadingOpacityProperty); }
            set { SetValue(LoadingOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets loadingBackground
        /// </summary>
        public SolidColorBrush LoadingBackground
        {
            get { return (SolidColorBrush)GetValue(LoadingBackgroundProperty); }
            set { SetValue(LoadingBackgroundProperty, value); }
        }

        private static void IsLoadingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (d as Loading);
            control?.OnApplyTemplate();
        }
    }
}
