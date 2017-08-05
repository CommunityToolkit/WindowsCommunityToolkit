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
using Windows.Foundation;
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
        private Control _lastFocusElement;
        private bool _altHandled;
        private bool _isLostFocus = true;
        private Control _lastFocusElementBeforeMenu;
        private Rect _bounds;

        private bool AllowTooltip => (bool)GetValue(AllowTooltipProperty);

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

        private void CoreWindow_PointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            // if contained with the whole Menu control
            if (IsOpened && _bounds.Contains(args.CurrentPoint.Position))
            {
                // if hover over current opened item
                if (SelectedMenuItem.ContainsPoint(args.CurrentPoint.Position))
                {
                    return;
                }

                foreach (MenuItem menuItem in Items)
                {
                    if (menuItem.ContainsPoint(args.CurrentPoint.Position))
                    {
                        SelectedMenuItem.HideMenu();
                        menuItem.Focus(FocusState.Keyboard);
                        menuItem.ShowMenu();
                    }
                }
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
            else
            {
                RemoveUnderlineSubItem();
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
                Debug.WriteLine("Enter");
                if (!IsOpened)
                {
                    Debug.WriteLine("Loop");
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
                        else
                        {
                            UnderlineSubItem();
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
    }
}
