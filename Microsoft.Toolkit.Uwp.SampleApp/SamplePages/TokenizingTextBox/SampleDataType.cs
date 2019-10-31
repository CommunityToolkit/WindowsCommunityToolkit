using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// Sample of strongly-typed data for <see cref="TokenizingTextBox"/>.
    /// </summary>
    public class SampleDataType
    {
        /// <summary>
        /// Gets or sets symbol to display.
        /// </summary>
        public Symbol Icon { get; set; }

        /// <summary>
        /// Gets or sets text to display.
        /// </summary>
        public string Text { get; set; }

        public override string ToString()
        {
            return "Sample Data: " + Text;
        }
    }
}
