using System.Linq;
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

            if (_flyoutButton != null && Items != null && Items.Any())
            {
                var menuFlyout = new MenuFlyout();
                menuFlyout.Placement = ParentMenu.Orientation == Orientation.Horizontal
                    ? FlyoutPlacementMode.Bottom
                        : FlyoutPlacementMode.Right;

                foreach (var item in Items)
                {
                    var menuItem = item as MenuFlyoutItemBase;
                    if (menuItem != null)
                    {
                        menuFlyout.Items?.Add(menuItem);
                    }
                }

                _flyoutButton.Flyout = menuFlyout;
            }

            base.OnApplyTemplate();
        }
        
        private ClassicMenu ParentMenu => this.FindAscendant<ClassicMenu>();
    }
}
