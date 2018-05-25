// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
                control.Selections.Clear();
                control.RaiseSelectionChanged();
                control._searchBox.Text = string.Empty;
            }
        }

        private void ClearAndHideSearchResultListBox()
        {
            SearchResultList.Clear();
            _searchResultListBox.Visibility = Visibility.Collapsed;
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
                        new QueryOption("$filter", "personType/class eq 'Person' and personType/subclass eq 'OrganizationUser'")
                    };
                    IUserPeopleCollectionPage peopleList = await graphClient.Me.People.Request(options).GetAsync();

                    if (peopleList.Any())
                    {
                        List<Person> searchResult = peopleList.ToList();

                        // Remove all selected items
                        foreach (Person selectedItem in Selections)
                        {
                            searchResult.RemoveAll(u => u.UserPrincipalName == selectedItem.UserPrincipalName);
                        }

                        SearchResultList.Clear();
                        var result = SearchResultLimit > 0
                            ? searchResult.Take(SearchResultLimit).ToList()
                            : searchResult;
                        foreach (var item in result)
                        {
                            SearchResultList.Add(item);
                        }

                        _searchResultListBox.Visibility = Visibility.Visible;
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
            this.SelectionChanged?.Invoke(this, new PeopleSelectionChangedEventArgs(this.Selections));
        }
    }
}