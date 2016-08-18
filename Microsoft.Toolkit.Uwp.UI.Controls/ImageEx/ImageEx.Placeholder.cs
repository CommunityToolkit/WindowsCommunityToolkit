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
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The ImageEx control extends the default Image platform control improving the performance and responsiveness of your Apps.
    /// Source images are downloaded asynchronously showing a load indicator while in progress.
    /// Once downloaded, the source image is stored in the App local cache to preserve resources and load time next time the image needs to be displayed.
    /// </summary>
    public partial class ImageEx
    {
        /// <summary>
        /// Identifies the <see cref="PlaceholderSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaceholderSourceProperty = DependencyProperty.Register(
            nameof(PlaceholderSource),
            typeof(ImageSource),
            typeof(ImageEx),
            new PropertyMetadata(default(ImageSource)));

        /// <summary>
        /// Identifies the <see cref="PlaceholderStretch"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaceholderStretchProperty = DependencyProperty.Register(
            nameof(PlaceholderStretch),
            typeof(Stretch),
            typeof(ImageEx),
            new PropertyMetadata(default(Stretch)));

        /// <summary>
        /// Identifies the <see cref="PlaceholderAnimationDuration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaceholderAnimationDurationProperty =
            DependencyProperty.Register(
                nameof(PlaceholderAnimationDuration),
                typeof(Duration),
                typeof(ImageEx),
                new PropertyMetadata(TimeSpan.Zero));

        /// <summary>
        /// Gets or sets the placeholder source.
        /// </summary>
        /// <value>
        /// The placeholder source.
        /// </value>
        public ImageSource PlaceholderSource
        {
            get { return (ImageSource)GetValue(PlaceholderSourceProperty); }
            set { SetValue(PlaceholderSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the placeholder stretch.
        /// </summary>
        /// <value>
        /// The placeholder stretch.
        /// </value>
        public Stretch PlaceholderStretch
        {
            get { return (Stretch)GetValue(PlaceholderStretchProperty); }
            set { SetValue(PlaceholderStretchProperty, value); }
        }

        /// <summary>
        /// Gets or sets the placeholder animation duration.
        /// </summary>
        /// <value>
        /// The placeholder animation duration.
        /// </value>
        public Duration PlaceholderAnimationDuration
        {
            get { return (Duration)GetValue(PlaceholderAnimationDurationProperty); }
            set { SetValue(PlaceholderAnimationDurationProperty, value); }
        }
    }
}
