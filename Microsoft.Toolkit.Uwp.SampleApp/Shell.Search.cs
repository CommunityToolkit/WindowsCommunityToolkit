// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public sealed partial class Shell
    {
        internal void StartSearch(string startingText = null)
        {
            if (FocusManager.GetFocusedElement() == SearchBox.FindDescendant<TextBox>())
            {
                return;
            }

            SearchBox.Focus(FocusState.Keyboard);

            var currentSearchText = SearchBox.Text;

            SearchBox.Text = string.Empty;

            if (!string.IsNullOrWhiteSpace(startingText))
            {
                SearchBox.Text = startingText;
            }
            else
            {
                SearchBox.Text = currentSearchText;
            }
        }

        private async void UpdateSearchSuggestions(bool focus = false)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                HideSamplePicker();
                return;
            }

            var samples = (await Samples.FindSample(SearchBox.Text)).OrderBy(s => s.Name).ToArray();
            if (samples.Count() > 0)
            {
                ShowSamplePicker(samples);
                if (focus)
                {
                    SamplePickerGridView.Focus(FocusState.Keyboard);
                }
            }
            else
            {
                HideSamplePicker();
            }
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            UpdateSearchSuggestions();
        }

        private void SearchBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Down)
            {
                UpdateSearchSuggestions(true);
            }
        }

        private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            UpdateSearchSuggestions();
        }
    }
}
