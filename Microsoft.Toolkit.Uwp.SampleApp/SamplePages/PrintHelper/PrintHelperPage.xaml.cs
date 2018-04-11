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
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.Graphics.Printing;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class PrintHelperPage
    {
        private PrintHelper _printHelper;

        public PrintHelperPage()
        {
            InitializeComponent();

            ShowOrientationCheckBox.IsChecked = true;

            DefaultOrientationComboBox.ItemsSource = new List<PrintOrientation>()
            {
                PrintOrientation.Default,
                PrintOrientation.Portrait,
                PrintOrientation.Landscape
            };
            DefaultOrientationComboBox.SelectedIndex = 0;
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

            if (ShowOrientationCheckBox.IsChecked.HasValue && ShowOrientationCheckBox.IsChecked.Value)
            {
                printHelperOptions.AddDisplayOption(StandardPrintTaskOptions.Orientation);
            }

            await _printHelper.ShowPrintUIAsync("UWP Community Toolkit Sample App", printHelperOptions);
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

            if (ShowOrientationCheckBox.IsChecked.HasValue && ShowOrientationCheckBox.IsChecked.Value)
            {
                printHelperOptions.AddDisplayOption(StandardPrintTaskOptions.Orientation);
            }

            await _printHelper.ShowPrintUIAsync("UWP Community Toolkit Sample App", printHelperOptions, true);
        }

        private void CustomPrint_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Shell.Current.DisplayWaitRing = true;

            _printHelper = new PrintHelper(PrintCanvas);

            var pageNumber = 0;

            foreach (var item in PrintSampleItems)
            {
                var grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                // Header
                var header = new TextBlock { Text = "UWP Community Sample App Print Helper Custom Print", Margin = new Thickness(0, 0, 0, 20) };
                Grid.SetRow(header, 0);
                grid.Children.Add(header);

                // Main content
                var cont = new ContentControl();
                cont.ContentTemplate = Resources["CustomPrintTemplate"] as DataTemplate;
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

            if (ShowOrientationCheckBox.IsChecked.HasValue && ShowOrientationCheckBox.IsChecked.Value)
            {
                printHelperOptions.AddDisplayOption(StandardPrintTaskOptions.Orientation);
            }

            _printHelper.ShowPrintUIAsync("UWP Community Toolkit Sample App", printHelperOptions);
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
