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
    [TemplatePart(Name = GraphAccessTokenPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = SearchBoxPartName, Type = typeof(TextBox))]
    [TemplatePart(Name = LoadingPartName, Type = typeof(ProgressRing))]
    [TemplatePart(Name = SearchResultListBoxPartName, Type = typeof(ListBox))]
    [TemplatePart(Name = SelectionsListBoxPartName, Type = typeof(ListBox))]
    [TemplatePart(Name = SelectionsCounterPartName, Type = typeof(TextBlock))]
    public partial class PeoplePicker : Control
    {
        private const string GraphAccessTokenPartName = "tbGraphAccessToken";
        private const string SearchBoxPartName = "SearchBox";
        private const string LoadingPartName = "Loading";
        private const string SearchResultListBoxPartName = "SearchResultListBox";
        private const string SelectionsListBoxPartName = "SelectionsListBox";
        private const string SelectionsCounterPartName = "SelectionsCounter";

        private TextBox _searchBox;
        private ProgressRing _loading;
        private ListBox _searchResultListBox;
        private ListBox _selectionsListBox;
        private TextBlock _selectionsCounter;

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
            _searchBox = GetTemplateChild("SearchBox") as TextBox;
            _loading = GetTemplateChild("Loading") as ProgressRing;
            _searchResultListBox = GetTemplateChild("SearchResultListBox") as ListBox;
            _selectionsListBox = GetTemplateChild("SelectionsListBox") as ListBox;
            _selectionsCounter = GetTemplateChild("SelectionsCounter") as TextBlock;

            if (_searchBox != null
                && _loading != null
                && _searchResultListBox != null
                && _selectionsListBox != null
                && _selectionsCounter != null)
            {
                SearchResultList = new ObservableCollection<Person>();
                Selections = Selections ?? new ObservableCollection<Person>();
                if (!this.AllowMultiple)
                {
                    _selectionsCounter.Visibility = Visibility.Collapsed;
                }

                _searchBox.TextChanged += SearchBox_OnTextChanged;
                _searchResultListBox.SelectionChanged += SearchResultListBox_OnSelectionChanged;

                _selectionsListBox.Tapped += SelectionsListBox_Tapped;
            }

            base.OnApplyTemplate();
        }
    }
}