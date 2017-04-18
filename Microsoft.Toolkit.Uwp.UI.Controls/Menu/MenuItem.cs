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

using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Menu Item is the items main container for Class Menu control
    /// </summary>
    public class MenuItem : ItemsControl
    {
        /// <summary>
        /// ClassicMenuItem header text
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(string), typeof(MenuItem), new PropertyMetadata(default(string)));

        /// <summary>
        /// ClassicMenuItem Meny Style
        /// </summary>
        public static readonly DependencyProperty MenuStyleProperty = DependencyProperty.Register(nameof(MenuStyle), typeof(Style), typeof(MenuItem), new PropertyMetadata(default(Style)));

        /// <summary>
        /// ClassicMenuItem Menu background
        /// </summary>
        public static readonly DependencyProperty MenuBackgroundProperty =
            DependencyProperty.Register(
            nameof(MenuBackground),
            typeof(Brush),
            typeof(MenuItem),
            new PropertyMetadata(default(Brush)));

        private Menu ParentMenu => this.FindAscendant<Menu>();

        private Button _flyoutButton;

        /// <summary>
        /// Gets or sets the title to appear in the title bar
        /// </summary>
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the menu style for ClassicMenuItem
        /// </summary>
        public Style MenuStyle
        {
            get { return (Style)GetValue(MenuStyleProperty); }
            set { SetValue(MenuStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the MenuBackground to appear in the title bar
        /// </summary>
        public string MenuBackground
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class.
        /// </summary>
        public MenuItem()
        {
            DefaultStyleKey = typeof(MenuItem);
            IsFocusEngagementEnabled = true;
        }

        /// <summary>
        /// This method is used to show the menu for current item
        /// </summary>
        public void ShowMenu()
        {
            _flyoutButton?.Flyout?.ShowAt(_flyoutButton);
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            _flyoutButton = GetTemplateChild("FlyoutButton") as Button;

            if (_flyoutButton != null && Items != null && Items.Any())
            {
                var menuFlyout = new MenuFlyout();
                menuFlyout.Placement = ParentMenu.Orientation == Orientation.Horizontal
                    ? FlyoutPlacementMode.Bottom
                        : FlyoutPlacementMode.Right;
                menuFlyout.MenuFlyoutPresenterStyle = MenuStyle;

                foreach (var item in Items)
                {
                    var menuItem = item as MenuFlyoutItemBase;
                    if (menuItem != null)
                    {
                        menuFlyout.Items.Add(menuItem);
                    }
                }

                _flyoutButton.Flyout = menuFlyout;
            }

            base.OnApplyTemplate();
        }
    }
}
