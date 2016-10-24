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
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Defines the properties for the <see cref="DraggableContentControl"/> control.
    /// </summary>
    public partial class DraggableContentControl
    {
        /// <summary>
        /// Defines the dependency property for the <see cref="IsScalingEnabled"/>.
        /// </summary>
        public static readonly DependencyProperty IsScalingEnabledProperty =
            DependencyProperty.Register(
                nameof(IsScalingEnabled),
                typeof(bool),
                typeof(DraggableContentControl),
                new PropertyMetadata(false));

        /// <summary>
        /// Defines the dependency property for the <see cref="IsRotatingEnabled"/>.
        /// </summary>
        public static readonly DependencyProperty IsRotatingEnabledProperty =
            DependencyProperty.Register(
                nameof(IsRotatingEnabled),
                typeof(bool),
                typeof(DraggableContentControl),
                new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether scaling is enabled.
        /// </summary>
        public bool IsScalingEnabled
        {
            get
            {
                return (bool)GetValue(IsScalingEnabledProperty);
            }

            set
            {
                SetValue(IsScalingEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether rotating is enabled.
        /// </summary>
        public bool IsRotatingEnabled
        {
            get
            {
                return (bool)GetValue(IsRotatingEnabledProperty);
            }

            set
            {
                SetValue(IsRotatingEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the grid used for manipulation.
        /// </summary>
        private Grid ManipulationGrid { get; set; }
    }
}