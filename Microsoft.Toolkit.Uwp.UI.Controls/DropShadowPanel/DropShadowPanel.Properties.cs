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

using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="DropShadowPanel"/> control allows the creation of a DropShadow for any Xaml FrameworkElement in markup
    /// making it easier to add shadows to Xaml without having to directly drop down to Windows.UI.Composition APIs.
    /// </summary>
    public sealed partial class DropShadowPanel
    {
        public static bool IsShadowSupported =>
            ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3); // SDK >= 14393

        /// <summary>
        /// Identifies the <see cref="BlurRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BlurRadiusProperty =
             DependencyProperty.Register(nameof(BlurRadius), typeof(double), typeof(DropShadowPanel), new PropertyMetadata(9.0, OnBlurRadiusChanged));

        /// <summary>
        /// Identifies the <see cref="Color"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(nameof(Color), typeof(Color), typeof(DropShadowPanel), new PropertyMetadata(Colors.Black, OnColorChanged));

        /// <summary>
        /// Identifies the <see cref="OffsetX"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OffsetXProperty =
            DependencyProperty.Register(nameof(OffsetX), typeof(double), typeof(DropShadowPanel), new PropertyMetadata(0.0, OnOffsetXChanged));

        /// <summary>
        /// Identifies the <see cref="OffsetY"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OffsetYProperty =
            DependencyProperty.Register(nameof(OffsetY), typeof(double), typeof(DropShadowPanel), new PropertyMetadata(0.0, OnOffsetYChanged));

        /// <summary>
        /// Identifies the <see cref="OffsetZ"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OffsetZProperty =
            DependencyProperty.Register(nameof(OffsetZ), typeof(double), typeof(DropShadowPanel), new PropertyMetadata(0.0, OnOffsetZChanged));

        /// <summary>
        /// Identifies the <see cref="ShadowOpacity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShadowOpacityProperty =
            DependencyProperty.Register(nameof(ShadowOpacity), typeof(double), typeof(DropShadowPanel), new PropertyMetadata(1.0, OnShadowOpacityChanged));

        /// <summary>
        /// Gets or sets the blur radius of the drop shadow.
        /// </summary>
        public double BlurRadius
        {
            get
            {
                return (double)GetValue(BlurRadiusProperty);
            }

            set
            {
                SetValue(BlurRadiusProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the drop shadow.
        /// </summary>
        public Color Color
        {
            get
            {
                return (Color)GetValue(ColorProperty);
            }

            set
            {
                SetValue(ColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the x offset of the drop shadow.
        /// </summary>
        public double OffsetX
        {
            get
            {
                return (double)GetValue(OffsetXProperty);
            }

            set
            {
                SetValue(OffsetXProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the y offset of the drop shadow.
        /// </summary>
        public double OffsetY
        {
            get
            {
                return (double)GetValue(OffsetYProperty);
            }

            set
            {
                SetValue(OffsetYProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the z offset of the drop shadow.
        /// </summary>
        public double OffsetZ
        {
            get
            {
                return (double)GetValue(OffsetZProperty);
            }

            set
            {
                SetValue(OffsetZProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the opacity of the drop shadow.
        /// </summary>
        public double ShadowOpacity
        {
            get
            {
                return (double)GetValue(ShadowOpacityProperty);
            }

            set
            {
                SetValue(ShadowOpacityProperty, value);
            }
        }

        private static void OnBlurRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsShadowSupported)
            {
                ((DropShadowPanel)d).OnBlurRadiusChanged((double)e.NewValue);
            }
        }

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsShadowSupported)
            {
                ((DropShadowPanel)d).OnColorChanged((Color)e.NewValue);
            }
        }

        private static void OnOffsetXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsShadowSupported)
            {
                ((DropShadowPanel)d).OnOffsetXChanged((double)e.NewValue);
            }
        }

        private static void OnOffsetYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsShadowSupported)
            {
                ((DropShadowPanel)d).OnOffsetYChanged((double)e.NewValue);
            }
        }

        private static void OnOffsetZChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsShadowSupported)
            {
                ((DropShadowPanel)d).OnOffsetZChanged((double)e.NewValue);
            }
        }

        private static void OnShadowOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsShadowSupported)
            {
                ((DropShadowPanel)d).OnShadowOpacityChanged((double)e.NewValue);
            }
        }
    }
}
