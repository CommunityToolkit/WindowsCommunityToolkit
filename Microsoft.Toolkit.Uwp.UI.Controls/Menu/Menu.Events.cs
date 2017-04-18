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

        private static bool NavigateThroughMenuHeader(KeyEventArgs args, Menu menu, MenuItem menuItem, Orientation orientation)
        {
            if (orientation == Orientation.Horizontal)
            {
                if (args.VirtualKey == VirtualKey.Down)
                {
                    menuItem.ShowMenu();
                    return true;
                }

                if (args.VirtualKey == VirtualKey.Left)
                {
                    return MoveFocusBackward(menu, menuItem);
                }

                if (args.VirtualKey == VirtualKey.Right)
                {
                    return MoveFocusForward(menu, menuItem);
                }
            }
            else
            {
                if (args.VirtualKey == VirtualKey.Right)
                {
                    menuItem.ShowMenu();
                    return true;
                }

                if (args.VirtualKey == VirtualKey.Up)
                {
                    return MoveFocusBackward(menu, menuItem);
                }

                if (args.VirtualKey == VirtualKey.Down)
                {
                    return MoveFocusForward(menu, menuItem);
                }
            }

            return false;
        }

        // this function enables the menu to cycle from start to end and from end to start
        private static bool MoveFocusForward(Menu menu, MenuItem menuItem)
        {
            var currentMenuItemIndex = menu.Items.IndexOf(menuItem);
            var nextIndex = (currentMenuItemIndex + 1) % menu.Items.Count;
            var nextItem = menu.Items.ElementAt(nextIndex) as MenuItem;
            nextItem?.Focus(FocusState.Keyboard);
            return true;
        }

        // this function enables the menu to cycle from end to start and from start to end
        private static bool MoveFocusBackward(Menu menu, MenuItem menuItem)
        {
            var currentMenuItemIndex = menu.Items.IndexOf(menuItem);
            var nextIndex = (currentMenuItemIndex - 1 + menu.Items.Count) % menu.Items.Count;
            var nextItem = menu.Items.ElementAt(nextIndex) as MenuItem;
            nextItem?.Focus(FocusState.Keyboard);
            return true;
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
            var menuItem = element as MenuItem;
            if (menuItem != null)
            {
                if (NavigateThroughMenuHeader(args, this, menuItem, Orientation))
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
                cachedMenuItem.Command?.Execute(cachedMenuItem.CommandParameter);
            }
        }

        private void Dispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Menu && !args.KeyStatus.WasKeyDown)
            {
                Focus(FocusState.Keyboard);
            }
        }
    }
}
