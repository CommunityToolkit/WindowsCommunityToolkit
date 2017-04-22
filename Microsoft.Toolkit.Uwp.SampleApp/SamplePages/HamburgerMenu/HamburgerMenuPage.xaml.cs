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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class HamburgerMenuPage
    {
        private AutoSuggestBox _searchBox;

        public HamburgerMenuPage()
        {
            InitializeComponent();

            Loaded += HamburgerMenuPage_Loaded;
        }

        private void HamburgerMenuPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Connect to search UI
            ConnectToSearch();
        }

        private void HamburgerMenu_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ContentGrid.DataContext = e.ClickedItem;
        }

        private async void HamburgerMenu_OnOptionsItemClick(object sender, ItemClickEventArgs e)
        {
            var menuItem = e.ClickedItem as HamburgerMenuItem;
            var dialog = new MessageDialog($"You clicked on {menuItem.Label} button");

            await dialog.ShowAsync();
        }

        private void ConnectToSearch()
        {
            var searchButton = HamburgerMenuControl.FindDescendantByName("SearchButton") as Button;
            _searchBox = HamburgerMenuControl.FindDescendantByName("SearchBox") as AutoSuggestBox;

            if (_searchBox == null || searchButton == null)
            {
                return;
            }

            _searchBox.DisplayMemberPath = "Label";
            _searchBox.TextMemberPath = "Label";

            searchButton.Click += async (sender, args) =>
            {
                HamburgerMenuControl.IsPaneOpen = true;
                _searchBox.Text = string.Empty;

                UpdateSearchSuggestions();

                // We need to wait for the textbox to be created to focus it (only first time).
                TextBox innerTextbox = null;

                do
                {
                    innerTextbox = _searchBox.FindDescendant<TextBox>();
                    innerTextbox?.Focus(FocusState.Programmatic);

                    if (innerTextbox == null)
                    {
                        await Task.Delay(150);
                    }
                }
                while (innerTextbox == null);
            };

            _searchBox.QuerySubmitted += (sender, args) =>
            {
                var menuItem = (HamburgerMenuControl.ItemsSource as HamburgerMenuItemCollection)?
                    .FirstOrDefault(item => item.Label.ToLower() == args.QueryText.ToLower());

                if (menuItem != null)
                {
                    HamburgerMenuControl.SelectedItem = menuItem;
                    ContentGrid.DataContext = menuItem;
                    HamburgerMenuControl.IsPaneOpen = false;
                }

                _searchBox.Text = string.Empty;
            };

            _searchBox.TextChanged += (sender, args) =>
            {
                if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput)
                {
                    return;
                }

                UpdateSearchSuggestions();
            };

            UpdateSearchSuggestions();
        }

        private void UpdateSearchSuggestions()
        {
            _searchBox.ItemsSource = (HamburgerMenuControl.ItemsSource as HamburgerMenuItemCollection)?
                .Where(item => item.Label.ToLower().Contains(_searchBox.Text.ToLower()))
                .OrderBy(item => item.Label);
        }
    }
}
