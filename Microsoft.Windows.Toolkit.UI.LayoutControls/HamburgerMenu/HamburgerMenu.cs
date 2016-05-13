using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    public partial class HamburgerMenu : ContentControl
    {
        Button hamburgerButton;
        SplitView mainSplitView;
        ListView buttonsListView;

        /// <summary>
        /// Create a new instance of a HamburgerMenu.
        /// </summary>
        public HamburgerMenu()
        {
            DefaultStyleKey = typeof(HamburgerMenu);
        }

        /// <summary>
        /// Override default OnApplyTemplate to capture children controls
        /// </summary>
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
