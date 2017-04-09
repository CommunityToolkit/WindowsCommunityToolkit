using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public partial class ClassicMenu
    {
        private static bool NavigateThrowMenuHeader(AcceleratorKeyEventArgs args, ClassicMenu menu, ClassicMenuItem menuItem, Orientation orientation)
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
                    var currentMenuItemIndex = menu.Items.IndexOf(menuItem);
                    if (currentMenuItemIndex > 0)
                    {
                        FocusManager.TryMoveFocus(FocusNavigationDirection.Left);
                        return true;
                    }
                }
                else if (args.VirtualKey == VirtualKey.Right)
                {
                    var currentMenuItemIndex = menu.Items.IndexOf(menuItem);
                    if (currentMenuItemIndex < menu.Items.Count - 1)
                    {
                        FocusManager.TryMoveFocus(FocusNavigationDirection.Right);
                        return true;
                    }
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
                    var currentMenuItemIndex = menu.Items.IndexOf(menuItem);
                    if (currentMenuItemIndex > 0)
                    {
                        FocusManager.TryMoveFocus(FocusNavigationDirection.Up);
                        return true;
                    }
                }
                else if (args.VirtualKey == VirtualKey.Down)
                {
                    var currentMenuItemIndex = menu.Items.IndexOf(menuItem);
                    if (currentMenuItemIndex < menu.Items.Count - 1)
                    {
                        FocusManager.TryMoveFocus(FocusNavigationDirection.Down);
                        return true;
                    }
                }
            }

            return false;
        }

        private static string MapInputToGestureKey(AcceleratorKeyEventArgs args)
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

        private void Dispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Menu && !args.KeyStatus.WasKeyDown)
            {
                Focus(FocusState.Programmatic);
            }

            var element = FocusManager.GetFocusedElement();
            var menuItem = element as ClassicMenuItem;
            if (menuItem != null)
            {
                if (NavigateThrowMenuHeader(args, this, menuItem, Orientation))
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
    }
}
