using System;
using Microsoft.Toolkit.Uwp.Deferred;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class SuggestionChosenEventArgs : DeferredEventArgs
    {
        public string Query { get; internal set; }

        public string Prefix { get; internal set; }

        public string Text { get; set; }

        public object SelectedItem { get; internal set; }

        public Guid Id { get; internal set; }

        public SuggestionTokenFormat Format { get; internal set; }
    }
}
