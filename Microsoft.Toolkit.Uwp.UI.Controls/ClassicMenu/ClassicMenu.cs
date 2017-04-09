using System.Collections.Generic;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public partial class ClassicMenu : ItemsControl
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
            Loaded += ClassicMenu_Loaded;
            base.OnApplyTemplate();
        }

        private void ClassicMenu_Loaded(object sender, RoutedEventArgs e)
        {
            var wrapPanel = (WrapPanel.WrapPanel)ItemsPanelRoot;
            wrapPanel.Orientation = Orientation;

            Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
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

        private void Dispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Menu && !args.KeyStatus.WasKeyDown)
            {
                Focus(FocusState.Keyboard);
            }
        }
    }
}
