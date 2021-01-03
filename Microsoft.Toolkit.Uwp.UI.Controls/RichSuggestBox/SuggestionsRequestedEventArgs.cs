using Microsoft.Toolkit.Uwp.Deferred;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class SuggestionsRequestedEventArgs : DeferredCancelEventArgs
    {
        public string Prefix { get; set; }

        public string Query { get; set; }
    }
}
