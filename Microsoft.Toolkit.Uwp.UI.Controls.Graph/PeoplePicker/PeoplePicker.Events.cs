// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
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

        private void ClearAndHideSearchResultListBox()
        {
            SearchResults.Clear();
            HideSearchResults();
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
            }
        }

        private void SelectionsListBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement sourceElement
                && sourceElement.FindAscendantByName("PersonRemoveButton") is FrameworkElement removeButton
                && removeButton.Tag is Person person)
            {
                Selections.Remove(person);
                RaiseSelectionChanged();
            }
        }

        private void RaiseSelectionChanged()
        {
            SelectionChanged?.Invoke(this, new PeopleSelectionChangedEventArgs(Selections));
        }

        private void SearchBox_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _searchResultListBox.Width = _searchBox.ActualWidth;
        }

        private void SearchResultListBox_OnLayoutUpdated(object sender, object e)
        {
            try
            {
                if (_searchResultListBox.Items.Count > 0 &&
                    _searchResultListBox.ContainerFromIndex(0) is ListBoxItem item)
                {
                    double itemHeight = item.ActualHeight;
                    double itemsHeight = itemHeight * _searchResultListBox.Items.Count;
                    double height = Window.Current.Bounds.Height;
                    if (Window.Current.Content is DependencyObject content)
                    {
                        while (VisualTreeHelper.GetParent(content) is DependencyObject parent)
                        {
                            content = parent;
                        }

                        if (content is ScrollViewer scrollViewer)
                        {
                            height = scrollViewer.ViewportHeight;
                        }
                    }

                    DisplayInformation information = DisplayInformation.GetForCurrentView();
                    TextBoxAutomationPeer textBoxAutomationPeer = new TextBoxAutomationPeer(_searchBox);
                    Rect textBoxBounding = textBoxAutomationPeer.GetBoundingRectangle();
                    double baseY = textBoxBounding.Bottom / information.RawPixelsPerViewPixel;
                    double inputHeight = _searchBox.ActualHeight;

                    if (baseY != _lastBaseY || height != _lastHeight || _searchResultListBox.Items.Count != _lastSearchResultCount)
                    {
                        if (itemsHeight > height)
                        {
                            _searchResultListBox.Height = height;
                            _searchResultPopup.VerticalOffset = -baseY;
                        }
                        else
                        {
                            _searchResultListBox.Height = double.NaN;
                            if (baseY < 0)
                            {
                                _searchResultPopup.VerticalOffset = -baseY;
                            }
                            else if (height < baseY - inputHeight)
                            {
                                _searchResultPopup.VerticalOffset = height - baseY - itemsHeight;
                            }
                            else if (height - baseY > itemsHeight)
                            {
                                _searchResultPopup.VerticalOffset = 0d;
                            }
                            else if (baseY - inputHeight > itemsHeight)
                            {
                                _searchResultPopup.VerticalOffset = -itemsHeight - inputHeight;
                            }
                            else
                            {
                                _searchResultPopup.VerticalOffset = -baseY;
                            }
                        }
                    }

                    _lastBaseY = baseY;
                    _lastHeight = height;
                }
                else
                {
                    _lastBaseY = 0d;
                    _lastHeight = 0d;
                }

                _lastSearchResultCount = _searchResultListBox.Items.Count;
            }
            catch
            {
                _searchResultPopup.VerticalOffset = 0;
            }
        }
    }
}