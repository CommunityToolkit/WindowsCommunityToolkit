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

        private async void Print_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            SampleController.Current.DisplayWaitRing = true;

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
            SampleController.Current.DisplayWaitRing = true;

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

        private void ReleasePrintHelper()
        {
            _printHelper.Dispose();

            if (!DirectPrintContainer.Children.Contains(PrintableContent))
            {
                DirectPrintContainer.Children.Add(PrintableContent);
            }

            SampleController.Current.DisplayWaitRing = false;
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
    }
}
