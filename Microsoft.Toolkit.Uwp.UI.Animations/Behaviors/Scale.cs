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

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Behaviors
{
    /// <summary>
    /// Performs a scale animation using composition.
    /// </summary>
    public class Scale : CompositionBehaviorBase<UIElement>
    {
        /// <summary>
        /// The scale (x axis) of the associated object
        /// </summary>
        public static readonly DependencyProperty ScaleXProperty = DependencyProperty.Register(nameof(ScaleX), typeof(double), typeof(Scale), new PropertyMetadata(1d, PropertyChangedCallback));

        /// <summary>
        /// The scale (y axis) of the associated object
        /// </summary>
        public static readonly DependencyProperty ScaleYProperty = DependencyProperty.Register(nameof(ScaleY), typeof(double), typeof(Scale), new PropertyMetadata(1d, PropertyChangedCallback));

        /// <summary>
        /// The center (x axis) of scale for associated object
        /// </summary>
        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register(nameof(CenterX), typeof(double), typeof(Scale), new PropertyMetadata(0d, PropertyChangedCallback));

        /// <summary>
        /// The center (y axis) of scale for associated object
        /// </summary>
        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register(nameof(CenterY), typeof(double), typeof(Scale), new PropertyMetadata(0d, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the scale on the x axis.
        /// </summary>
        /// <value>
        /// The scale on the x axis.
        /// </value>
        public double ScaleX
        {
            get { return (double)GetValue(ScaleXProperty); }
            set { SetValue(ScaleXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the scale on the y axis.
        /// </summary>
        /// <value>
        /// The scale on the y axis.
        /// </value>
        public double ScaleY
        {
            get { return (double)GetValue(ScaleYProperty); }
            set { SetValue(ScaleYProperty, value); }
        }

        /// <summary>
        /// Gets or sets the scale (x axis) of the associated object.
        /// </summary>
        /// <value>
        /// The scale (x axis) of the associated object.
        /// </value>
        public double CenterX
        {
            get { return (double)GetValue(CenterXProperty); }
            set { SetValue(CenterXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the scale (y axis) of the associated object.
        /// </summary>
        /// <value>
        /// The scale (y axis) of the associated object.
        /// </value>
        public double CenterY
        {
            get { return (double)GetValue(CenterYProperty); }
            set { SetValue(CenterYProperty, value); }
        }

        /// <summary>
        /// Starts the animation.
        /// </summary>
        public override void StartAnimation()
        {
            AssociatedObject.Scale(
                duration: Duration,
                delay: Delay,
                easingType: EasingType,
                centerX: (float)CenterX,
                centerY: (float)CenterY,
                scaleX: (float)ScaleX,
                scaleY: (float)ScaleY)?
                .Start();
        }
    }
}