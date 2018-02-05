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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The HamburgerMenuItem provides an abstract implementation for HamburgerMenu entries.
    /// </summary>
    public abstract class HamburgerMenuItem : DependencyObject
    {
        /// <summary>
        /// Identifies the <see cref="Label"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(HamburgerMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="TargetPageType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TargetPageTypeProperty = DependencyProperty.Register(nameof(TargetPageType), typeof(Type), typeof(HamburgerMenuItem), new PropertyMetadata(null));

        /// <summary>
         /// Identifies the <see cref="Tag"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TagProperty = DependencyProperty.Register(nameof(Tag), typeof(object), typeof(HamburgerMenuItem), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value that specifies label to display.
        /// </summary>
        public string Label
        {
            get
            {
                return (string)GetValue(LabelProperty);
            }

            set
            {
                SetValue(LabelProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that specifies the page to navigate to (if you use the HamburgerMenu with a Frame content)
        /// </summary>
        public Type TargetPageType
        {
            get
            {
                return (Type)GetValue(TargetPageTypeProperty);
            }

            set
            {
                SetValue(TargetPageTypeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that specifies an user specific value.
        /// </summary>
        public object Tag
        {
            get
            {
                return GetValue(TagProperty);
            }

            set
            {
                SetValue(TagProperty, value);
            }
        }
    }
}
