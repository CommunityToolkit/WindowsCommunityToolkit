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
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

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
        private FlyoutPlacementMode? _currentFlyoutPlacement;

        private static bool NavigateUsingKeyboard(object element, KeyEventArgs args, Menu menu, Orientation orientation)
        {
            if (!menu.IsOpened && element is MenuItem)
            {
                menu.UpdateMenuItemsFlyoutPlacement();

                if (args.VirtualKey == VirtualKey.Enter ||
                    ((args.VirtualKey == VirtualKey.Down) && menu._currentFlyoutPlacement == FlyoutPlacementMode.Bottom) ||
                    ((args.VirtualKey == VirtualKey.Up) && menu._currentFlyoutPlacement == FlyoutPlacementMode.Top) ||
                    ((args.VirtualKey == VirtualKey.Left) && menu._currentFlyoutPlacement == FlyoutPlacementMode.Left) ||
                    ((args.VirtualKey == VirtualKey.Right) && menu._currentFlyoutPlacement == FlyoutPlacementMode.Right))
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

            if ((menu._currentFlyoutPlacement == FlyoutPlacementMode.Left &&
                 args.VirtualKey == VirtualKey.Right) ||
                 (args.VirtualKey == VirtualKey.Left &&
                 menu._currentFlyoutPlacement != FlyoutPlacementMode.Left))
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

            if ((args.VirtualKey == VirtualKey.Right &&
                menu._currentFlyoutPlacement != FlyoutPlacementMode.Left) ||
                (args.VirtualKey == VirtualKey.Left &&
                menu._currentFlyoutPlacement == FlyoutPlacementMode.Left))
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

        private static string MapInputToGestureKey(VirtualKey key, bool menuHasFocus = false)
        {
            var isCtrlDown = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
            var isShiftDown = Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down);
            var isAltDown = Window.Current.CoreWindow.GetKeyState(VirtualKey.Menu).HasFlag(CoreVirtualKeyStates.Down) || menuHasFocus;

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

        internal bool UpdateMenuItemsFlyoutPlacement()
        {
            var placementMode = GetMenuFlyoutPlacementMode();

            if (placementMode == _currentFlyoutPlacement)
            {
                return false;
            }

            _currentFlyoutPlacement = placementMode;

            foreach (MenuItem menuItem in Items)
            {
                if (menuItem.MenuFlyout != null)
                {
                    menuItem.MenuFlyout.Placement = _currentFlyoutPlacement.Value;
                }
            }

            return true;
        }

        internal FlyoutPlacementMode GetMenuFlyoutPlacementMode()
        {
            var ttv = TransformToVisual(Window.Current.Content);
            var menuCoords = ttv.TransformPoint(new Point(0, 0));

            if (Orientation == Orientation.Horizontal)
            {
                var menuCenter = menuCoords.Y + (ActualHeight / 2);

                if (menuCenter <= Window.Current.Bounds.Height / 2)
                {
                    return FlyoutPlacementMode.Bottom;
                }
                else
                {
                    return FlyoutPlacementMode.Top;
                }
            }
            else
            {
                var menuCenter = menuCoords.X + (ActualWidth / 2);

                if (menuCenter <= Window.Current.Bounds.Width / 2)
                {
                    return FlyoutPlacementMode.Right;
                }
                else
                {
                    return FlyoutPlacementMode.Left;
                }
            }
        }

        private static void OrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menu = (Menu)d;
            if (menu._wrapPanel != null)
            {
                menu._wrapPanel.Orientation = menu.Orientation;
            }

            menu.UpdateMenuItemsFlyoutPlacement();
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

        private void ShowMenuItemsToolTips()
        {
            foreach (MenuItem item in Items)
            {
                item.ShowTooltip();
            }
        }

        private void UnderlineMenuItems()
        {
            foreach (MenuItem item in Items)
            {
                item.Underline();
            }
        }

        private void RemoveUnderlineMenuItems()
        {
            foreach (MenuItem item in Items)
            {
                item.RemoveUnderline();
            }
        }

        private void HideMenuItemsTooltips()
        {
            foreach (MenuItem item in Items)
            {
                item.HideTooltip();
            }
        }

        internal void CalculateBounds()
        {
            var ttv = TransformToVisual(Window.Current.Content);
            Point screenCoords = ttv.TransformPoint(new Point(0, 0));
            _bounds.X = screenCoords.X;
            _bounds.Y = screenCoords.Y;
            _bounds.Width = ActualWidth;
            _bounds.Height = ActualHeight;

            foreach (MenuItem item in Items)
            {
                item.CalculateBounds();
            }
        }
    }
}
