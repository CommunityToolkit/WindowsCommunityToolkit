// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.SampleApp.Common;
using Microsoft.Toolkit.Uwp.SampleApp.Data;
using Microsoft.Toolkit.Uwp.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class ListViewExtensionsPage : Page, IXamlRenderListener
    {
        public ListViewExtensionsPage()
        {
            this.InitializeComponent();
        }

        public ICommand SampleCommand => new DelegateCommand<string>(OnExecuteSampleCommand);

        public async void OnXamlRendered(FrameworkElement control)
        {
            var sampleListView = control.FindChild("SampleListView") as ListView;
            var indexInput = control.FindChild("IndexInput") as TextBlock;
            var itemPlacementInput = control.FindChild("ItemPlacementInput") as TextBlock;
            var disableAnimationInput = control.FindChild("DisableAnimationInput") as TextBlock;
            var scrollIfVisibileInput = control.FindChild("ScrollIfVisibileInput") as TextBlock;
            var additionalHorizontalOffsetInput = control.FindChild("AdditionalHorizontalOffsetInput") as TextBlock;
            var additionalVerticalOffsetInput = control.FindChild("AdditionalVerticalOffsetInput") as TextBlock;

            SampleController.Current.RegisterNewCommand("Start Smooth Scroll", (sender, args) =>
            {
                var index = int.Parse(indexInput.Text);
                var itemPlacement = (ItemPlacement)Enum.Parse(typeof(ItemPlacement), itemPlacementInput.Text);
                var disableAnimation = bool.Parse(disableAnimationInput.Text);
                var scrollIfVisibile = bool.Parse(scrollIfVisibileInput.Text);
                var additionalHorizontalOffset = int.Parse(additionalHorizontalOffsetInput.Text);
                var additionalVerticalOffset = int.Parse(additionalVerticalOffsetInput.Text);
                sampleListView.SmoothScrollIntoViewWithIndex(index, itemPlacement, disableAnimation, scrollIfVisibile, additionalHorizontalOffset, additionalVerticalOffset);
            });

            if (sampleListView != null)
            {
                sampleListView.ItemsSource = GetOddEvenSource(500);
            }

            // Transfer Data Context so we can access SampleCommand
            control.DataContext = this;
        }

        private async void OnExecuteSampleCommand(string item)
        {
            await new MessageDialog($"You clicked {item} via the 'ListViewExtensions.Command' binding", "Item Clicked").ShowAsync();
        }

        public ObservableCollection<string> GetOddEvenSource(int count)
        {
            var oddEvenSource = new ObservableCollection<string>();

            for (int number = 0; number <= count; number++)
            {
                var item = (number % 2) == 0 ? $"{number} - Even" : $"{number} - Odd";
                oddEvenSource.Add(item);
            }

            return oddEvenSource;
        }
    }
}