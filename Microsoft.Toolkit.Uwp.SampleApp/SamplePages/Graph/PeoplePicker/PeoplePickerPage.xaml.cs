using CommunityToolkit.Authentication;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class PeoplePickerPage : Page
    {
        public PeoplePickerPage()
        {
            this.InitializeComponent();

#pragma warning disable CS0618 // MockProvider is meant for prototyping purposes only.
            ProviderManager.Instance.GlobalProvider = new MockProvider(false);
#pragma warning restore CS0618
        }
    }
}
