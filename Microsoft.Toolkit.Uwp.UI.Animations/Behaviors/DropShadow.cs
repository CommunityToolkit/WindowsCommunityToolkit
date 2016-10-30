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

namespace Microsoft.Toolkit.Uwp.UI.Animations.Behaviors
{
    /// <summary>
    /// Performs Drop Shadow.
    /// </summary>
    public class DropShadow : CompositionBehaviorBase
    {
        /// <summary>
        /// Gets or sets Shadow size Y
        /// </summary>
        public double SizeY
        {
            get
            {
                return (double)GetValue(SizeYProperty);
            }
            set { SetValue(SizeYProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="SizeY"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SizeYProperty =
            DependencyProperty.Register(
                nameof(SizeY),
                typeof(double),
                typeof(DropShadow),
                new PropertyMetadata(50d, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets Shadow size X
        /// </summary>
        public double SizeX
        {
            get { return (double)GetValue(SizeXProperty); }
            set { SetValue(SizeXProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="SizeX"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SizeXProperty =
            DependencyProperty.Register(
                nameof(SizeX),
                typeof(double),
                typeof(DropShadow),
                new PropertyMetadata(50d, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets Shadow blur radius
        /// </summary>
        public double BlurRadius
        {
            get { return (double)GetValue(BlurRadiusProperty); }
            set { SetValue(BlurRadiusProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="BlurRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BlurRadiusProperty =
            DependencyProperty.Register(
                nameof(BlurRadius),
                typeof(double),
                typeof(DropShadow),
                new PropertyMetadata(1d, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets Shadow blur Opacity
        /// </summary>
        public double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Opacity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OpacityProperty =
            DependencyProperty.Register(
                nameof(Opacity),
                typeof(double),
                typeof(DropShadow),
                new PropertyMetadata(.5d, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets Shadow color
        /// </summary>
        public Color ShadowColor
        {
            get { return (Color)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ShadowColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.Register(
                nameof(ShadowColor),
                typeof(Color),
                typeof(DropShadow),
                new PropertyMetadata(default(Color), PropertyChangedCallback));

        /// <summary>
        /// Gets or sets Shadow OffsetZ
        /// </summary>
        public double OffsetZ
        {
            get { return (double)GetValue(OffsetZProperty); }
            set { SetValue(OffsetZProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="OffsetZ"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OffsetZProperty =
            DependencyProperty.Register(
                nameof(OffsetZ),
                typeof(double),
                typeof(DropShadow),
                new PropertyMetadata(10d, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets Shadow OffsetY
        /// </summary>
        public double OffsetY
        {
            get { return (double)GetValue(OffsetYProperty); }
            set { SetValue(OffsetYProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="OffsetY"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OffsetYProperty =
            DependencyProperty.Register(
                nameof(OffsetY),
                typeof(double),
                typeof(DropShadow),
                new PropertyMetadata(10d, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets Shadow OffsetX
        /// </summary>
        public double OffsetX
        {
            get { return (double)GetValue(OffsetXProperty); }
            set { SetValue(OffsetXProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="OffsetX"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OffsetXProperty =
            DependencyProperty.Register(
                nameof(OffsetX),
                typeof(double),
                typeof(DropShadow),
                new PropertyMetadata(10d, PropertyChangedCallback));

        /// <summary>
        /// Starts the animation.
        /// </summary>
        public override void StartAnimation()
        {
            AssociatedObject?.DropShadow(
              (float)OffsetX,
              (float)OffsetY,
              (float)OffsetZ,
              ShadowColor,
              (float)Opacity,
              (float)BlurRadius,
              (float)SizeX,
              (float)SizeY)
                .Start();
        }
    }
}
