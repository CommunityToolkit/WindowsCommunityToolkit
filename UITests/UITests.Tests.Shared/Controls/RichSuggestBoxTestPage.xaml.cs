// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Controls;

namespace UITests.App.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RichSuggestBoxTestPage : Page
    {
        private static readonly List<string> _suggestions = new() { "Token1", "Token2", "Token3" };

        public RichSuggestBoxTestPage()
        {
            this.InitializeComponent();
        }

        private void RichSuggestBox_OnSuggestionRequested(RichSuggestBox sender, SuggestionRequestedEventArgs args)
        {
            sender.ItemsSource = _suggestions;
        }

        private void RichSuggestBox_OnSuggestionChosen(RichSuggestBox sender, SuggestionChosenEventArgs args)
        {
            args.DisplayText = args.QueryText + (string)args.SelectedItem;
        }
    }
}