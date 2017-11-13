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

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Provides common Dependency properties for KeyFrames
    /// </summary>
    public abstract class KeyFrame : DependencyObject
    {
        /// <summary>
        /// Identifies the <see cref="Key"/> dependency property
        /// </summary>
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register(nameof(Key), typeof(double), typeof(KeyFrame), new PropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the key of the key frame
        /// Value should be between 0.0 and 1.0
        /// </summary>
        public double Key
        {
            get { return (double)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }
    }
}
