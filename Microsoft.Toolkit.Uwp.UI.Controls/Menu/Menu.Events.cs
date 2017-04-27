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
using System.Text;
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
        private static object _lastFocusElement;
        private bool _altHandled;

        private static bool NavigateUsingKeyboard(object element, KeyEventArgs args, Menu menu, Orientation orientation)
        {
            if (element is MenuItem)
            {
                if ((args.VirtualKey == VirtualKey.Down && orientation == Orientation.Horizontal) ||
                    (args.VirtualKey == VirtualKey.Right && orientation == Orientation.Vertical))
                {
                    menu.SelectedHeaderItem.ShowMenu();
                    return true;
                }

                if ((args.VirtualKey == VirtualKey.Left && orientation == Orientation.Horizontal) ||
                    (args.VirtualKey == VirtualKey.Up && orientation == Orientation.Vertical))
                {
                    MoveFocusBackwardAndGetPrevious(menu);
                    return true;
                }

                if ((args.VirtualKey == VirtualKey.Right && orientation == Orientation.Horizontal) ||
                    (args.VirtualKey == VirtualKey.Down && orientation == Orientation.Vertical))
                {
                    MoveFocusForwardAndGetNext(menu);
                    return true;
                }
            }

            if (args.VirtualKey == VirtualKey.Left)
            {
                if (element is MenuFlyoutItem)
                {
                    menu.SelectedHeaderItem.HideMenu();
                    MoveFocusBackwardAndGetPrevious(menu).ShowMenu();
                    return true;
                }

                if (element is MenuFlyoutSubItem)
                {
                    var menuFlyoutSubItem = (MenuFlyoutSubItem)element;
                    if (menuFlyoutSubItem.Parent is MenuItem && element == _lastFocusElement)
                    {
                        menu.SelectedHeaderItem.HideMenu();
                        MoveFocusBackwardAndGetPrevious(menu).ShowMenu();
                        return true;
                    }
                }
            }

            if (args.VirtualKey == VirtualKey.Right)
            {
                if (element is MenuFlyoutItem)
                {
                    menu.SelectedHeaderItem.HideMenu();
                    MoveFocusForwardAndGetNext(menu).ShowMenu();
                    return true;
                }
            }

            return false;
        }

        // this function enables the menu to cycle from start to end and from end to start
        private static MenuItem MoveFocusForwardAndGetNext(Menu menu)
        {
            var currentMenuItemIndex = menu.Items.IndexOf(menu.SelectedHeaderItem);
            var nextIndex = (currentMenuItemIndex + 1) % menu.Items.Count;
            var nextItem = menu.Items.ElementAt(nextIndex) as MenuItem;
            nextItem?.Focus(FocusState.Keyboard);
            return nextItem;
        }

        // this function enables the menu to cycle from end to start and from start to end
        private static MenuItem MoveFocusBackwardAndGetPrevious(Menu menu)
        {
            var currentMenuItemIndex = menu.Items.IndexOf(menu.SelectedHeaderItem);
            var nextIndex = (currentMenuItemIndex - 1 + menu.Items.Count) % menu.Items.Count;
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

        private void ClassicMenu_Loaded(object sender, RoutedEventArgs e)
        {
            _wrapPanel = ItemsPanelRoot as WrapPanel.WrapPanel;
            if (_wrapPanel != null)
            {
                _wrapPanel.Orientation = Orientation;
            }

            Dispatcher.AcceleratorKeyActivated -= Dispatcher_AcceleratorKeyActivated;
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
            Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
        }

        private void ClassicMenu_Unloaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.AcceleratorKeyActivated -= Dispatcher_AcceleratorKeyActivated;
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
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

        private void Dispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            _lastFocusElement = FocusManager.GetFocusedElement();
            if (args.VirtualKey == VirtualKey.Menu && !args.KeyStatus.WasKeyDown)
            {
                _altHandled = false;
            }
            else if (args.VirtualKey == VirtualKey.Menu && args.KeyStatus.IsKeyReleased && !_altHandled)
            {
                Focus(FocusState.Keyboard);
            }
            else if (args.KeyStatus.IsMenuKeyDown && args.KeyStatus.IsKeyReleased)
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
                        SelectedHeaderItem = menuItem;
                        menuItem.ShowMenu();
                    }
                }
            }
        }
    }
}
