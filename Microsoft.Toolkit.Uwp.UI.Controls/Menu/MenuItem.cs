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
using Windows.Foundation.Collections;
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
            _parentMenu = this.FindAscendant<Menu>();
            IsOpened = false;

            if (_menuFlyout != null)
            {
                _menuFlyout.Opened -= MenuFlyout_Opened;
                _menuFlyout.Closed -= MenuFlyout_Closed;
            }

            if (Items != null)
            {
                Items.VectorChanged -= Items_VectorChanged;
                Items.VectorChanged += Items_VectorChanged;
            }

            if (FlyoutButton == null)
            {
                return;
            }

            FlyoutButton.PointerExited -= FlyoutButton_PointerExited;

            _menuFlyout = new MenuFlyout
            {
                Placement = _parentMenu.Orientation == Orientation.Horizontal
                    ? FlyoutPlacementMode.Bottom
                    : FlyoutPlacementMode.Right
            };

            _menuFlyout.Opened += MenuFlyout_Opened;
            _menuFlyout.Closed += MenuFlyout_Closed;
            FlyoutButton.PointerExited += FlyoutButton_PointerExited;

            _menuFlyout.MenuFlyoutPresenterStyle = _parentMenu.MenuFlyoutStyle;
            FlyoutButton.Style = _parentMenu.HeaderButtonStyle;
            ReAddItemsToFlyout();

            FlyoutButton.Flyout = _menuFlyout;

            base.OnApplyTemplate();
        }

        private void ReAddItemsToFlyout()
        {
            if (_menuFlyout == null || Items == null)
            {
                return;
            }

            _menuFlyout.Items.Clear();
            for (var index = 0; index < Items.Count; index++)
            {
                var item = Items[index];
                AddItemToFlyout(item, index);
            }
        }

        private void AddItemToFlyout(object item, int index)
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
                    AddItemToFlyout(sender.ElementAt(index), index);
                    break;
                case CollectionChange.ItemRemoved:
                    _menuFlyout.Items.RemoveAt(index);
                    break;
                case CollectionChange.ItemChanged:
                    _menuFlyout.Items.RemoveAt(index);
                    AddItemToFlyout(sender.ElementAt(index), index);
                    break;
            }
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
            _parentMenu.IsInTransitionState = false;
        }

        /// <inheritdoc />
        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            _parentMenu.SelectedHeaderItem = this;
            base.OnTapped(e);
        }

        /// <inheritdoc />
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            _parentMenu.SelectedHeaderItem = this;
            base.OnGotFocus(e);
        }
    }
}
