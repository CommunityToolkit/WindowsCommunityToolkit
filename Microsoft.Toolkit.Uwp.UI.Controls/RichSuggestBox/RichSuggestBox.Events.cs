using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public partial class RichSuggestBox
    {
        public event TypedEventHandler<RichSuggestBox, SuggestionsRequestedEventArgs> SuggestionsRequested;

        public event TypedEventHandler<RichSuggestBox, SuggestionChosenEventArgs> SuggestionChosen;

        public event TypedEventHandler<RichEditBox, RoutedEventArgs> TextChanged;
    }
}
