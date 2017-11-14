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

using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages.ConnectedAnimations.Pages
{
    public sealed partial class SecondPage : Page
    {
        private static List<Item> items;

        public SecondPage()
        {
            this.InitializeComponent();

            if (items == null)
            {
                items = new List<Item>
                {
                    new Item() { Title = "Test 1" },
                    new Item() { Title = "Test 2" },
                    new Item() { Title = "Test 3" },
                    new Item() { Title = "Test 4" },
                    new Item() { Title = "Test 5" },
                    new Item() { Title = "Test 6" },
                    new Item() { Title = "Test 7" },
                    new Item() { Title = "Test 8" },
                    new Item() { Title = "Test 9" },
                    new Item() { Title = "Test 10" },
                    new Item() { Title = "Test 11" },
                    new Item() { Title = "Test 12" },
                    new Item() { Title = "Test 13" },
                    new Item() { Title = "Test 14" },
                    new Item() { Title = "Test 15" },
                    new Item() { Title = "Test 16" },
                    new Item() { Title = "Test 17" },
                    new Item() { Title = "Test 18" }
                };
            }

            listView.ItemsSource = items;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(ThirdPage), e.ClickedItem);
        }
    }
}
