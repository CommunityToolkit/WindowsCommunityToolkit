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
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The RoundImageEx control extends the default ImageBrush platform control improving the performance and responsiveness of your Apps.
    /// Source images are downloaded asynchronously showing a load indicator while in progress.
    /// Once downloaded, the source image is stored in the App local cache to preserve resources and load time next time the image needs to be displayed.
    /// </summary>
    public partial class RoundImageEx
    {
        // Using a DependencyProperty as the backing store for MaskHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaskHeightProperty =
            DependencyProperty.Register(nameof(MaskHeight), typeof(double), typeof(RoundImageEx), new PropertyMetadata(default(double)));

        // Using a DependencyProperty as the backing store for MaskWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaskWidthProperty =
            DependencyProperty.Register(nameof(MaskWidth), typeof(double), typeof(RoundImageEx), new PropertyMetadata(default(double)));

        // Using a DependencyProperty as the backing store for StrokeThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(RoundImageEx), new PropertyMetadata(default(double)));

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(StrokeProperty), typeof(Brush), typeof(RoundImageEx), new PropertyMetadata(default(Brush)));

        // Using a DependencyProperty as the backing store for ShowPlaceholderStroke.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowPlaceholderStrokeProperty =
            DependencyProperty.Register(nameof(ShowPlaceholderStroke), typeof(bool), typeof(RoundImageEx), new PropertyMetadata(true));

        /// <summary>
        /// Sets the Height of the Ellipse Mask
        /// </summary>
        public double MaskHeight
        {
            get { return (double)GetValue(MaskHeightProperty); }
            set { SetValue(MaskHeightProperty, value); }
        }

        /// <summary>
        /// Sets the Width of the Ellipse Mask
        /// </summary>
        public double MaskWidth
        {
            get { return (double)GetValue(MaskWidthProperty); }
            set { SetValue(MaskWidthProperty, value); }
        }

        /// <summary>
        /// Sets the Stroke Thickness for the Ellipse Mask
        /// </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Sets the Stroke for the Ellipse Mask
        /// </summary>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public bool ShowPlaceholderStroke
        {
            get { return (bool)GetValue(ShowPlaceholderStrokeProperty); }
            set { SetValue(ShowPlaceholderStrokeProperty, value); }
        }
    }
}