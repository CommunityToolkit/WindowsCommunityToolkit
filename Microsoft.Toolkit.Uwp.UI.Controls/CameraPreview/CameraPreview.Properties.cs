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
        /// Using a DependencyProperty as the backing store for <see cref="IsFrameSourceGroupButtonVisible "/>.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty IsFrameSourceGroupButtonVisibleProperty =
            DependencyProperty.Register("IsFrameSourceGroupButtonVisible", typeof(bool), typeof(CameraPreview), new PropertyMetadata(true, IsFrameSourceGroupButtonVisibleChanged));

        /// <summary>
        /// Gets or sets a value indicating whether Frame Source Group Button is visible or not
        /// </summary>
        public bool IsFrameSourceGroupButtonVisible
        {
            get { return (bool)GetValue(IsFrameSourceGroupButtonVisibleProperty); }
            set { SetValue(IsFrameSourceGroupButtonVisibleProperty, value); }
        }

        private static void IsFrameSourceGroupButtonVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cameraPreview = d as CameraPreview;
            if (cameraPreview._frameSourceGroupButton != null)
            {
                cameraPreview.SetFrameSourceGroupButtonVisibility();
            }
        }
    }
}
