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

using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="FrostedGlassPanel"/> control allows user to create layout control that enables tint and blur
    /// </summary>
    public partial class FrostedGlassPanel
    {
        /// <summary>
        /// Identifies the <see cref="BlurAmount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BlurAmountProperty =
             DependencyProperty.Register(nameof(BlurAmount), typeof(double), typeof(FrostedGlassPanel), new PropertyMetadata(15.0, OnBlurAmountChanged));

        /// <summary>
        /// Identifies the <see cref="GlassColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GlassColorProperty =
             DependencyProperty.Register(nameof(GlassColor), typeof(Brush), typeof(FrostedGlassPanel), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 245, 245, 245)), OnGlassColorChanged));

        /// <summary>
        /// Identifies the <see cref="GlassTransparency"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GlassTransparencyProperty =
             DependencyProperty.Register(nameof(GlassTransparency), typeof(double), typeof(FrostedGlassPanel), new PropertyMetadata(0.5, OnGlassTransparencyChanged));

        /// <summary>
        /// Gets a value indicating whether the platform supports drop shadows.
        /// </summary>
        /// <remarks>
        /// On platforms not supporting drop shadows, this control has no effect.
        /// </remarks>
        public static bool IsSupported =>
            !DesignMode.DesignModeEnabled &&
            ApiInformation.IsTypePresent("Windows.UI.Composition.CompositionBackdropBrush"); // SDK >= 14393

        /// <summary>
        /// Gets or sets the blur amount of the frosted glass.
        /// </summary>
        public double BlurAmount
        {
            get { return (double)GetValue(BlurAmountProperty); }
            set { SetValue(BlurAmountProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the frosted glass.
        /// </summary>
        public Brush GlassColor
        {
            get { return (Brush)GetValue(GlassColorProperty); }
            set { SetValue(GlassColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the transparency of the frosted glass.
        /// </summary>
        public double GlassTransparency
        {
            get { return (double)GetValue(GlassTransparencyProperty); }
            set { SetValue(GlassTransparencyProperty, value); }
        }

        private static void OnBlurAmountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsSupported)
            {
                ((FrostedGlassPanel)d).OnBlurAmountChanged((double)e.NewValue);
            }
        }

        private static void OnGlassColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsSupported)
            {
                ((FrostedGlassPanel)d).OnGlassColorChanged((Brush)e.NewValue);
            }
        }

        private static void OnGlassTransparencyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsSupported)
            {
                ((FrostedGlassPanel)d).OnGlassTransparencyChanged((double)e.NewValue);
            }
        }
    }
}
