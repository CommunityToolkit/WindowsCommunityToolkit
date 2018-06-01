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
    /// <see cref="StyleSelector"/> to be used with <see cref="NavigationView"/>
    /// HamburgerMenuNavViewItemStyleSelector is used by the <see cref="HamburgerMenu"/>
    /// </summary>
    [Obsolete("The HamburgerMenuNavViewItemStyleSelector will be removed alongside the HamburgerMenu in a future major release. Please use the NavigationView control available in the Fall Creators Update")]
    public class HamburgerMenuNavViewItemStyleSelector : StyleSelector
    {
        /// <summary>
        /// Gets or sets the <see cref="Style"/> to be set if the container is a <see cref="NavigationViewItem"/>
        /// </summary>
        public Style MenuItemStyle { get; set; }

        /// <inheritdoc/>
        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            if (container is NavigationViewItem)
            {
                return MenuItemStyle;
            }
            else
            {
                return null;
            }
        }
    }
}
