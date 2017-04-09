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
    }
}
