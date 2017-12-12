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
        private MenuItem buildMenu;

        public MenuPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            fileMenu = control.FindChildByName("FileMenu") as MenuItem;
            buildMenu = control.FindChildByName("BuildMenu") as MenuItem;
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

            Shell.Current.RegisterNewCommand("Enable/disable build menu", (sender, args) =>
            {
                if (buildMenu != null)
                {
                    buildMenu.IsEnabled = !buildMenu.IsEnabled;
                }
            });
        }
    }
}
