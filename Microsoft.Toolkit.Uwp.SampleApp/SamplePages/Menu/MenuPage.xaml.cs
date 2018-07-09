// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class MenuPage : IXamlRenderListener
    {
        private MenuItem fileMenu;

        public MenuPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            fileMenu = control.FindChildByName("FileMenu") as MenuItem;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Append Item to file menu", (sender, args) =>
            {
                if (fileMenu != null)
                {
                    var flyoutItem = new MenuFlyoutItem
                    {
                        Text = "Click to remove"
                    };

                    flyoutItem.Click += (a, b) =>
                    {
                        fileMenu.Items.Remove(flyoutItem);
                    };

                    fileMenu.Items.Add(flyoutItem);
                }
            });

            Shell.Current.RegisterNewCommand("Prepend Item to file menu", (sender, args) =>
            {
                if (fileMenu != null)
                {
                    var flyoutItem = new MenuFlyoutItem
                    {
                        Text = "Click to remove"
                    };

                    flyoutItem.Click += (a, b) =>
                    {
                        fileMenu.Items.Remove(flyoutItem);
                    };

                    fileMenu.Items.Insert(0, flyoutItem);
                }
            });
        }
    }
}
