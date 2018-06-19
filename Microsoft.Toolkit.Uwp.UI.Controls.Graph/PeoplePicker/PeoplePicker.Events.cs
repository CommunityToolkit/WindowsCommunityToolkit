// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Defines the events for the <see cref="PeoplePicker"/> control.
    /// </summary>
    public partial class PeoplePicker : Control
    {
        private static void AllowMultiplePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PeoplePicker control && !control.AllowMultiple)
            {
                if (control.Selections != null)
                {
                    control.Selections.Clear();
                    control.RaiseSelectionChanged();
                }

                if (control._searchBox != null)
                {
                    control._searchBox.Text = string.Empty;
                }
            }
        }

        private static async void SearchPatternPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PeoplePicker peoplePicker)
            {
                await peoplePicker.SearchPeopleAsync(peoplePicker.SearchPattern);
            }
        }

        private async void SearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textboxSender = (TextBox)sender;
            string searchText = textboxSender.Text.Trim();
            await SearchPeopleAsync(searchText);
        }

        private void SearchResultListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is Person person)
            {
                if (!AllowMultiple && Selections.Any())
                {
                    Selections.Clear();
                    Selections.Add(person);
                }
                else
                {
                    Selections.Add(person);
                }

                RaiseSelectionChanged();
                _searchBox.Text = string.Empty;
                if (!string.IsNullOrWhiteSpace(GroupId))
                {
                    SearchBox_OnTextChanged(_searchBox, null);
                }
            }
        }

        private void SelectionsListBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement sourceElement
                && sourceElement.FindAscendantByName(PeoplePicker.PersonRemoveButtonName) is FrameworkElement removeButton
                && removeButton.DataContext is Person person)
            {
                Selections.Remove(person);
                RaiseSelectionChanged();
            }
        }

        private void SelectionsListBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter
                && e.OriginalSource is FrameworkElement source
                && source.Name == PersonRemoveButtonName
                && source.DataContext is Person person)
            {
                Selections.Remove(person);
                RaiseSelectionChanged();
            }
        }

        private void SearchBox_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _searchResultListBox.Width = _searchBox.ActualWidth;
        }

        private void SearchResultListBox_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement source)
            {
                var parent = VisualTreeHelper.GetParent(source);
                while (parent != null)
                {
                    parent = VisualTreeHelper.GetParent(parent);
                    if (parent is ListBoxItem item)
                    {
                        if (item.DataContext is Person person)
                        {
                            SelectPerson(person);
                        }

                        break;
                    }
                }
            }
        }

        private void SearchResultListBox_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter
                && _searchResultListBox.SelectedItem is Person person)
            {
                SelectPerson(person);
            }
        }

        private void Flyout_Closed(object sender, object e)
        {
            _searchBox.Opacity = 1;
            _searchBox.Focus(FocusState.Programmatic);
        }
    }
}