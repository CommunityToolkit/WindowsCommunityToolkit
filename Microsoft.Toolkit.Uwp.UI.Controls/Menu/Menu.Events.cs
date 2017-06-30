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

using System.Diagnostics;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Menu Control defines a menu of choices for users to invoke.
    /// </summary>
    public partial class Menu
    {
        private const string CtrlValue = "CTRL";
        private const string ShiftValue = "SHIFT";
        private const string AltValue = "ALT";
        private Control _lastFocusElement;
        private bool _altHandled;
        private bool _isLostFocus = true;
        private Control _lastFocusElementBeforeMenu;
        private double _x1;
        private double _y1;
        private double _x2;
        private double _y2;

        private bool AllowTooltip => (bool)GetValue(AllowTooltipProperty);

        private static bool NavigateUsingKeyboard(object element, KeyEventArgs args, Menu menu, Orientation orientation)
        {
            if (!menu.IsOpened && element is MenuItem)
            {
                if (((args.VirtualKey == VirtualKey.Down || args.VirtualKey == VirtualKey.Enter) && orientation == Orientation.Horizontal) ||
                    ((args.VirtualKey == VirtualKey.Right || args.VirtualKey == VirtualKey.Enter) && orientation == Orientation.Vertical))
                {
                    menu.SelectedMenuItem.ShowMenu();
                    return true;
                }

                if ((args.VirtualKey == VirtualKey.Left && orientation == Orientation.Horizontal) ||
                    (args.VirtualKey == VirtualKey.Up && orientation == Orientation.Vertical))
                {
                    GetNextMenuItem(menu, -1);
                    return true;
                }

                if ((args.VirtualKey == VirtualKey.Right && orientation == Orientation.Horizontal) ||
                    (args.VirtualKey == VirtualKey.Down && orientation == Orientation.Vertical))
                {
                    GetNextMenuItem(menu, +1);
                    return true;
                }
            }

            if (args.VirtualKey == VirtualKey.Left)
            {
                if (element is MenuFlyoutItem)
                {
                    menu.IsInTransitionState = true;
                    menu.SelectedMenuItem.HideMenu();
                    GetNextMenuItem(menu, -1).ShowMenu();
                    return true;
                }

                if (element is MenuFlyoutSubItem)
                {
                    var menuFlyoutSubItem = (MenuFlyoutSubItem)element;
                    if (menuFlyoutSubItem.Parent is MenuItem && element == menu._lastFocusElement)
                    {
                        menu.IsInTransitionState = true;
                        menu.SelectedMenuItem.HideMenu();
                        GetNextMenuItem(menu, -1).ShowMenu();
                        return true;
                    }
                }
            }

            if (args.VirtualKey == VirtualKey.Right)
            {
                if (element is MenuFlyoutItem)
                {
                    menu.IsInTransitionState = true;
                    menu.SelectedMenuItem.HideMenu();
                    GetNextMenuItem(menu, +1).ShowMenu();
                    return true;
                }
            }

            return false;
        }

        private static MenuItem GetNextMenuItem(Menu menu, int moveCount)
        {
            var currentMenuItemIndex = menu.Items.IndexOf(menu.SelectedMenuItem);
            var nextIndex = (currentMenuItemIndex + moveCount + menu.Items.Count) % menu.Items.Count;
            var nextItem = menu.Items.ElementAt(nextIndex) as MenuItem;
            nextItem?.Focus(FocusState.Keyboard);
            return nextItem;
        }

        private static string MapInputToGestureKey(VirtualKey key)
        {
            var isCtrlDown = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
            var isShiftDown = Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down);
            var isAltDown = Window.Current.CoreWindow.GetKeyState(VirtualKey.Menu).HasFlag(CoreVirtualKeyStates.Down);

            if (!isCtrlDown && !isShiftDown && !isAltDown)
            {
                return null;
            }

            StringBuilder gestureKeyBuilder = new StringBuilder();

            if (isCtrlDown)
            {
                gestureKeyBuilder.Append(CtrlValue);
                gestureKeyBuilder.Append("+");
            }

            if (isShiftDown)
            {
                gestureKeyBuilder.Append(ShiftValue);
                gestureKeyBuilder.Append("+");
            }

            if (isAltDown)
            {
                gestureKeyBuilder.Append(AltValue);
                gestureKeyBuilder.Append("+");
            }

            if (key == VirtualKey.None)
            {
                gestureKeyBuilder.Remove(gestureKeyBuilder.Length - 1, 1);
            }
            else
            {
                gestureKeyBuilder.Append(key);
            }

            return gestureKeyBuilder.ToString();
        }

        private static void OrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menu = (Menu)d;
            if (menu._wrapPanel != null)
            {
                menu._wrapPanel.Orientation = menu.Orientation;
                foreach (MenuItem menuItem in menu.Items)
                {
                    if (menuItem.FlyoutButton?.Flyout != null)
                    {
                        menuItem.FlyoutButton.Flyout.Placement = menu.Orientation == Orientation.Horizontal
                            ? FlyoutPlacementMode.Bottom
                        : FlyoutPlacementMode.Right;
                    }
                }
            }
        }

        private static void RemoveElementFromCache(FrameworkElement descendant)
        {
            var value = descendant.GetValue(InputGestureTextProperty);
            if (value == null)
            {
                return;
            }

            var inputGestureText = value.ToString().ToUpper();
            if (!MenuItemInputGestureCache.ContainsKey(inputGestureText))
            {
                return;
            }

            var cachedMenuItem = MenuItemInputGestureCache[inputGestureText];
            if (cachedMenuItem == descendant)
            {
                MenuItemInputGestureCache.Remove(inputGestureText);
            }
        }

        private static bool WithinRange(double x1, double x2, double y1, double y2, double cursorX, double cursorY)
        {
            if (x1 < cursorX && cursorX < x2 &&
                y1 < cursorY && cursorY < y2)
            {
                return true;
            }

            return false;
        }

        private void Menu_Loaded(object sender, RoutedEventArgs e)
        {
            _wrapPanel = ItemsPanelRoot as WrapPanel.WrapPanel;
            if (_wrapPanel != null)
            {
                _wrapPanel.Orientation = Orientation;
            }

            Window.Current.CoreWindow.PointerMoved -= CoreWindow_PointerMoved;
            LostFocus -= Menu_LostFocus;
            LostFocus += Menu_LostFocus;
            Dispatcher.AcceleratorKeyActivated -= Dispatcher_AcceleratorKeyActivated;
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
            Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.PointerMoved += CoreWindow_PointerMoved;
        }

        private void Menu_LayoutUpdated(object sender, object e)
        {
            var ttv = TransformToVisual(Window.Current.Content);
            Point screenCoords = ttv.TransformPoint(new Point(0, 0));
            _x1 = screenCoords.X;
            _y1 = screenCoords.Y;
            _x2 = _x1 + ActualWidth;
            _y2 = _y1 + ActualHeight;
        }

        private void CoreWindow_PointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            // if contained with the whole Menu control
            if (IsOpened && WithinRange(_x1, _x2, _y1, _y2, args.CurrentPoint.Position.X, args.CurrentPoint.Position.Y))
            {
                // if hover over current opened item
                if (WithinRange(
                    SelectedMenuItem.X1,
                    SelectedMenuItem.X2,
                    SelectedMenuItem.Y1,
                    SelectedMenuItem.Y2,
                    args.CurrentPoint.Position.X,
                    args.CurrentPoint.Position.Y))
                {
                    return;
                }

                // TODO to be replaced with Range tree or any faster datastructure
                    foreach (MenuItem menuItem in Items)
                {
                    if (WithinRange(
                        menuItem.X1,
                        menuItem.X2,
                        menuItem.Y1,
                        menuItem.Y2,
                        args.CurrentPoint.Position.X,
                        args.CurrentPoint.Position.Y))
                    {
                        if (menuItem == SelectedMenuItem)
                        {
                            continue;
                        }

                        SelectedMenuItem.HideMenu();
                        menuItem.Focus(FocusState.Keyboard);
                        menuItem.ShowMenu();
                    }
                }
            }
        }

        private void Menu_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerMoved -= CoreWindow_PointerMoved;
            Dispatcher.AcceleratorKeyActivated -= Dispatcher_AcceleratorKeyActivated;
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;

            // Clear Cache
            foreach (MenuItem menuItem in Items)
            {
                var menuFlyoutItems = menuItem.GetMenuFlyoutItems();
                foreach (var flyoutItem in menuFlyoutItems)
                {
                    RemoveElementFromCache(flyoutItem);
                }

                RemoveElementFromCache(menuItem);
            }
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (IsInTransitionState)
            {
                return;
            }

            var element = FocusManager.GetFocusedElement();

            if (NavigateUsingKeyboard(element, args, this, Orientation))
            {
                return;
            }

            string gestureKey = MapInputToGestureKey(args.VirtualKey);

            if (gestureKey == null)
            {
                return;
            }

            if (MenuItemInputGestureCache.ContainsKey(gestureKey))
            {
                var cachedMenuItem = MenuItemInputGestureCache[gestureKey];
                if (cachedMenuItem is MenuFlyoutItem)
                {
                    var menuFlyoutItem = (MenuFlyoutItem)cachedMenuItem;
                    menuFlyoutItem.Command?.Execute(menuFlyoutItem.CommandParameter);
                }
            }
        }

        private void Menu_LostFocus(object sender, RoutedEventArgs e)
        {
            _isLostFocus = true;

            if (AllowTooltip)
            {
                HideSubItemTooltips();
            }
        }

        private void Dispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            _lastFocusElement = FocusManager.GetFocusedElement() as Control;
            if (args.VirtualKey == VirtualKey.Menu && !args.KeyStatus.WasKeyDown)
            {
                _altHandled = false;
            }
            else if (args.KeyStatus.IsMenuKeyDown && args.KeyStatus.IsKeyReleased && !_altHandled)
            {
                _altHandled = true;
                string gestureKey = MapInputToGestureKey(args.VirtualKey);

                if (gestureKey == null)
                {
                    return;
                }

                if (MenuItemInputGestureCache.ContainsKey(gestureKey))
                {
                    var cachedMenuItem = MenuItemInputGestureCache[gestureKey];
                    if (cachedMenuItem is MenuItem)
                    {
                        var menuItem = (MenuItem)cachedMenuItem;
                        menuItem.ShowMenu();
                        menuItem.Focus(FocusState.Keyboard);
                    }
                }
            }
            else if (args.VirtualKey == VirtualKey.Menu && args.KeyStatus.IsKeyReleased && !_altHandled)
            {
                _altHandled = true;
                if (!IsOpened)
                {
                    if (_isLostFocus)
                    {
                        LostFocus -= Menu_LostFocus;
                        Focus(FocusState.Programmatic);
                        _lastFocusElementBeforeMenu = _lastFocusElement;
                        _isLostFocus = false;

                        if (AllowTooltip)
                        {
                            ShowSubItemToolTips();
                        }

                        LostFocus += Menu_LostFocus;
                    }
                    else
                    {
                        _lastFocusElementBeforeMenu?.Focus(FocusState.Keyboard);
                    }
                }
            }
        }

        private void ShowSubItemToolTips()
        {
            foreach (var item in Items)
            {
                var i = item as MenuItem;
                i?.ShowTooltip();
            }
        }

        private void HideSubItemTooltips()
        {
            foreach (var item in Items)
            {
                var i = item as MenuItem;
                i?.HideTooltip();
            }
        }
    }
}
