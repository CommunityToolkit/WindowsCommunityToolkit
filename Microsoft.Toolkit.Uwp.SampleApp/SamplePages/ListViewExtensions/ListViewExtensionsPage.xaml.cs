// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class ListViewExtensionsPage : Page, IXamlRenderListener
    {
        private ListView sampleListView;
        private TextBlock indexInput;
        private TextBlock itemPlacementInput;
        private TextBlock disableAnimationInput;
        private TextBlock scrollIfVisibileInput;
        private TextBlock additionalHorizontalOffsetInput;
        private TextBlock additionalVerticalOffsetInput;

        public ListViewExtensionsPage()
        {
            this.InitializeComponent();
            Load();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            sampleListView = control.FindChild("SampleListView") as ListView;
            indexInput = control.FindChild("IndexInput") as TextBlock;
            itemPlacementInput = control.FindChild("ItemPlacementInput") as TextBlock;
            disableAnimationInput = control.FindChild("DisableAnimationInput") as TextBlock;
            scrollIfVisibileInput = control.FindChild("ScrollIfVisibileInput") as TextBlock;
            additionalHorizontalOffsetInput = control.FindChild("AdditionalHorizontalOffsetInput") as TextBlock;
            additionalVerticalOffsetInput = control.FindChild("AdditionalVerticalOffsetInput") as TextBlock;

            if (sampleListView != null)
            {
                sampleListView.ItemsSource = GetOddEvenSource(201);
            }

            // Transfer Data Context so we can access SampleCommand
            control.DataContext = this;
        }

        private void Load()
        {
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
        }

        private ObservableCollection<string> GetOddEvenSource(int count)
        {
            var oddEvenSource = new ObservableCollection<string>();

            for (int number = 0; number < count; number++)
            {
                var item = (number % 2) == 0 ? $"{number} - Even" : $"{number} - Odd";
                oddEvenSource.Add(item);
            }

            return oddEvenSource;
        }
    }

#pragma warning disable SA1402 // File may only contain a single class
    internal class SampleCommand : ICommand
#pragma warning restore SA1402 // File may only contain a single class
    {
        event EventHandler ICommand.CanExecuteChanged
        {
            add { }
            remove { }
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameter is string s)
            {
                OnExecuteSampleCommand(s);
            }
        }

        private static async void OnExecuteSampleCommand(string item)
        {
            await new MessageDialog($"You clicked {item} via the 'ListViewExtensions.Command' binding", "Item Clicked").ShowAsync();
        }
    }
}