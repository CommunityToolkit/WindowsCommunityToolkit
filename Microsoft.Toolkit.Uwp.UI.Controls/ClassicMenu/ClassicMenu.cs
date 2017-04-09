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

using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Classic Menu Control defines a menu of choices for users to invoke.
    /// </summary>
    public partial class ClassicMenu : ItemsControl
    {
        private const string CtrlValue = "CTRL";
        private const string ShiftValue = "SHIFT";

        // even if we have multiple menus in the same page we need only one cache because only one menu item will have certain short cut.
        private static readonly Dictionary<string, MenuFlyoutItem> MenuItemInputGestureCache = new Dictionary<string, MenuFlyoutItem>();

        /// <summary>
        /// Gets or sets the orientation of the Classic menu, Horizontal or vertical means that child controls will be added horizontally until the width of the panel can't fit more control then a new row is added to fit new horizontal added child controls, vertical means that child will be added vertically until the height of the panel is recieved then a new column is added
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                nameof(Orientation),
                typeof(Orientation),
                typeof(ClassicMenu),
                new PropertyMetadata(Orientation.Horizontal));

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassicMenu"/> class.
        /// </summary>
        public ClassicMenu()
        {
            DefaultStyleKey = typeof(ClassicMenu);
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            Loaded -= ClassicMenu_Loaded;
            Unloaded -= ClassicMenu_Unloaded;

            Loaded += ClassicMenu_Loaded;
            Unloaded += ClassicMenu_Unloaded;

            base.OnApplyTemplate();
        }
    }
}
