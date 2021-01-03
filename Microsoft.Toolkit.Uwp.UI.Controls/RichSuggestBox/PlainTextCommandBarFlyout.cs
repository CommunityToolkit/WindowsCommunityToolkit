using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class PlainTextCommandBarFlyout : TextCommandBarFlyout
    {
        public PlainTextCommandBarFlyout()
        {
            Opening += (sender, o) =>
            {
                PrimaryCommands.Clear();

                // TODO: Limit Pasting to plain-text only
            };
        }
    }
}
