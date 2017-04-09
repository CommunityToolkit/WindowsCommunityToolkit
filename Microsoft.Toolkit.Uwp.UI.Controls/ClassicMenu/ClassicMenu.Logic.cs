using System.Text;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public partial class ClassicMenu
    {
        private static bool NavigateThrowMenuHeader(KeyEventArgs args, ClassicMenu menu, ClassicMenuItem menuItem, Orientation orientation)
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
    }
}
