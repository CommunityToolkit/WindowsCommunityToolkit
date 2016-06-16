// *********************************************************
//  Copyright (c) Microsoft. All rights reserved.
//  This code is licensed under the MIT License (MIT).
//  THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//  DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
//  TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
//  THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// *********************************************************

using System;

using Microsoft.Windows.Toolkit.SampleApp.Data;
using Microsoft.Windows.Toolkit.SampleApp.Models;

using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Windows.Toolkit.SampleApp.SamplePages
{
    public class OptionMenuItem
    {
        public string Name { get; set; }
        public string Glyph { get; set; }
    }

    public sealed partial class HamburgerMenuPage
    {
        public HamburgerMenuPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var propertyDesc = e.Parameter as PropertyDescriptor;

            if (propertyDesc != null)
            {
                DataContext = propertyDesc.Expando;
            }

            HamburgerMenu.ItemsSource = new PhotosDataSource().GetItems();

            HamburgerMenu.OptionsItemsSource = new [] { new OptionMenuItem { Glyph = "", Name = "About" } };
        }

        private void HamburgerMenu_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ContentGrid.DataContext = e.ClickedItem;
        }

        private async void HamburgerMenu_OnOptionsItemClick(object sender, ItemClickEventArgs e)
        {
            var menuItem = e.ClickedItem as OptionMenuItem;
            var dialog = new MessageDialog($"You clicked on {menuItem.Name} button");

            await dialog.ShowAsync();
        }
    }
}
