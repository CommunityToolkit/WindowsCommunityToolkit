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
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Menu Item is the items main container for Class Menu control
    /// </summary>
    public class MenuItem : ItemsControl
    {
        /// <summary>
        /// MenuItem header text
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(string), typeof(MenuItem), new PropertyMetadata(default(string)));

        private Menu ParentMenu => this.FindAscendant<Menu>();

        internal Button FlyoutButton { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the menu is opened or not
        /// </summary>
        public bool IsOpened { get; private set; }

        /// <summary>
        /// Gets or sets the title to appear in the title bar
        /// </summary>
        public string Header
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
            FlyoutButton?.Flyout?.ShowAt(FlyoutButton);
        }

        /// <summary>
        /// This method is used to hide the menu for current item
        /// </summary>
        public void HideMenu()
        {
            FlyoutButton?.Flyout?.Hide();
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            FlyoutButton = GetTemplateChild("FlyoutButton") as Button;
            IsOpened = false;

            if (FlyoutButton != null && Items != null && Items.Any())
            {
                var menuFlyout = new MenuFlyout();
                menuFlyout.Placement = ParentMenu.Orientation == Orientation.Horizontal
                    ? FlyoutPlacementMode.Bottom
                        : FlyoutPlacementMode.Right;

                menuFlyout.Opened -= MenuFlyout_Opened;
                menuFlyout.Closed -= MenuFlyout_Closed;
                FlyoutButton.PointerExited -= FlyoutButton_PointerExited;
                menuFlyout.Opened += MenuFlyout_Opened;
                menuFlyout.Closed += MenuFlyout_Closed;
                FlyoutButton.PointerExited += FlyoutButton_PointerExited;

                menuFlyout.MenuFlyoutPresenterStyle = ParentMenu.MenuFlyoutStyle;
                FlyoutButton.Style = ParentMenu.HeaderButtonStyle;

                foreach (var item in Items)
                {
                    var menuItem = item as MenuFlyoutItemBase;
                    if (menuItem != null)
                    {
                        menuFlyout.Items.Add(menuItem);
                    }
                    else
                    {
                        var newMenuItem = new MenuFlyoutItem();
                        newMenuItem.DataContext = item;
                        menuFlyout.Items.Add(newMenuItem);
                    }
                }

                FlyoutButton.Flyout = menuFlyout;
            }

            base.OnApplyTemplate();
        }

        private void FlyoutButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (IsOpened)
            {
                VisualStateManager.GoToState(FlyoutButton, "Opened", true);
            }
        }

        private void MenuFlyout_Closed(object sender, object e)
        {
            IsOpened = false;
            VisualStateManager.GoToState(FlyoutButton, "Normal", true);
        }

        private void MenuFlyout_Opened(object sender, object e)
        {
            IsOpened = true;
            VisualStateManager.GoToState(FlyoutButton, "Opened", true);
        }

        /// <inheritdoc />
        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            ParentMenu.SelectedHeaderItem = this;
            base.OnTapped(e);
        }

        /// <inheritdoc />
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            ParentMenu.SelectedHeaderItem = this;
            base.OnGotFocus(e);
        }
    }
}
