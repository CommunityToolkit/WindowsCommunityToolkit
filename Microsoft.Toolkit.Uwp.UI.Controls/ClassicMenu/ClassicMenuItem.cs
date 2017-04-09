using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class ClassicMenuItem : ItemsControl
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(string), typeof(ClassicMenuItem), new PropertyMetadata(default(string)));
        private Button _flyoutButton;

        /// <summary>
        /// Gets or sets the title to appear in the title bar
        /// </summary>
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public ClassicMenuItem()
        {
            DefaultStyleKey = typeof(ClassicMenuItem);
            IsFocusEngagementEnabled = true;
        }

        public void ShowMenu()
        {
            _flyoutButton?.Flyout?.ShowAt(_flyoutButton);
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            _flyoutButton = GetTemplateChild("FlyoutButton") as Button;

            if (_flyoutButton != null)
            {
                var menuFlyout = new MenuFlyout();
                menuFlyout.Placement = ParentMenu.Orientation == Orientation.Horizontal
                    ? FlyoutPlacementMode.Bottom
                        : FlyoutPlacementMode.Right;
                if (Items != null)
                {
                    foreach (var item in Items)
                    {
                        var menuItem = item as MenuFlyoutItemBase;
                        if (menuItem != null)
                        {
                            menuFlyout.Items?.Add(menuItem);
                        }
                    }
                }

                _flyoutButton.Flyout = menuFlyout;
            }

            base.OnApplyTemplate();
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MenuFlyoutItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is MenuFlyoutItemBase;
        }

        private ClassicMenu ParentMenu => this.FindAscendant<ClassicMenu>();
    }
}
