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
using System.Linq;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
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
        private const string FlyoutButtonName = "FlyoutButton";
        private readonly bool _isAccessKeySupported = ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3);
        private Menu _parentMenu;
        private bool _isOpened;
        private MenuFlyout _menuFlyout;

        internal Button FlyoutButton { get; private set; }

        /// <summary>
        /// Identifies the <see cref="Header"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(string), typeof(MenuItem), new PropertyMetadata(default(string)));

        /// <summary>
        /// Gets or sets the title to appear in the title bar
        /// </summary>
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the menu is opened or not
        /// </summary>
        public bool IsOpened
        {
            get
            {
                return _isOpened;
            }

            private set
            {
                _parentMenu.IsOpened = _isOpened = value;
            }
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
            _menuFlyout.ShowAt(FlyoutButton, new Windows.Foundation.Point(0, FlyoutButton.ActualHeight));
        }

        /// <summary>
        /// This method is used to hide the menu for current item
        /// </summary>
        public void HideMenu()
        {
            _menuFlyout?.Hide();
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            FlyoutButton = GetTemplateChild(FlyoutButtonName) as Button;
            _parentMenu = this.FindAscendant<Menu>();
            IsOpened = false;

            Items.VectorChanged -= Items_VectorChanged;

            if (_menuFlyout == null)
            {
                _menuFlyout = new MenuFlyout();
            }
            else
            {
                _menuFlyout.Opened -= MenuFlyout_Opened;
                _menuFlyout.Closed -= MenuFlyout_Closed;
            }

            if (FlyoutButton != null)
            {
                FlyoutButton.PointerExited -= FlyoutButton_PointerExited;
                Items.VectorChanged += Items_VectorChanged;

                _menuFlyout.Placement = _parentMenu.Orientation == Orientation.Horizontal
                    ? FlyoutPlacementMode.Bottom
                    : FlyoutPlacementMode.Right;

                FlyoutButton.Flyout = _menuFlyout;

                _menuFlyout.Opened += MenuFlyout_Opened;
                _menuFlyout.Closed += MenuFlyout_Closed;
                FlyoutButton.PointerExited += FlyoutButton_PointerExited;

                _menuFlyout.MenuFlyoutPresenterStyle = _parentMenu.MenuFlyoutStyle;
                ReAddItemsToFlyout();

                if (_isAccessKeySupported)
                {
                    FlyoutButton.AccessKey = AccessKey;
                    AccessKey = string.Empty;
                }
            }

            base.OnApplyTemplate();
        }

        internal IEnumerable<MenuFlyoutItemBase> GetMenuFlyoutItems()
        {
            var allItems = new List<MenuFlyoutItemBase>();
            var menuFlyout = FlyoutButton.Flyout as MenuFlyout;
            if (menuFlyout != null)
            {
                GetMenuFlyoutItemItems(menuFlyout.Items, allItems);
            }

            return allItems;
        }

        private void GetMenuFlyoutItemItems(IList<MenuFlyoutItemBase> menuFlyoutItems, List<MenuFlyoutItemBase> allItems)
        {
            foreach (var menuFlyoutItem in menuFlyoutItems)
            {
                allItems.Add(menuFlyoutItem);

                if (menuFlyoutItem is MenuFlyoutSubItem)
                {
                    var menuItem = (MenuFlyoutSubItem)menuFlyoutItem;
                    GetMenuFlyoutItemItems(menuItem.Items, allItems);
                }
            }
        }

        internal void ShowTooltip()
        {
            var inputGestureText = GetValue(Menu.InputGestureTextProperty) as string;
            if (string.IsNullOrEmpty(inputGestureText))
            {
                return;
            }

            var tooltip = ToolTipService.GetToolTip(FlyoutButton) as ToolTip;
            if (tooltip == null)
            {
                tooltip = new ToolTip();
                tooltip.Style = _parentMenu.TooltipStyle;
                ToolTipService.SetToolTip(FlyoutButton, tooltip);
            }

            tooltip.Placement = _parentMenu.TooltipPlacement;
            tooltip.Content = RemoveAlt(inputGestureText);
            tooltip.IsOpen = !tooltip.IsOpen;
        }

        private string RemoveAlt(string inputGesture)
        {
            if (string.IsNullOrEmpty(inputGesture))
            {
                return string.Empty;
            }

            return inputGesture.Replace("Alt+", string.Empty);
        }

        internal void HideTooltip()
        {
            var tooltip = ToolTipService.GetToolTip(FlyoutButton) as ToolTip;
            if (tooltip != null)
            {
                tooltip.IsOpen = false;
            }
        }

        private void ReAddItemsToFlyout()
        {
            if (_menuFlyout == null)
            {
                return;
            }

            _menuFlyout.Items.Clear();
            foreach (var item in Items)
            {
                AddItemToFlyout(item);
            }
        }

        private void AddItemToFlyout(object item)
        {
            var menuItem = item as MenuFlyoutItemBase;
            if (menuItem != null)
            {
                _menuFlyout.Items.Add(menuItem);
            }
            else
            {
                var newMenuItem = new MenuFlyoutItem();
                newMenuItem.DataContext = item;
                _menuFlyout.Items.Add(newMenuItem);
            }
        }

        private void InsertItemToFlyout(object item, int index)
        {
            var menuItem = item as MenuFlyoutItemBase;
            if (menuItem != null)
            {
                _menuFlyout.Items.Insert(index, menuItem);
            }
            else
            {
                var newMenuItem = new MenuFlyoutItem();
                newMenuItem.DataContext = item;
                _menuFlyout.Items.Insert(index, newMenuItem);
            }
        }

        private void Items_VectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs e)
        {
            var index = (int)e.Index;
            switch (e.CollectionChange)
            {
                case CollectionChange.Reset:
                    ReAddItemsToFlyout();
                    break;
                case CollectionChange.ItemInserted:
                    AddItemToFlyout(sender.ElementAt(index));
                    break;
                case CollectionChange.ItemRemoved:
                    _menuFlyout.Items.RemoveAt(index);
                    break;
                case CollectionChange.ItemChanged:
                    _menuFlyout.Items.RemoveAt(index);
                    InsertItemToFlyout(sender.ElementAt(index), index);
                    break;
            }
        }

        private void FlyoutButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (IsOpened)
            {
                VisualStateManager.GoToState(this, "Opened", true);
            }
        }

        private void MenuFlyout_Closed(object sender, object e)
        {
            IsOpened = false;
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void MenuFlyout_Opened(object sender, object e)
        {
            IsOpened = true;
            VisualStateManager.GoToState(this, "Opened", true);
            _parentMenu.IsInTransitionState = false;
        }

        /// <inheritdoc />
        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            _parentMenu.SelectedMenuItem = this;
            ShowMenu();
            base.OnTapped(e);
        }

        /// <inheritdoc />
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            _parentMenu.SelectedMenuItem = this;
            base.OnGotFocus(e);
        }
    }
}
