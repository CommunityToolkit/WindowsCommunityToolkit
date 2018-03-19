﻿// ******************************************************************
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

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Properties for the <see cref="RadialGradientBrush"/>
    /// </summary>
    public partial class RadialGradientBrush
    {
        /// <summary>
        /// Gets or sets a <see cref="AlphaMode"/> enumeration that specifies the way in which an alpha channel affects color channels.  The default is <see cref="AlphaMode.Straight"/> for compatibility with WPF.
        /// </summary>
        public AlphaMode AlphaMode
        {
            get { return (AlphaMode)GetValue(AlphaModeProperty); }
            set { SetValue(AlphaModeProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="AlphaMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AlphaModeProperty =
            DependencyProperty.Register(nameof(AlphaMode), typeof(AlphaMode), typeof(RadialGradientBrush), new PropertyMetadata(AlphaMode.Straight, new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Gets or sets a <see cref="ColorInterpolationMode"/> enumeration that specifies how the gradient's colors are interpolated.  The default is <see cref="ColorInterpolationMode.SRgbLinearInterpolation"/>.
        /// </summary>
        public ColorInterpolationMode ColorInterpolationMode
        {
            get { return (ColorInterpolationMode)GetValue(ColorInterpolationModeProperty); }
            set { SetValue(ColorInterpolationModeProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ColorInterpolationMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorInterpolationModeProperty =
            DependencyProperty.Register(nameof(ColorInterpolationMode), typeof(ColorInterpolationMode), typeof(RadialGradientBrush), new PropertyMetadata(ColorInterpolationMode.SRgbLinearInterpolation, new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Gets or sets the brush's gradient stops.
        /// </summary>
        public GradientStopCollection GradientStops
        {
            get { return (GradientStopCollection)GetValue(GradientStopsProperty); }
            set { SetValue(GradientStopsProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="GradientStops"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GradientStopsProperty =
            DependencyProperty.Register(nameof(GradientStops), typeof(GradientStopCollection), typeof(RadialGradientBrush), new PropertyMetadata(null, new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Gets or sets the center of the outermost circle of the radial gradient.  The default is 0.5,0.5.
        /// </summary>
        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Center"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register(nameof(Center), typeof(Point), typeof(RadialGradientBrush), new PropertyMetadata(new Point(0.5, 0.5), new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Gets or sets the location of the two-dimensional focal point that defines the beginning of the gradient.  The default is 0.5,0.5.
        /// </summary>
        public Point GradientOrigin
        {
            get { return (Point)GetValue(GradientOriginProperty); }
            set { SetValue(GradientOriginProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="GradientOrigin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GradientOriginProperty =
            DependencyProperty.Register(nameof(GradientOrigin), typeof(Point), typeof(RadialGradientBrush), new PropertyMetadata(new Point(0.5, 0.5), new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Gets or sets the horizontal radius of the outermost circle of the radial gradient.  The default is 0.5.
        /// </summary>
        public double RadiusX
        {
            get { return (double)GetValue(RadiusXProperty); }
            set { SetValue(RadiusXProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="RadiusX"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RadiusXProperty =
            DependencyProperty.Register(nameof(RadiusX), typeof(double), typeof(RadialGradientBrush), new PropertyMetadata(0.5, new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Gets or sets the vertical radius of the outermost circle of the radial gradient.  The default is 0.5.
        /// </summary>
        public double RadiusY
        {
            get { return (double)GetValue(RadiusYProperty); }
            set { SetValue(RadiusYProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="RadiusX"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RadiusYProperty =
            DependencyProperty.Register(nameof(RadiusY), typeof(double), typeof(RadialGradientBrush), new PropertyMetadata(0.5, new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Gets or sets the type of spread method that specifies how to draw a gradient that starts or ends inside the bounds of the object to be painted.
        /// </summary>
        public GradientSpreadMethod SpreadMethod
        {
            get { return (GradientSpreadMethod)GetValue(SpreadMethodProperty); }
            set { SetValue(SpreadMethodProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="SpreadMethod"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SpreadMethodProperty =
            DependencyProperty.Register(nameof(SpreadMethod), typeof(GradientSpreadMethod), typeof(RadialGradientBrush), new PropertyMetadata(GradientSpreadMethod.Pad, new PropertyChangedCallback(OnPropertyChanged)));
    }
}
