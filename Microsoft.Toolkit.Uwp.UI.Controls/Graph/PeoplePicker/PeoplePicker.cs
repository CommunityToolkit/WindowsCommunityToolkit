using Microsoft.Graph;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    public partial class PeoplePicker : Control
    {
        public PeoplePicker()
        {
            DefaultStyleKey = typeof(PeoplePicker);
        }

        private TextBox _searchBox;
        private ProgressRing _loading;
        private ListBox _searchResultListBox;
        private TextBlock _selectionsCounter;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _searchBox = GetTemplateChild("SearchBox") as TextBox;
            _loading = GetTemplateChild("Loading") as ProgressRing;
            _searchResultListBox = GetTemplateChild("SearchResultListBox") as ListBox;
            _selectionsCounter = GetTemplateChild("SelectionsCounter") as TextBlock;

            SearchResultList = new ObservableCollection<Person>();
            Selections = Selections ?? new ObservableCollection<Person>();
            _selectionsCounter.Text = $"{Selections.Count} selected";
            if (!AllowMultiple)
            {
                _selectionsCounter.Visibility = Visibility.Collapsed;
            }

            _searchBox.TextChanged += SearchBox_OnTextChanged;
            _searchResultListBox.SelectionChanged += SearchResultListBox_OnSelectionChanged;
        }

        private static void GraphAccessTokenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PeoplePicker;
            control?.SignInCurrentUserAsync();
        }

        private async void SignInCurrentUserAsync()
        {
            GraphClient = Common.GetAuthenticatedClient(GraphAccessToken);
            if (GraphClient != null)
            {
                var me = await GraphClient.Me.Request().GetAsync();
            }
        }
    }
}