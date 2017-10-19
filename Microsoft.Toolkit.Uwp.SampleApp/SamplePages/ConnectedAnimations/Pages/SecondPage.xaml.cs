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

using Microsoft.Toolkit.Uwp.SampleApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages.ConnectedAnimations.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SecondPage : Page
    {
        private static List<Item> items;

        public SecondPage()
        {
            this.InitializeComponent();

            if (items == null)
            {
                items = new List<Item>();
                items.Add(new Item() { Title = "Test 1" });
                items.Add(new Item() { Title = "Test 2" });
                items.Add(new Item() { Title = "Test 3" });
                items.Add(new Item() { Title = "Test 4" });
                items.Add(new Item() { Title = "Test 5" });
                items.Add(new Item() { Title = "Test 6" });
                items.Add(new Item() { Title = "Test 7" });
                items.Add(new Item() { Title = "Test 8" });
                items.Add(new Item() { Title = "Test 9" });
                items.Add(new Item() { Title = "Test 10" });
                items.Add(new Item() { Title = "Test 11" });
                items.Add(new Item() { Title = "Test 12" });
                items.Add(new Item() { Title = "Test 13" });
                items.Add(new Item() { Title = "Test 14" });
                items.Add(new Item() { Title = "Test 15" });
                items.Add(new Item() { Title = "Test 16" });
                items.Add(new Item() { Title = "Test 17" });
                items.Add(new Item() { Title = "Test 18" });
            }

            listView.ItemsSource = items;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(ThirdPage), e.ClickedItem);
        }
    }
}
