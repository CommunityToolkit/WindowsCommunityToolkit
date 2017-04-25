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

        private static bool NavigateThroughMenuHeader(KeyEventArgs args, Menu menu, Orientation orientation)
        {
            if (orientation == Orientation.Horizontal)
            {
                if (args.VirtualKey == VirtualKey.Down)
                {
                    menu.SelectedHeaderItem.ShowMenu();
                    return true;
                }

                if (args.VirtualKey == VirtualKey.Left)
                {
                    MoveFocusBackwardAndGetPrevious(menu);
                    return true;
                }

                if (args.VirtualKey == VirtualKey.Right)
                {
                    MoveFocusForwardAndGetNext(menu);
                    return true;
                }
            }
            else
            {
                if (args.VirtualKey == VirtualKey.Right)
                {
                    menu.SelectedHeaderItem.ShowMenu();
                    return true;
                }

                if (args.VirtualKey == VirtualKey.Up)
                {
                    MoveFocusBackwardAndGetPrevious(menu);
                    return true;
                }

                if (args.VirtualKey == VirtualKey.Down)
                {
                    MoveFocusForwardAndGetNext(menu);
                    return true;
                }
            }

            return false;
        }

        private static bool NavigateThroughMenuFlyoutItems(KeyEventArgs args, Menu menu, Orientation orientation)
        {
            if (orientation == Orientation.Horizontal)
            {
                if (args.VirtualKey == VirtualKey.Left)
                {
                    menu.SelectedHeaderItem.HideMenu();
                    MoveFocusBackwardAndGetPrevious(menu).ShowMenu();
                    return true;
                }

                if (args.VirtualKey == VirtualKey.Right)
                {
                    menu.SelectedHeaderItem.HideMenu();
                    MoveFocusForwardAndGetNext(menu).ShowMenu();
                    return true;
                }
            }
            else
            {
                if (args.VirtualKey == VirtualKey.Up)
                {
                    menu.SelectedHeaderItem.HideMenu();
                    MoveFocusBackwardAndGetPrevious(menu).ShowMenu();
                    return true;
                }

                if (args.VirtualKey == VirtualKey.Down)
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

        private static string MapInputToGestureKey(KeyEventArgs args)
        {
            var isCtrlDown = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
            var isShiftDown = Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down);

            if (!isCtrlDown && !isShiftDown)
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

            if (args.VirtualKey == VirtualKey.None)
            {
                gestureKeyBuilder.Remove(gestureKeyBuilder.Length - 1, 1);
            }
            else
            {
                gestureKeyBuilder.Append(args.VirtualKey);
            }

            return gestureKeyBuilder.ToString();
        }

        private void ClassicMenu_Loaded(object sender, RoutedEventArgs e)
        {
            var wrapPanel = ItemsPanelRoot as WrapPanel.WrapPanel;
            if (wrapPanel != null)
            {
                wrapPanel.Orientation = Orientation;
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
            if (element is MenuItem)
            {
                if (NavigateThroughMenuHeader(args, this, Orientation))
                {
                    return;
                }
            }

            if (element is MenuFlyoutItem)
            {
                if (NavigateThroughMenuFlyoutItems(args, this, Orientation))
                {
                    return;
                }
            }

            string gestureKey = MapInputToGestureKey(args);

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

                if (cachedMenuItem is MenuItem)
                {
                    var menuItem = (MenuItem)cachedMenuItem;
                    SelectedHeaderItem = menuItem;
                    menuItem.ShowMenu();
                }
            }
        }

        private void Dispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            var isAltDown = Window.Current.CoreWindow.GetKeyState(VirtualKey.Menu).HasFlag(CoreVirtualKeyStates.Down);
            if (args.VirtualKey == VirtualKey.Menu && !isAltDown)
            {
                Focus(FocusState.Keyboard);
            }
        }
    }
}
