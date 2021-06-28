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
        private TextBox indexInput;
        private ComboBox itemPlacementInput;
        private CheckBox disableAnimationInput;
        private CheckBox scrollIfVisibileInput;
        private TextBox additionalHorizontalOffsetInput;
        private TextBox additionalVerticalOffsetInput;

        public ListViewExtensionsPage()
        {
            this.InitializeComponent();
            Load();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            sampleListView = control.FindChild("SampleListView") as ListView;
            indexInput = control.FindChild("IndexInput") as TextBox;
            itemPlacementInput = control.FindChild("ItemPlacementInput") as ComboBox;
            disableAnimationInput = control.FindChild("DisableAnimationInput") as CheckBox;
            scrollIfVisibileInput = control.FindChild("ScrollIfVisibileInput") as CheckBox;
            additionalHorizontalOffsetInput = control.FindChild("AdditionalHorizontalOffsetInput") as TextBox;
            additionalVerticalOffsetInput = control.FindChild("AdditionalVerticalOffsetInput") as TextBox;

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
                var index = int.TryParse(indexInput.Text, out var i) ? i : 0;
                var itemPlacement = itemPlacementInput.SelectedItem switch
                {
                    "Default" => UI.ItemPlacement.Default,
                    "Left" => UI.ItemPlacement.Left,
                    "Top" => UI.ItemPlacement.Top,
                    "Center" => UI.ItemPlacement.Center,
                    "Right" => UI.ItemPlacement.Right,
                    "Bottom" => UI.ItemPlacement.Bottom,
                    _ => UI.ItemPlacement.Default
                };

                var disableAnimation = disableAnimationInput.IsChecked ?? false;
                var scrollIfVisibile = scrollIfVisibileInput.IsChecked ?? true;
                var additionalHorizontalOffset = int.TryParse(additionalHorizontalOffsetInput.Text, out var ho) ? ho : 0;
                var additionalVerticalOffset = int.TryParse(additionalVerticalOffsetInput.Text, out var vo) ? vo : 0;
                sampleListView.SmoothScrollIntoViewWithIndexAsync(index, itemPlacement, disableAnimation, scrollIfVisibile, additionalHorizontalOffset, additionalVerticalOffset);
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