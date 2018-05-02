using Microsoft.Graph;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    public partial class PeoplePicker : Control
    {
        //GraphAccessToken
        public static readonly DependencyProperty GraphAccessTokenProperty =
            DependencyProperty.Register(
                nameof(GraphAccessToken), typeof(string),
                typeof(PeoplePicker), new PropertyMetadata("", GraphAccessTokenPropertyChanged)
            );

        // AllowMultiple
        public static readonly DependencyProperty AllowMultipleProperty =
            DependencyProperty.Register(
                nameof(AllowMultiple), typeof(bool),
                typeof(PeoplePicker), null
            );

        // SearchResultLimit
        public static readonly DependencyProperty SearchResultLimitProperty =
            DependencyProperty.Register(
                nameof(SearchResultLimit), typeof(int),
                typeof(PeoplePicker), null
            );

        // PersonNotSelectedMessage
        public static readonly DependencyProperty PersonNotSelectedMessageProperty =
            DependencyProperty.Register(
                nameof(PersonNotSelectedMessage), typeof(string),
                typeof(PeoplePicker), null
            );

        // Selections
        public static readonly DependencyProperty SelectionsProperty =
            DependencyProperty.Register(
                nameof(Selections), typeof(ObservableCollection<Person>),
                typeof(PeoplePicker), null
            );

        // PeopleList
        internal static readonly DependencyProperty SearchResultListProperty =
            DependencyProperty.Register(
                nameof(SearchResultList), typeof(ObservableCollection<Person>),
                typeof(PeoplePicker), null
            );

        public string GraphAccessToken
        {
            get => (string) this.GetValue(GraphAccessTokenProperty);
            set => this.SetValue(GraphAccessTokenProperty, value);
        }

        public bool AllowMultiple
        {
            get => (bool) this.GetValue(AllowMultipleProperty);
            set => this.SetValue(AllowMultipleProperty, value);
        }

        public int SearchResultLimit
        {
            get => (int) this.GetValue(SearchResultLimitProperty);
            set => this.SetValue(SearchResultLimitProperty, value);
        }

        public string PersonNotSelectedMessage
        {
            get => (string) this.GetValue(PersonNotSelectedMessageProperty);
            set => this.SetValue(PersonNotSelectedMessageProperty, value);
        }

        public ObservableCollection<Person> Selections
        {
            get => (ObservableCollection<Person>) this.GetValue(SelectionsProperty);
            set => this.SetValue(SelectionsProperty, value);
        }

        internal ObservableCollection<Person> SearchResultList
        {
            get => (ObservableCollection<Person>) this.GetValue(SearchResultListProperty);
            set => this.SetValue(SearchResultListProperty, value);
        }

        internal GraphServiceClient GraphClient { get; set; }

        internal DelegateCommand DeleteItem => new DelegateCommand(DeleteSelectionItem);
    }
}