// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Graph;
using Microsoft.Toolkit.Uwp.UI.Extensions;
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

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && !string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Select(textBox.Text.Length, 0);
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

        private void SearchResultListBox_Tapped(object sender, TappedRoutedEventArgs e)
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

        private void SearchResultListBox_KeyUp(object sender, KeyRoutedEventArgs e)
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