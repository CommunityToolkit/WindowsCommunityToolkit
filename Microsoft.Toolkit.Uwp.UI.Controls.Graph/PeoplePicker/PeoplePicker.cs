// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The PeoplePicker Control is a simple control that allows for selection of one or more users from an organizational AD.
    /// </summary>
    [TemplatePart(Name = SearchBoxPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = InternalSearchBoxTextBoxPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = SearchResultListBoxPartName, Type = typeof(ListBox))]
    [TemplatePart(Name = SelectionsListBoxPartName, Type = typeof(ListBox))]
    [TemplatePart(Name = SearchResultPopupName, Type = typeof(Popup))]
    [TemplatePart(Name = FlyoutContainerPartName, Type = typeof(FrameworkElement))]
    public partial class PeoplePicker : Control
    {
        private const string SearchBoxPartName = "SearchBox";
        private const string SearchResultListBoxPartName = "SearchResultListBox";
        private const string SearchResultPopupName = "SearchResultPopup";
        private const string SelectionsListBoxPartName = "SelectionsListBox";
        private const string FlyoutContainerPartName = "FlyoutContainer";
        private const string InternalSearchBoxTextBoxPartName = "InternalSearchBox";
        private const string PersonRemoveButtonName = "PersonRemoveButton";

        private FrameworkElement _flyoutContainer;
        private TextBox _searchBox;
        private TextBox _internalSearchBox;
        private ListBox _searchResultListBox;
        private ListBox _selectionsListBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeoplePicker"/> class.
        /// </summary>
        public PeoplePicker()
        {
            DefaultStyleKey = typeof(PeoplePicker);

            SearchResults = new ObservableCollection<Person>();
            Selections = new ObservableCollection<Person>();
        }

        /// <summary>
        /// Called when applying the control template.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            IsLoading = false;

            if (_flyoutContainer != null)
            {
                var flyout = FlyoutBase.GetAttachedFlyout(_flyoutContainer);
                if (flyout != null)
                {
                    flyout.Closed -= Flyout_Closed;
                }
            }

            if (_searchBox != null)
            {
                _searchBox.SizeChanged -= SearchBox_OnSizeChanged;
                _searchBox.GotFocus -= SearchBox_GotFocus;
            }

            if (_internalSearchBox != null)
            {
                _internalSearchBox.GotFocus -= SearchBox_GotFocus;
            }

            if (_searchResultListBox != null)
            {
                _searchResultListBox.Tapped -= SearchResultListBox_Tapped;
                _searchResultListBox.KeyUp -= SearchResultListBox_KeyUp;
            }

            if (_selectionsListBox != null)
            {
                _selectionsListBox.Tapped -= SelectionsListBox_Tapped;
                if (!IsWindowsPhone)
                {
                    _selectionsListBox.PreviewKeyUp -= SelectionsListBox_KeyUp;
                }
            }

            _searchBox = GetTemplateChild(SearchBoxPartName) as TextBox;
            _internalSearchBox = GetTemplateChild(InternalSearchBoxTextBoxPartName) as TextBox;
            _searchResultListBox = GetTemplateChild(SearchResultListBoxPartName) as ListBox;
            _selectionsListBox = GetTemplateChild(SelectionsListBoxPartName) as ListBox;
            _flyoutContainer = GetTemplateChild(FlyoutContainerPartName) as FrameworkElement;

            if (_flyoutContainer != null)
            {
                var flyout = FlyoutBase.GetAttachedFlyout(_flyoutContainer);
                if (flyout != null)
                {
                    flyout.Closed += Flyout_Closed;
                }
            }

            if (_searchBox != null)
            {
                _searchBox.SizeChanged += SearchBox_OnSizeChanged;
                _searchBox.GotFocus += SearchBox_GotFocus;
            }

            if (_internalSearchBox != null)
            {
                _internalSearchBox.GotFocus += SearchBox_GotFocus;
            }

            if (_searchResultListBox != null)
            {
                _searchResultListBox.Tapped += SearchResultListBox_Tapped;
                _searchResultListBox.KeyUp += SearchResultListBox_KeyUp;
            }

            if (_selectionsListBox != null)
            {
                _selectionsListBox.Tapped += SelectionsListBox_Tapped;
                if (!IsWindowsPhone)
                {
                    _selectionsListBox.PreviewKeyUp += SelectionsListBox_KeyUp;
                }
            }

            base.OnApplyTemplate();
        }

        private void ClearAndHideSearchResultListBox()
        {
            SearchResults.Clear();
            HideSearchResults();
        }

        private void ShowSearchResults()
        {
            FlyoutBase.ShowAttachedFlyout(_flyoutContainer);
            _searchBox.Opacity = 0;
        }

        private void HideSearchResults()
        {
            FlyoutBase.GetAttachedFlyout(_flyoutContainer)?.Hide();
        }

        private void RaiseSelectionChanged()
        {
            SelectionChanged?.Invoke(this, new PeopleSelectionChangedEventArgs(Selections));
        }

        private Person GetPersonFromUser(User user)
        {
            Person person = new Person
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                GivenName = user.GivenName,
                Surname = user.Surname,
                JobTitle = user.JobTitle,
                OfficeLocation = user.OfficeLocation,
                UserPrincipalName = user.UserPrincipalName,
                ScoredEmailAddresses = new ScoredEmailAddress[] { new ScoredEmailAddress() { Address = user.Mail } },
                AdditionalData = new Dictionary<string, object>() { [EtagHelper.ODataEtagPropertyName] = user.GetEtag() }
            };
            return person;
        }

        private async Task SearchPeopleAsync(string searchPattern)
        {
            if (string.IsNullOrWhiteSpace(searchPattern))
            {
                ClearAndHideSearchResultListBox();
                return;
            }

            IsLoading = true;
            try
            {
                GraphServiceClient graphClient = await GraphServiceHelper.GetGraphServiceClientAsync();
                if (graphClient != null)
                {
                    int searchLimit = SearchResultLimit > 0 ? SearchResultLimit : DefaultSearchResultLimit;
                    int queryLimit = searchLimit + Selections.Count;
                    IEnumerable<Person> rawResults;
                    if (string.IsNullOrWhiteSpace(GroupId))
                    {
                        var options = new List<QueryOption>
                        {
                            new QueryOption("$search", $"\"{searchPattern}\""),
                            new QueryOption("$filter", "personType/class eq 'Person' and personType/subclass eq 'OrganizationUser'"),
                            new QueryOption("$top", queryLimit.ToString())
                        };
                        rawResults = await graphClient.Me.People.Request(options).GetAsync();
                    }
                    else
                    {
                        IGroupMembersCollectionWithReferencesPage userRequest = await graphClient.Groups[GroupId].Members.Request().GetAsync();
                        List<Person> allPersons = new List<Person>();
                        while (true)
                        {
                            foreach (User user in userRequest)
                            {
                                if (string.IsNullOrEmpty(searchPattern)
                                    || (!string.IsNullOrEmpty(user.DisplayName) && user.DisplayName.StartsWith(searchPattern, StringComparison.OrdinalIgnoreCase))
                                    || (!string.IsNullOrEmpty(user.Surname) && user.Surname.StartsWith(searchPattern, StringComparison.OrdinalIgnoreCase)))
                                {
                                    Person person = GetPersonFromUser(user);
                                    allPersons.Add(person);
                                }

                                if (allPersons.Count >= queryLimit)
                                {
                                    break;
                                }
                            }

                            if (allPersons.Count >= queryLimit
                                || userRequest.NextPageRequest == null)
                            {
                                break;
                            }

                            userRequest = await userRequest.NextPageRequest.GetAsync();
                        }

                        rawResults = allPersons;
                    }

                    SearchResults.Clear();
                    var results = rawResults.Where(o => !Selections.Any(s => s.Id == o.Id))
                        .Take(searchLimit);
                    foreach (var item in results)
                    {
                        SearchResults.Add(item);
                    }

                    if (SearchResults.Count > 0)
                    {
                        ShowSearchResults();
                    }
                    else
                    {
                        ClearAndHideSearchResultListBox();
                    }
                }
            }
            catch (Exception exception)
            {
                MessageDialog messageDialog = new MessageDialog(exception.Message);
                await messageDialog.ShowAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void SelectPerson(Person person)
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
            SearchPattern = string.Empty;
        }
    }
}