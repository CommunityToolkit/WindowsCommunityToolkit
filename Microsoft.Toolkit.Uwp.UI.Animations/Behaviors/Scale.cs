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

namespace Microsoft.Toolkit.Uwp.UI.Animations.Behaviors
{
    /// <summary>
    /// Performs a scale animation using composition.
    /// </summary>
    public class Scale : CompositionBehaviorBase
    {
        /// <summary>
        /// The scale (x axis) of the associated object
        /// </summary>
        public static readonly DependencyProperty ScaleXProperty = DependencyProperty.Register("ScaleX", typeof(double), typeof(Scale), new PropertyMetadata(1d, PropertyChangedCallback));

        /// <summary>
        /// The scale (y axis) of the associated object
        /// </summary>
        public static readonly DependencyProperty ScaleYProperty = DependencyProperty.Register("ScaleY", typeof(double), typeof(Scale), new PropertyMetadata(1d, PropertyChangedCallback));

        /// <summary>
        /// The scale (z axis) of the associated object
        /// </summary>
        public static readonly DependencyProperty ScaleZProperty = DependencyProperty.Register("ScaleZ", typeof(double), typeof(Scale), new PropertyMetadata(1d, PropertyChangedCallback));

        /// <summary>
        /// The center (x axis) of scale for associated object
        /// </summary>
        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register("CenterX", typeof(double), typeof(Scale), new PropertyMetadata(0d, PropertyChangedCallback));

        /// <summary>
        /// The center (y axis) of scale for associated object
        /// </summary>
        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register("CenterY", typeof(double), typeof(Scale), new PropertyMetadata(0d, PropertyChangedCallback));

        /// <summary>
        /// The center (z axis) of scale for associated object
        /// </summary>
        public static readonly DependencyProperty CenterZProperty = DependencyProperty.Register("CenterZ", typeof(double), typeof(Scale), new PropertyMetadata(0d, PropertyChangedCallback));

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
        /// Gets or sets the scale on the z axis.
        /// </summary>
        /// <value>
        /// The scale on the z axis.
        /// </value>
        public double ScaleZ
        {
            get { return (double)GetValue(ScaleZProperty); }
            set { SetValue(ScaleZProperty, value); }
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
        /// Gets or sets the scale (z axis) of the associated object.
        /// </summary>
        /// <value>
        /// The scale (z axis) of the associated object.
        /// </value>
        public double CenterZ
        {
            get { return (double)GetValue(CenterZProperty); }
            set { SetValue(CenterZProperty, value); }
        }

        /// <summary>
        /// Starts the animation.
        /// </summary>
        public override void StartAnimation()
        {
            AssociatedObject.Scale(
                duration: Duration,
                delay: Delay,
                centerX: (float)CenterX,
                centerY: (float)CenterY,
                centerZ: (float)CenterZ,
                scaleX: (float)ScaleX,
                scaleY: (float)ScaleY,
                scaleZ: (float)ScaleZ)?
                .StartAsync();
        }
    }
}