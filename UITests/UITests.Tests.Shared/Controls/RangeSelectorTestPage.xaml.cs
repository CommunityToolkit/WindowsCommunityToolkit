// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UITests.App.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RangeSelectorTestPage : Page
    {
        public RangeSelectorTestPage()
        {
            this.InitializeComponent();
        }

        private void submitStepFrequency_Click(object sender, RoutedEventArgs e)
        {
            if(tryDoubleFromTextBox(inputStepFrequency, out var sf))
            {
                rangeSelector.StepFrequency = sf;
            }
        }

        private void submitMinimum_Click(object sender, RoutedEventArgs e)
        {
            if(tryDoubleFromTextBox(inputMinimum, out var min))
            {
                rangeSelector.Minimum = min;
            }
        }

        private void submitRangeStart_Click(object sender, RoutedEventArgs e)
        {
            if(tryDoubleFromTextBox(inputRangeStart, out var rStart))
            {
                rangeSelector.RangeStart = rStart;
            }
        }

        private void submitRangeEnd_Click(object sender, RoutedEventArgs e)
        {
            if(tryDoubleFromTextBox(inputRangeEnd, out var rEnd))
            {
                rangeSelector.RangeEnd = rEnd;
            }
        }

        private void submitMaximum_Click(object sender, RoutedEventArgs e)
        {
            if(tryDoubleFromTextBox(inputMaximum, out var max))
            {
                rangeSelector.Maximum = max;
            }
        }

        private void submitAll_Click(object sender, RoutedEventArgs e)
        {
            if(tryDoubleFromTextBox(inputStepFrequency, out var sf)
                && tryDoubleFromTextBox(inputMinimum, out var min)
                && tryDoubleFromTextBox(inputRangeStart, out var rStart)
                && tryDoubleFromTextBox(inputRangeEnd, out var rEnd)
                && tryDoubleFromTextBox(inputMaximum, out var max))
            {
                // This order is important. 
                // TODO (2021.04.28) - document that this order for changing these props is the most predicable.
                rangeSelector.Minimum = min;
                rangeSelector.Maximum = max;
                rangeSelector.StepFrequency = sf;
                rangeSelector.RangeStart = rStart;
                rangeSelector.RangeEnd = rEnd;
            }
        }

        private bool tryDoubleFromTextBox(TextBox tbx, out double value)
        {
            if (tbx.Text is string s && double.TryParse(s, out value))
            {
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
    }
}
