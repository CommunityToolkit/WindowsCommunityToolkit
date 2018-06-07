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
            var control = d as PeoplePicker;
            if (!control.AllowMultiple)
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
            _searchResultPopup.IsOpen = false;
        }

        private async void SearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textboxSender = (TextBox)sender;
            string searchText = textboxSender.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                ClearAndHideSearchResultListBox();
                return;
            }

            IsLoading = true;
            try
            {
                var graphService = MicrosoftGraphService.Instance;
                await graphService.TryLoginAsync();
                GraphServiceClient graphClient = graphService.GraphProvider;

                if (graphClient != null)
                {
                    var options = new List<QueryOption>
                    {
                        new QueryOption("$search", $"\"{searchText}\""),
                        new QueryOption("$filter", "personType/class eq 'Person' and personType/subclass eq 'OrganizationUser'"),
                        new QueryOption("$top", (SearchResultLimit>0?SearchResultLimit:DefaultSearchResultLimit).ToString())
                    };
                    IUserPeopleCollectionPage rawResults = await graphClient.Me.People.Request(options).GetAsync();

                    if (rawResults.Any())
                    {
                        SearchResults.Clear();

                        var results = rawResults.Where(o => !Selections.Any(s => s.Id == o.Id))
                            .Take(SearchResultLimit > 0 ? SearchResultLimit : DefaultSearchResultLimit);
                        foreach (var item in results)
                        {
                            SearchResults.Add(item);
                        }

                        _searchResultPopup.IsOpen = true;
                    }
                    else
                    {
                        ClearAndHideSearchResultListBox();
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void SearchResultListBox_OnSelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
#pragma warning disable SA1119 // Statement must not use unnecessary parenthesis
            if (!((sender as ListBox)?.SelectedItem is Person person))
#pragma warning restore SA1119 // Statement must not use unnecessary parenthesis
            {
                return;
            }

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

        private void SelectionsListBox_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var elem = e.OriginalSource as FrameworkElement;

            var removeButton = elem.FindAscendantByName("PersonRemoveButton");
            if (removeButton != null)
            {
                if (removeButton.Tag is Person item)
                {
                    Selections.Remove(item);
                    RaiseSelectionChanged();
                }
            }
        }

        private void RaiseSelectionChanged()
        {
            SelectionChanged?.Invoke(this, new PeopleSelectionChangedEventArgs(this.Selections));
        }

        private void SearchBox_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _searchResultListBox.Width = _searchBox.ActualWidth;
        }

        private void SearchResultListBox_OnLayoutUpdated(object sender, object e)
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

        private void SearchBox_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                InputPane inputPane = InputPane.GetForCurrentView();
                if (inputPane != null)
                {
                    inputPane.TryHide();
                }
            }
        }
    }
}