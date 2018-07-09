// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Graphics.Printing;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class PrintHelperPage : IXamlRenderListener
    {
        private PrintHelper _printHelper;
        private DataTemplate customPrintTemplate;

        public PrintHelperPage()
        {
            InitializeComponent();

            ShowOrientationSwitch.IsOn = true;

            DefaultOrientationComboBox.ItemsSource = new List<PrintOrientation>()
            {
                PrintOrientation.Default,
                PrintOrientation.Portrait,
                PrintOrientation.Landscape
            };
            DefaultOrientationComboBox.SelectedIndex = 0;
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            var listView = control.FindChildByName("PrintSampleListView") as ListView;
            if (listView == null)
            {
                customPrintTemplate = null;
                return;
            }

            listView.ItemsSource = PrintSampleItems;

            try
            {
                customPrintTemplate = listView.Resources["CustomPrintTemplate"] as DataTemplate;
            }
            catch (Exception)
            {
                customPrintTemplate = null;
            }
        }

        internal List<PrintSampleItem> PrintSampleItems
        {
            get
            {
                return new List<PrintSampleItem>
                {
                    new PrintSampleItem
                    {
                        PicturePath = "/Assets/Photos/LakeAnnMushroom.jpg",
                        Description = "A Smurf house is a standard form of residence for a Smurf that's shaped like a mushroom. It is a two-floor house that typically has one door, a few ground-floor windows and some rooftop windows, and a chimney stack.",
                        SourceUrl = @"From http://http://smurfs.wikia.com/wiki/Smurf_house"
                    },
                    new PrintSampleItem
                    {
                        PicturePath = "/Assets/Photos/Owl.jpg",
                        Description = "O RLY? is an Internet phenomenon, typically presented as an image macro featuring a snowy owl. The phrase 'O RLY?', an abbreviated form of 'Oh, really?', is popularly used in Internet forums in a sarcastic manner, often in response to an obvious, predictable, or blatantly false statement.",
                        SourceUrl = @"From https://en.wikipedia.org/wiki/O_RLY%3F"
                    }
                };
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Print", Print_Click);
            Shell.Current.RegisterNewCommand("Direct Print", DirectPrint_Click);
            Shell.Current.RegisterNewCommand("Custom Print", CustomPrint_Click);
        }

        private async void Print_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Shell.Current.DisplayWaitRing = true;

            DirectPrintContainer.Children.Remove(PrintableContent);

            _printHelper = new PrintHelper(Container);
            _printHelper.AddFrameworkElementToPrint(PrintableContent);

            _printHelper.OnPrintCanceled += PrintHelper_OnPrintCanceled;
            _printHelper.OnPrintFailed += PrintHelper_OnPrintFailed;
            _printHelper.OnPrintSucceeded += PrintHelper_OnPrintSucceeded;

            var printHelperOptions = new PrintHelperOptions(false);
            printHelperOptions.Orientation = (PrintOrientation)DefaultOrientationComboBox.SelectedItem;

            if (ShowOrientationSwitch.IsOn)
            {
                printHelperOptions.AddDisplayOption(StandardPrintTaskOptions.Orientation);
            }

            await _printHelper.ShowPrintUIAsync("Windows Community Toolkit Sample App", printHelperOptions);
        }

        private async void DirectPrint_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Shell.Current.DisplayWaitRing = true;

            _printHelper = new PrintHelper(DirectPrintContainer);

            _printHelper.OnPrintCanceled += PrintHelper_OnPrintCanceled;
            _printHelper.OnPrintFailed += PrintHelper_OnPrintFailed;
            _printHelper.OnPrintSucceeded += PrintHelper_OnPrintSucceeded;

            var printHelperOptions = new PrintHelperOptions(false);
            printHelperOptions.Orientation = (PrintOrientation)DefaultOrientationComboBox.SelectedItem;

            if (ShowOrientationSwitch.IsOn)
            {
                printHelperOptions.AddDisplayOption(StandardPrintTaskOptions.Orientation);
            }

            await _printHelper.ShowPrintUIAsync("Windows Community Toolkit Sample App", printHelperOptions, true);
        }

        private async void CustomPrint_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (customPrintTemplate == null)
            {
                var dialog = new MessageDialog("Could not find the data template resource called 'CustomPrintTemplate' under the listview called 'PrintSampleListView'.", "Incomplete XAML");
                await dialog.ShowAsync();
                return;
            }

            Shell.Current.DisplayWaitRing = true;

            // Provide an invisible container
            _printHelper = new PrintHelper(CustomPrintContainer);

            var pageNumber = 0;

            foreach (var item in PrintSampleItems)
            {
                var grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                // Static header
                var header = new TextBlock { Text = "Windows Community Toolkit Sample App - Print Helper - Custom Print", Margin = new Thickness(0, 0, 0, 20) };
                Grid.SetRow(header, 0);
                grid.Children.Add(header);

                // Main content with layout from data template
                var cont = new ContentControl();
                cont.ContentTemplate = customPrintTemplate;
                cont.DataContext = item;
                Grid.SetRow(cont, 1);
                grid.Children.Add(cont);

                // Footer with page number
                pageNumber++;
                var footer = new TextBlock { Text = string.Format("page {0}", pageNumber), Margin = new Thickness(0, 20, 0, 0) };
                Grid.SetRow(footer, 2);
                grid.Children.Add(footer);

                _printHelper.AddFrameworkElementToPrint(grid);
            }

            _printHelper.OnPrintCanceled += PrintHelper_OnPrintCanceled;
            _printHelper.OnPrintFailed += PrintHelper_OnPrintFailed;
            _printHelper.OnPrintSucceeded += PrintHelper_OnPrintSucceeded;

            var printHelperOptions = new PrintHelperOptions(false);
            printHelperOptions.Orientation = (PrintOrientation)DefaultOrientationComboBox.SelectedItem;

            if (ShowOrientationSwitch.IsOn)
            {
                printHelperOptions.AddDisplayOption(StandardPrintTaskOptions.Orientation);
            }

            await _printHelper.ShowPrintUIAsync("Windows Community Toolkit Sample App", printHelperOptions);
        }

        private void ReleasePrintHelper()
        {
            _printHelper.Dispose();

            if (!DirectPrintContainer.Children.Contains(PrintableContent))
            {
                DirectPrintContainer.Children.Add(PrintableContent);
            }

            Shell.Current.DisplayWaitRing = false;
        }

        private async void PrintHelper_OnPrintSucceeded()
        {
            ReleasePrintHelper();
            var dialog = new MessageDialog("Printing done.");
            await dialog.ShowAsync();
        }

        private async void PrintHelper_OnPrintFailed()
        {
            ReleasePrintHelper();
            var dialog = new MessageDialog("Printing failed.");
            await dialog.ShowAsync();
        }

        private void PrintHelper_OnPrintCanceled()
        {
            ReleasePrintHelper();
        }

        internal class PrintSampleItem
        {
            public string PicturePath { get; set; }

            public string Description { get; set; }

            public string SourceUrl { get; set; }
        }
    }
}
