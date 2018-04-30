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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Camera Control to preview video. Can subscribe to video frames, software bitmap when they arrive.
    /// </summary>
    public partial class CameraPreview
    {
        /// <summary>
        /// Using a DependencyProperty as the backing store for <see cref="FrameSourceGroupButtonIcon"/>.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty FrameSourceGroupButtonIconProperty =
            DependencyProperty.Register("FrameSourceGroupButtonIcon", typeof(ImageSource), typeof(CameraPreview), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets icon for Frame Source Group Button
        /// </summary>
        public ImageSource FrameSourceGroupButtonIcon
        {
            get { return (ImageSource)GetValue(FrameSourceGroupButtonIconProperty); }
            set { SetValue(FrameSourceGroupButtonIconProperty, value); }
        }

    }
}
