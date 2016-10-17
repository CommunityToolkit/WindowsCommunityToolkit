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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// Sample page showing the MasterDetail control
    /// </summary>
    public sealed partial class MasterDetailPage
    {
        public MasterDetailPage()
        {
            InitializeComponent();

            var list = new List<DetailItem>();
            for (var i = 0; i < 30; i++)
            {
                list.Add(new DetailItem
                {
                    Subject = $"This is the update for {DateTime.Now.AddDays(-i).Date:d}"
                });
            }

            ItemsList.ItemsSource = list;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var propertyDesc = e.Parameter as PropertyDescriptor;

            if (propertyDesc != null)
            {
                DataContext = propertyDesc.Expando;
            }
        }

        private void ItemsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Any())
            {
                MasterDetail.Detail = e.AddedItems.First();
            }
        }

        public class DetailItem
        {
            public string Header { get; } = "Update provider";

            public string Subject { get; set; }

            public string Content { get; } = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras mollis placerat diam, eu lacinia felis faucibus non. Cras nec tristique metus, a hendrerit libero. Fusce at est sed turpis lobortis euismod quis et urna. Praesent id sapien sit amet nibh finibus rutrum eget at tortor. Vestibulum purus nibh, vulputate in vehicula non, iaculis ac elit. Quisque imperdiet iaculis laoreet. Mauris sodales ante eu enim commodo euismod. Quisque quis mi laoreet, ultricies dolor et, dapibus ante. Aenean vel elit nisi. Morbi lectus lorem, tincidunt a auctor eget, vehicula dictum turpis. Etiam sit amet vehicula lectus.";
        }
    }
}
