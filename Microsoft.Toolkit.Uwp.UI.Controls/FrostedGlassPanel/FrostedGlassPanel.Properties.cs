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
        /// Identifies the <see cref="Brush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BrushProperty =
             DependencyProperty.Register(nameof(Brush), typeof(Brush), typeof(FrostedGlassPanel), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 245, 245, 245)), OnBrushChanged));

        /// <summary>
        /// Identifies the <see cref="Transparency"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TransparencyProperty =
             DependencyProperty.Register(nameof(Transparency), typeof(double), typeof(FrostedGlassPanel), new PropertyMetadata(0.5, OnTransparencyChanged));

        /// <summary>
        /// Gets a value indicating whether the platform supports drop shadows.
        /// </summary>
        /// <remarks>
        /// On platforms not supporting drop shadows, this control has no effect.
        /// </remarks>
        public static bool IsSupported =>
            !DesignMode.DesignModeEnabled &&
            ApiInformation.IsTypePresent("Windows.UI.Composition.DropShadow"); // SDK >= 14393

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
        public Brush Brush
        {
            get { return (Brush)GetValue(BrushProperty); }
            set { SetValue(BrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the transparency of the frosted glass.
        /// </summary>
        public double Transparency
        {
            get { return (double)GetValue(TransparencyProperty); }
            set { SetValue(TransparencyProperty, value); }
        }

        private static void OnBlurAmountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsSupported)
            {
                ((FrostedGlassPanel)d).OnBlurAmountChanged((double)e.NewValue);
            }
        }

        private static void OnBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsSupported)
            {
                ((FrostedGlassPanel)d).OnBrushChanged((Brush)e.NewValue);
            }
        }

        private static void OnTransparencyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsSupported)
            {
                ((FrostedGlassPanel)d).OnTransparencyChanged((double)e.NewValue);
            }
        }
    }
}
