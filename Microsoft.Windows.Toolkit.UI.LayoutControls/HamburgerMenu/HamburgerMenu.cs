using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Windows.Toolkit.UI.LayoutControls
{
    public sealed partial class HamburgerMenu : ContentControl
    {
        Button hamburgerButton;
        SplitView mainSplitView;
        ListView buttonsListView;

        public HamburgerMenu()
        {
            DefaultStyleKey = typeof(HamburgerMenu);
        }

        protected override void OnApplyTemplate()
        {
            hamburgerButton = (Button)GetTemplateChild("HamburgerButton");
            mainSplitView = (SplitView)GetTemplateChild("MainSplitView");
            buttonsListView = (ListView)GetTemplateChild("ButtonsListView");

            hamburgerButton.Click += HamburgerButton_Click;
            buttonsListView.ItemClick += ButtonsListView_ItemClick;

            base.OnApplyTemplate();
        }
    }
}
