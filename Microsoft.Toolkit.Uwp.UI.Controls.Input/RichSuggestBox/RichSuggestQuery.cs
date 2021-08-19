using Windows.UI.Text;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A structure for <see cref="RichSuggestBox"/> to keep track of the current query internally.
    /// </summary>
    internal class RichSuggestQuery
    {
        public string Prefix { get; set; }

        public string QueryText { get; set; }

        public ITextRange Range { get; set; }
    }
}
