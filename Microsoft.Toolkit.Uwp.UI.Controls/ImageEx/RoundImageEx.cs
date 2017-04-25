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

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media;

    /// <summary>
    /// The RoundImageEx control extends the default ImageBrush platform control improving the performance and responsiveness of your Apps.
    /// Source images are downloaded asynchronously showing a load indicator while in progress.
    /// Once downloaded, the source image is stored in the App local cache to preserve resources and load time next time the image needs to be displayed.
    /// </summary>
    [TemplateVisualState(Name = ShowStrokeState, GroupName = StrokeGroup)]
    [TemplateVisualState(Name = StrokeUnloaded, GroupName = StrokeGroup)]
    public partial class RoundImageEx : ImageExBase
    {
        internal const string StrokeGroup = "StrokeStates";
        internal const string ShowStrokeState = "ShowStroke";
        internal const string StrokeUnloaded = "StrokeUnloaded";

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundImageEx"/> class.
        /// </summary>
        public RoundImageEx()
            : base()
        {
            DefaultStyleKey = typeof(RoundImageEx);

            // Changes default Stretching as Uniform doesn't work well for ImageBrush
            Stretch = Stretch.UniformToFill;
            PlaceholderStretch = Stretch.UniformToFill;
        }
    }
}