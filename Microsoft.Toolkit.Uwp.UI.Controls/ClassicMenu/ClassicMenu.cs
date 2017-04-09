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
    public class ClassicMenu : ItemsControl
    {
        private const string CtrlValue = "CTRL";
        private const string ShiftValue = "SHIFT";

        // even if we have multiple menus in the same page we need only one cache because only one menu item will have certain short cut.
        private static readonly Dictionary<string, MenuFlyoutItem> MenuItemInputGestureCache;

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                "Orientation",
                typeof(Orientation),
                typeof(ClassicMenu),
                new PropertyMetadata(Orientation.Horizontal));

        static ClassicMenu()
        {
            MenuItemInputGestureCache =
            new Dictionary<string, MenuFlyoutItem>();
        }

        public ClassicMenu()
        {
            DefaultStyleKey = typeof(ClassicMenu);
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            Loaded -= ClassicMenu_Loaded;
            Dispatcher.AcceleratorKeyActivated -= Dispatcher_AcceleratorKeyActivated;

            Loaded += ClassicMenu_Loaded;
            Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
            base.OnApplyTemplate();
        }

        private void ClassicMenu_Loaded(object sender, RoutedEventArgs e)
        {
            var wrapPanel = (WrapPanel.WrapPanel)ItemsPanelRoot;
            wrapPanel.Orientation = Orientation;
        }

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

        public static readonly DependencyProperty InputGestureTextProperty = DependencyProperty.RegisterAttached("InputGestureText", typeof(string), typeof(MenuFlyoutItem), new PropertyMetadata(null, InputGestureTextChanged));

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

        private static void InputGestureTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var inputGestureValue = e.NewValue as string;
            if (string.IsNullOrEmpty(inputGestureValue) || MenuItemInputGestureCache.ContainsKey(inputGestureValue))
            {
                return;
            }

            var menuItem = (MenuFlyoutItem)obj;
            MenuItemInputGestureCache.Add(inputGestureValue.ToUpper(), menuItem);
        }

        public static string GetInputGestureText(MenuFlyoutItem obj)
        {
            return (string)obj.GetValue(InputGestureTextProperty);
        }

        public static void SetInputGestureText(MenuFlyoutItem obj, string value)
        {
            obj.SetValue(InputGestureTextProperty, value);
        }
    }
}
