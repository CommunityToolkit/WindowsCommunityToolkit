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
    /// Menu Control defines a menu of choices for users to invoke.
    /// </summary>
    public partial class Menu : ItemsControl
    {
        private WrapPanel.WrapPanel _wrapPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        public Menu()
        {
            DefaultStyleKey = typeof(Menu);
        }

        // even if we have multiple menus in the same page we need only one cache because only one menu item will have certain short cut.
        private static readonly Dictionary<string, DependencyObject> MenuItemInputGestureCache = new Dictionary<string, DependencyObject>();

        /// <summary>
        /// Gets or sets the orientation of the Menu, Horizontal or vertical means that child controls will be added horizontally
        /// until the width of the panel can't fit more control then a new row is added to fit new horizontal added child controls, 
        /// vertical means that child will be added vertically until the height of the panel is received then a new column is added
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
                typeof(Menu),
                new PropertyMetadata(Orientation.Horizontal, OrientationPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="MenuFlyoutStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MenuFlyoutStyleProperty = DependencyProperty.Register(nameof(MenuFlyoutStyle), typeof(Style), typeof(MenuItem), new PropertyMetadata(default(Style)));

        /// <summary>
        /// Gets or sets the menu style for MenuItem
        /// </summary>
        public Style MenuFlyoutStyle
        {
            get { return (Style)GetValue(MenuFlyoutStyleProperty); }
            set { SetValue(MenuFlyoutStyleProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="AllowTooltip"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AllowTooltipProperty = DependencyProperty.Register(nameof(AllowTooltip), typeof(bool), typeof(Menu), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether to allow tooltip on alt or not
        /// </summary>
        public bool AllowTooltip
        {
            get { return (bool)GetValue(AllowTooltipProperty); }
            set { SetValue(AllowTooltipProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="HeaderButtonStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderButtonStyleProperty = DependencyProperty.Register(nameof(HeaderButtonStyle), typeof(Style), typeof(MenuItem), new PropertyMetadata(default(Style)));

        /// <summary>
        /// Gets or sets the style for HeaderButton
        /// </summary>
        public Style HeaderButtonStyle
        {
            get { return (Style)GetValue(HeaderButtonStyleProperty); }
            set { SetValue(HeaderButtonStyleProperty, value); }
        }

        /// <summary>
        /// Gets the current selected menu header item
        /// </summary>
        public MenuItem SelectedHeaderItem { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the menu is opened or not
        /// </summary>
        public bool IsOpened { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether the menu is in transition state between menu closing state and menu opened state.
        /// </summary>
        internal bool IsInTransitionState { get; set; }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            Loaded += ClassicMenu_Loaded;
            Unloaded += ClassicMenu_Unloaded;

            base.OnApplyTemplate();
        }

        /// <inheritdoc />
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is MenuItem;
        }

        /// <inheritdoc />
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MenuItem();
        }
    }
}
