// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using Microsoft.Graph;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Defines the properties for the <see cref="PeoplePicker"/> control.
    /// </summary>
    public partial class PeoplePicker : Control
    {
        /// <summary>
        /// File is selected
        /// </summary>
        public event EventHandler<PeopleSelectionChangedEventArgs> SelectionChanged;

        /// <summary>
        /// Gets required delegated permissions for the <see cref="PeoplePicker"/> control
        /// </summary>
        public static string[] RequiredDelegatedPermissions
        {
            get
            {
                return new string[] { "User.Read", "User.ReadBasic.All", "People.Read" };
            }
        }

        /// <summary>
        /// Identifies the <see cref="AllowMultiple"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AllowMultipleProperty =
            DependencyProperty.Register(
                nameof(AllowMultiple),
                typeof(bool),
                typeof(PeoplePicker),
                new PropertyMetadata(true, AllowMultiplePropertyChanged));

        /// <summary>
        /// Identifies the <see cref="SearchResultLimit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SearchResultLimitProperty =
            DependencyProperty.Register(
                nameof(SearchResultLimit),
                typeof(int),
                typeof(PeoplePicker),
                null);

        /// <summary>
        /// Identifies the <see cref="PlaceholderText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(
                nameof(PlaceholderText),
                typeof(string),
                typeof(PeoplePicker),
                new PropertyMetadata("Enter keywords to search people"));

        /// <summary>
        /// Identifies the <see cref="Selections"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectionsProperty =
            DependencyProperty.Register(
                nameof(Selections),
                typeof(ObservableCollection<Person>),
                typeof(PeoplePicker),
                null);

        /// <summary>
        /// Identifies the <see cref="SearchResultList"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty SearchResultListProperty =
            DependencyProperty.Register(
                nameof(SearchResultList),
                typeof(ObservableCollection<Person>),
                typeof(PeoplePicker),
                null);

        private static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(
                nameof(IsLoading),
                typeof(bool),
                typeof(PeoplePicker),
                null);

        /// <summary>
        /// Gets or sets a value indicating whether multiple people can be selected
        /// </summary>
        public bool AllowMultiple
        {
            get => (bool)GetValue(AllowMultipleProperty);
            set => SetValue(AllowMultipleProperty, value);
        }

        /// <summary>
        /// Gets or sets the max person returned in the search results
        /// </summary>
        public int SearchResultLimit
        {
            get => (int)GetValue(SearchResultLimitProperty);
            set => SetValue(SearchResultLimitProperty, value);
        }

        /// <summary>
        /// Gets or sets the text to be displayed when no user is selected
        /// </summary>
        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        /// <summary>
        /// Gets or sets the selected person list.
        /// </summary>
        public ObservableCollection<Person> Selections
        {
            get => (ObservableCollection<Person>)GetValue(SelectionsProperty);
            set => SetValue(SelectionsProperty, value);
        }

        internal ObservableCollection<Person> SearchResultList
        {
            get => (ObservableCollection<Person>)GetValue(SearchResultListProperty);
            set => SetValue(SearchResultListProperty, value);
        }

        private bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }
    }
}