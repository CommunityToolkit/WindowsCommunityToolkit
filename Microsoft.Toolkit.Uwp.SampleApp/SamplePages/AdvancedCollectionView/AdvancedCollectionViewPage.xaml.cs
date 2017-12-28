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
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.UI;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    using Microsoft.Toolkit.Uwp.SampleApp.Models;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdvancedCollectionViewPage : Page
    {
        public AdvancedCollectionViewPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Setup();
        }

        private void Setup()
        {
            // left list
            var oc = new ObservableCollection<Person>
            {
                new Person { Name = "Staff" },
                new Person { Name = "42" },
                new Person { Name = "Swan" },
                new Person { Name = "Orchid" },
                new Person { Name = "15" },
                new Person { Name = "Flame" },
                new Person { Name = "16" },
                new Person { Name = "Arrow" },
                new Person { Name = "Tempest" },
                new Person { Name = "23" },
                new Person { Name = "Pearl" },
                new Person { Name = "Hydra" },
                new Person { Name = "Lamp Post" },
                new Person { Name = "4" },
                new Person { Name = "Looking Glass" },
                new Person { Name = "8" },
            };

            LeftList.ItemsSource = oc;

            // right list
            var acv = new AdvancedCollectionView(oc);
            int nul;
            acv.Filter = x => !int.TryParse(((Person)x).Name, out nul);
            acv.SortDescriptions.Add(new SortDescription("Name", SortDirection.Ascending));

            RightList.ItemsSource = acv;

            // add button
            AddButton.Click += (sender, args) =>
            {
                if (!string.IsNullOrWhiteSpace(NewItemBox.Text))
                {
                    oc.Add(new Person { Name = NewItemBox.Text });
                }
            };
        }
    }
}
