// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using Microsoft.Graph;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The PeoplePicker Control is a simple control that allows for selection of one or more users from an organizational AD.
    /// </summary>
    [TemplatePart(Name = SearchBoxPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = SearchResultListBoxPartName, Type = typeof(ListBox))]
    [TemplatePart(Name = SelectionsListBoxPartName, Type = typeof(ListBox))]
    [TemplatePart(Name = SearchResultPopupName, Type = typeof(Popup))]
    public partial class PeoplePicker : Control
    {
        private const string SearchBoxPartName = "SearchBox";
        private const string SearchResultListBoxPartName = "SearchResultListBox";
        private const string SearchResultPopupName = "SearchResultPopup";
        private const string SelectionsListBoxPartName = "SelectionsListBox";

        private TextBox _searchBox;
        private ListBox _searchResultListBox;
        private ListBox _selectionsListBox;
        private Popup _searchResultPopup;
        private int _lastSearchResultCount;
        private double _lastBaseY;
        private double _lastHeight;

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
            if (_searchBox != null)
            {
                _searchBox.TextChanged -= SearchBox_OnTextChanged;
                _searchBox.SizeChanged -= SearchBox_OnSizeChanged;
                _searchBox.KeyUp -= SearchBox_OnKeyUp;
            }

            if (_searchResultListBox != null)
            {
                _searchResultListBox.SelectionChanged -= SearchResultListBox_OnSelectionChanged;
                _searchResultListBox.LayoutUpdated -= SearchResultListBox_OnLayoutUpdated;
            }

            if (_selectionsListBox != null)
            {
                _selectionsListBox.Tapped -= SelectionsListBox_Tapped;
            }

            _searchBox = GetTemplateChild(SearchBoxPartName) as TextBox;
            _searchResultListBox = GetTemplateChild(SearchResultListBoxPartName) as ListBox;
            _selectionsListBox = GetTemplateChild(SelectionsListBoxPartName) as ListBox;
            _searchResultPopup = GetTemplateChild(SearchResultPopupName) as Popup;

            if (_searchBox != null)
            {
                _searchBox.TextChanged += SearchBox_OnTextChanged;
                _searchBox.SizeChanged += SearchBox_OnSizeChanged;
                _searchBox.KeyUp += SearchBox_OnKeyUp;
            }

            if (_searchResultListBox != null)
            {
                _searchResultListBox.SelectionChanged += SearchResultListBox_OnSelectionChanged;
                _searchResultListBox.LayoutUpdated += SearchResultListBox_OnLayoutUpdated;
            }

            if (_selectionsListBox != null)
            {
                _selectionsListBox.Tapped += SelectionsListBox_Tapped;
            }

            base.OnApplyTemplate();
        }
    }
}