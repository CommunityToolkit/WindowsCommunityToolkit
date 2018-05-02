using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    public partial class PeoplePicker : Control
    {
        private void ClearAndHideSearchResultListBox()
        {
            SearchResultList.Clear();
            SearchResultListBox.Visibility = Visibility.Collapsed;
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

            Loading.IsActive = true;

            searchText = Regex.Replace(searchText, "[^0-9a-zA-Z .@]", "");
            int cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = searchText;
            textboxSender.SelectionStart = cursorPosition;

            try
            {
                var options = new List<QueryOption>
                {
                    new QueryOption("$search", searchText)
                };
                IUserPeopleCollectionPage peopleList = await GraphClient.Me.People.Request(options).GetAsync();

                if (peopleList.Any())
                {
                    List<Person> searchResult = peopleList.Where(
                        u => !string.IsNullOrWhiteSpace(u.UserPrincipalName)).ToList();

                    // Remove all selected items
                    foreach (Person selectedItem in Selections)
                        searchResult.RemoveAll(u => u.UserPrincipalName == selectedItem.UserPrincipalName);

                    SearchResultList = new ObservableCollection<Person>(SearchResultLimit > 0
                        ? searchResult.Take(SearchResultLimit).ToList()
                        : searchResult);
                    SearchResultListBox.Visibility = Visibility.Visible;
                }
                else
                {
                    ClearAndHideSearchResultListBox();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                Loading.IsActive = false;
            }
        }

        private void SearchResultListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!((sender as ListBox)?.SelectedItem is Person person))
                return;
            if (!AllowMultiple && Selections.Any())
            {
                Selections.Clear();
                Selections.Add(person);
            }
            else
            {
                Selections.Add(person);
            }
            SelectionsCounter.Text = $"{Selections.Count} selected";
            SearchBox.Text = "";
        }

        private void DeleteSelectionItem(object parameter)
        {
            var upn = parameter as string;
            Person target = Selections.FirstOrDefault(u => u.UserPrincipalName == upn);
            if (target != null)
            {
                Selections.Remove(target);
                SelectionsCounter.Text = $"{Selections.Count} selected";
            }
        }
    }
}