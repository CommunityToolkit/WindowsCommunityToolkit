// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.WinUI.UI;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace CommunityToolkit.WinUI.SampleApp.SamplePages
{
    public sealed partial class ListViewExtensionsPage : Page, IXamlRenderListener
    {
        private ListView sampleListView;

        public ListViewExtensionsPage()
        {
            this.InitializeComponent();
            Load();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            sampleListView = control.FindChild("SampleListView") as ListView;

            if (sampleListView != null)
            {
                sampleListView.ItemsSource = GetOddEvenSource(201);
            }

            // Transfer Data Context so we can access SampleCommand
            control.DataContext = this;
        }

        private void Load()
        {
            SampleController.Current.RegisterNewCommand("Start Smooth Scroll", async (sender, args) =>
            {
                var index = int.TryParse(IndexInput.Text, out var i) ? i : 0;
                var itemPlacement = ItemPlacementInput.SelectedItem switch
                {
                    "Default" => ScrollItemPlacement.Default,
                    "Left" => ScrollItemPlacement.Left,
                    "Top" => ScrollItemPlacement.Top,
                    "Center" => ScrollItemPlacement.Center,
                    "Right" => ScrollItemPlacement.Right,
                    "Bottom" => ScrollItemPlacement.Bottom,
                    _ => ScrollItemPlacement.Default
                };

                var disableAnimation = DisableAnimationInput.IsChecked ?? false;
                var scrollIfVisibile = ScrollIfVisibileInput.IsChecked ?? true;
                var additionalHorizontalOffset = int.TryParse(AdditionalHorizontalOffsetInput.Text, out var ho) ? ho : 0;
                var additionalVerticalOffset = int.TryParse(AdditionalVerticalOffsetInput.Text, out var vo) ? vo : 0;
                UpdateScrollIndicator(true);
                await sampleListView.SmoothScrollIntoViewWithIndexAsync(index, itemPlacement, disableAnimation, scrollIfVisibile, additionalHorizontalOffset, additionalVerticalOffset);
                UpdateScrollIndicator(false);
            });

            if (sampleListView != null)
            {
                sampleListView.ItemsSource = GetOddEvenSource(500);
            }
        }

        private void UpdateScrollIndicator(bool isScrolling)
        {
            if (isScrolling)
            {
                ScrollIndicatorTest.Text = "Scrolling";
                ScrollIndicator.Fill = new SolidColorBrush(Colors.Green);
            }
            else
            {
                ScrollIndicator.Fill = new SolidColorBrush(Colors.Red);
                ScrollIndicatorTest.Text = "Not Scolling";
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

        private async void OnExecuteSampleCommand(string item)
        {
            await new ContentDialog
            {
                Title = "Item Clicked",
                Content = $"You clicked {item} via the 'ListViewExtensions.Command' binding",
                CloseButtonText = "Close",
                XamlRoot = Shell.Current.XamlRoot
            }.ShowAsync();
        }
    }
}
