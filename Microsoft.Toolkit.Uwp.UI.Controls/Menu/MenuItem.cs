// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Menu Item is the items main container for Class Menu control
    /// </summary>
    public class MenuItem : HeaderedItemsControl
    {
        private const string FlyoutButtonName = "FlyoutButton";
        private const char UnderlineCharacter = '^';
        private readonly bool _isAccessKeySupported = ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3);
        private readonly bool _isTextTextDecorationsSupported = ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 4);
        private Menu _parentMenu;
        private bool _isOpened;
        private bool _menuFlyoutRepositioned;
        private bool _menuFlyoutPlacementChanged;
        private string _originalHeader;
        private bool _isInternalHeaderUpdate;

        internal MenuFlyout MenuFlyout { get; set; }

        internal Button FlyoutButton { get; private set; }

        private Rect _bounds;

        private object InternalHeader
        {
            set
            {
                _isInternalHeaderUpdate = true;
                Header = value;
                _isInternalHeaderUpdate = false;
            }
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

        internal bool ContainsPoint(Point point)
        {
            return _bounds.Contains(point);
        }

        /// <summary>
        /// This method is used to hide the menu for current item
        /// </summary>
        public void HideMenu()
        {
            MenuFlyout?.Hide();
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            FlyoutButton = GetTemplateChild(FlyoutButtonName) as Button;
            _parentMenu = this.FindParent<Menu>();
            IsOpened = false;

            Items.VectorChanged -= Items_VectorChanged;
            IsEnabledChanged -= MenuItem_IsEnabledChanged;

            if (MenuFlyout == null)
            {
                MenuFlyout = new MenuFlyout();
            }
            else
            {
                MenuFlyout.Opened -= MenuFlyout_Opened;

                MenuFlyout.Closed -= MenuFlyout_Closed;
            }

            if (FlyoutButton != null)
            {
                FlyoutButton.PointerExited -= FlyoutButton_PointerExited;
                Items.VectorChanged += Items_VectorChanged;

                MenuFlyout.Opened += MenuFlyout_Opened;
                MenuFlyout.Closed += MenuFlyout_Closed;
                FlyoutButton.PointerExited += FlyoutButton_PointerExited;

                MenuFlyout.MenuFlyoutPresenterStyle = _parentMenu.MenuFlyoutStyle;
                ReAddItemsToFlyout();

                IsEnabledChanged += MenuItem_IsEnabledChanged;

                if (_isAccessKeySupported)
                {
                    FlyoutButton.AccessKey = AccessKey;
                    AccessKey = string.Empty;
                }
            }

            UpdateEnabledVisualState();

            base.OnApplyTemplate();
        }

        private void MenuItem_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var menuItemControl = (MenuItem)sender;
            menuItemControl.UpdateEnabledVisualState();
        }

        internal void CalculateBounds()
        {
            var ttv = TransformToVisual(Window.Current.Content);
            Point screenCoords = ttv.TransformPoint(new Point(0, 0));
            _bounds.X = screenCoords.X;
            _bounds.Y = screenCoords.Y;
            _bounds.Width = ActualWidth;
            _bounds.Height = ActualHeight;
        }

        internal IEnumerable<MenuFlyoutItemBase> GetMenuFlyoutItems()
        {
            var allItems = new List<MenuFlyoutItemBase>();
            if (MenuFlyout != null)
            {
                GetMenuFlyoutItemItems(MenuFlyout.Items, allItems);
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
            tooltip.IsOpen = true;
            tooltip.IsEnabled = true;
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
                tooltip.IsEnabled = false;
            }
        }

        private void ReAddItemsToFlyout()
        {
            if (MenuFlyout == null)
            {
                return;
            }

            MenuFlyout.Items.Clear();
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
                MenuFlyout.Items.Add(menuItem);
            }
            else
            {
                var newMenuItem = new MenuFlyoutItem();
                newMenuItem.DataContext = item;
                MenuFlyout.Items.Add(newMenuItem);
            }
        }

        private void InsertItemToFlyout(object item, int index)
        {
            var menuItem = item as MenuFlyoutItemBase;
            if (menuItem != null)
            {
                MenuFlyout.Items.Insert(index, menuItem);
            }
            else
            {
                var newMenuItem = new MenuFlyoutItem();
                newMenuItem.DataContext = item;
                MenuFlyout.Items.Insert(index, newMenuItem);
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
                    InsertItemToFlyout(sender.ElementAt(index), index);
                    break;
                case CollectionChange.ItemRemoved:
                    MenuFlyout.Items.RemoveAt(index);
                    break;
                case CollectionChange.ItemChanged:
                    MenuFlyout.Items.RemoveAt(index);
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
            _menuFlyoutRepositioned = false;
            _menuFlyoutPlacementChanged = false;
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void MenuFlyout_Opened(object sender, object e)
        {
            if (_parentMenu.UpdateMenuItemsFlyoutPlacement() && !_menuFlyoutPlacementChanged)
            {
                _menuFlyoutPlacementChanged = true;
                ShowMenu();
            }

            _parentMenu.CalculateBounds();
            IsOpened = true;
            VisualStateManager.GoToState(this, "Opened", true);
            _parentMenu.IsInTransitionState = false;

            if (!_menuFlyoutRepositioned)
            {
                var popup = VisualTreeHelper.GetOpenPopups(Window.Current).FirstOrDefault(p => p.Child is MenuFlyoutPresenter);

                if (popup != null)
                {
                    var mfp = (MenuFlyoutPresenter)popup.Child;
                    var height = mfp.ActualHeight;
                    var width = mfp.ActualWidth;

                    var flytoutButtonPoint = FlyoutButton.TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0));

                    if ((width > Window.Current.Bounds.Width - flytoutButtonPoint.X &&
                        (MenuFlyout.Placement == FlyoutPlacementMode.Bottom)) ||
                        (height > Window.Current.Bounds.Height - flytoutButtonPoint.Y &&
                        (MenuFlyout.Placement == FlyoutPlacementMode.Right)))
                    {
                        ShowMenuRepositioned(width, height);
                    }
                }
            }
        }

        private void ShowMenuRepositioned(double menuWidth, double menuHeight)
        {
            if (!IsEnabled || MenuFlyout.Items.Count == 0)
            {
                return;
            }

            _menuFlyoutRepositioned = true;
            Point location;
            if (MenuFlyout.Placement == FlyoutPlacementMode.Bottom)
            {
                location = new Point(FlyoutButton.ActualWidth - menuWidth, FlyoutButton.ActualHeight);
            }
            else if (MenuFlyout.Placement == FlyoutPlacementMode.Right)
            {
                location = new Point(FlyoutButton.ActualWidth, FlyoutButton.ActualHeight - menuHeight);
            }
            else
            {
                // let the flyout decide where to show
                MenuFlyout.ShowAt(FlyoutButton);
                return;
            }

            MenuFlyout.ShowAt(FlyoutButton, location);
        }

        /// <summary>
        /// This method is used to show the menu for current item
        /// </summary>
        public void ShowMenu()
        {
            if (!IsEnabled || MenuFlyout.Items.Count == 0)
            {
                return;
            }

            Point location;
            if (MenuFlyout.Placement == FlyoutPlacementMode.Bottom)
            {
                location = new Point(0, FlyoutButton.ActualHeight);
            }
            else if (MenuFlyout.Placement == FlyoutPlacementMode.Right)
            {
                location = new Point(FlyoutButton.ActualWidth, 0);
            }
            else
            {
                // let the flyout decide where to show
                MenuFlyout.ShowAt(FlyoutButton);
                return;
            }

            MenuFlyout.ShowAt(FlyoutButton, location);
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

        internal void Underline()
        {
            if (_originalHeader == null)
            {
                return;
            }

            var underlineCharacterIndex = _originalHeader.IndexOf(UnderlineCharacter);

            var underlinedCharacter = _originalHeader[underlineCharacterIndex + 1];
            var text = new TextBlock();

            var runWithUnderlinedCharacter = new Run
            {
                Text = underlinedCharacter.ToString()
            };

            if (_isTextTextDecorationsSupported)
            {
                runWithUnderlinedCharacter.TextDecorations = Windows.UI.Text.TextDecorations.Underline;
            }

            var firstPartBuilder = new StringBuilder();
            var secondPartBuilder = new StringBuilder();

            for (int i = 0; i < underlineCharacterIndex; i++)
            {
                firstPartBuilder.Append(_originalHeader[i]);
            }

            for (int i = underlineCharacterIndex + 2; i < _originalHeader.Length; i++)
            {
                secondPartBuilder.Append(_originalHeader[i]);
            }

            var firstPart = firstPartBuilder.ToString();
            var secondPart = secondPartBuilder.ToString();

            if (!string.IsNullOrEmpty(firstPart))
            {
                text.Inlines.Add(new Run
                {
                    Text = firstPart
                });
            }

            text.Inlines.Add(runWithUnderlinedCharacter);

            if (!string.IsNullOrEmpty(secondPart))
            {
                text.Inlines.Add(new Run
                {
                    Text = secondPart
                });
            }

            InternalHeader = text;
        }

        /// <inheritdoc />
        protected override void OnHeaderChanged(object oldValue, object newValue)
        {
            base.OnHeaderChanged(oldValue, newValue);

            if (_isInternalHeaderUpdate)
            {
                return;
            }

            _originalHeader = null;

            var headerString = newValue as string;

            if (string.IsNullOrEmpty(headerString))
            {
                return;
            }

            var underlineCharacterIndex = headerString.IndexOf(UnderlineCharacter);

            if (underlineCharacterIndex == -1)
            {
                return;
            }

            if (underlineCharacterIndex == headerString.Length - 1)
            {
                InternalHeader = headerString.Replace(UnderlineCharacter.ToString(), string.Empty);
                return;
            }

            _originalHeader = headerString;
            InternalHeader = headerString.Replace(UnderlineCharacter.ToString(), string.Empty);
        }

        internal void RemoveUnderline()
        {
            if (_originalHeader != null)
            {
                InternalHeader = _originalHeader.Replace(UnderlineCharacter.ToString(), string.Empty);
            }
        }

        internal void UpdateEnabledVisualState()
        {
            if (IsEnabled)
            {
                VisualStateManager.GoToState(this, "Normal", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Disabled", true);
            }
        }
    }
}
