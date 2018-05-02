using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    public partial class PeoplePicker : Control
    {
        public PeoplePicker()
        {
            DefaultStyleKey = typeof(PeoplePicker);
            DataContext = this;
        }

        private TextBox SearchBox;
        private ProgressRing Loading;
        private ListBox SearchResultListBox;
        private TextBlock SelectionsCounter;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SearchBox = GetTemplateChild("SearchBox") as TextBox;
            Loading = GetTemplateChild("Loading") as ProgressRing;
            SearchResultListBox = GetTemplateChild("SearchResultListBox") as ListBox;
            SelectionsCounter = GetTemplateChild("SelectionsCounter") as TextBlock;

            SearchResultList = new ObservableCollection<Person>();
            Selections = Selections ?? new ObservableCollection<Person>();
            SelectionsCounter.Text = $"{Selections.Count} selected";
            if (!AllowMultiple)
                SelectionsCounter.Visibility = Visibility.Collapsed;

            SearchBox.PlaceholderText = string.IsNullOrWhiteSpace(PersonNotSelectedMessage)
                ? "Select a person"
                : PersonNotSelectedMessage;

            SearchBox.TextChanged += SearchBox_OnTextChanged;
            SearchResultListBox.SelectionChanged += SearchResultListBox_OnSelectionChanged;
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