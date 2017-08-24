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

using System;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class HamburgerMenuPage : IXamlRenderListener
    {
        private HamburgerMenu hamburgerMenuControl;
        private Grid contentGrid;

        public HamburgerMenuPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            contentGrid = control.FindChildByName("ContentGrid") as Grid;
            hamburgerMenuControl = control.FindDescendantByName("HamburgerMenu") as HamburgerMenu;
            if (hamburgerMenuControl != null)
            {
                hamburgerMenuControl.ItemClick += HamburgerMenu_OnItemClick;
                hamburgerMenuControl.OptionsItemClick += HamburgerMenu_OnOptionsItemClick;
            }
        }

        private void HamburgerMenu_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (contentGrid != null)
            {
                contentGrid.DataContext = e.ClickedItem;
            }
        }

        private async void HamburgerMenu_OnOptionsItemClick(object sender, ItemClickEventArgs e)
        {
            var menuItem = e.ClickedItem as HamburgerMenuItem;
            var dialog = new MessageDialog($"You clicked on {menuItem.Label} button");

            await dialog.ShowAsync();
        }
    }
}
