// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Collections.ObjectModel;
using Microsoft.Graph;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The PeoplePicker Control is a simple control that allows for selection of one or more users from an organizational AD.
    /// </summary>
    [TemplatePart(Name = SearchBoxPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = SearchResultListBoxPartName, Type = typeof(ListBox))]
    [TemplatePart(Name = SelectionsListBoxPartName, Type = typeof(ListBox))]
    public partial class PeoplePicker : Control
    {
        private const string SearchBoxPartName = "SearchBox";
        private const string SearchResultListBoxPartName = "SearchResultListBox";
        private const string SelectionsListBoxPartName = "SelectionsListBox";

        private TextBox _searchBox;
        private ListBox _searchResultListBox;
        private ListBox _selectionsListBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeoplePicker"/> class.
        /// </summary>
        public PeoplePicker()
        {
            DefaultStyleKey = typeof(PeoplePicker);
        }

        /// <summary>
        /// Called when applying the control template.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            IsLoading = false;

            _searchBox = GetTemplateChild(SearchBoxPartName) as TextBox;
            _searchResultListBox = GetTemplateChild(SearchResultListBoxPartName) as ListBox;
            _selectionsListBox = GetTemplateChild(SelectionsListBoxPartName) as ListBox;

            SearchResultList = new ObservableCollection<Person>();
            Selections = Selections ?? new ObservableCollection<Person>();
            if (_searchBox != null)
            {
                _searchBox.TextChanged += SearchBox_OnTextChanged;
            }
            if (_searchResultListBox != null)
            {
                _searchResultListBox.SelectionChanged += SearchResultListBox_OnSelectionChanged;
            }
            if (_selectionsListBox != null)
            {
                _selectionsListBox.Tapped += SelectionsListBox_Tapped;
            }
            base.OnApplyTemplate();
        }
    }
}