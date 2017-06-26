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
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The HamburgerMenuImageItem provides an image based implementation for HamburgerMenu entries.
    /// </summary>
    public class HamburgerMenuImageItem : HamburgerMenuItem
    {
        /// <summary>
        /// Identifies the <see cref="Thumbnail"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ThumbnailProperty = DependencyProperty.Register(nameof(Thumbnail), typeof(BitmapImage), typeof(HamburgerMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="HamburgerMenuImageItem"/> class.
        /// </summary>
        public HamburgerMenuImageItem()
        {
            DefaultStyleKey = typeof(HamburgerMenuImageItem);
        }

        /// <summary>
        /// Gets or sets a value that specifies a bitmap to display with an Image control.
        /// </summary>
        public BitmapImage Thumbnail
        {
            get
            {
                return (BitmapImage)GetValue(ThumbnailProperty);
            }

            set
            {
                SetValue(ThumbnailProperty, value);
            }
        }
    }
}
