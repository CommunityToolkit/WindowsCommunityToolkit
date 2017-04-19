namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
    using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Toolbar for Editing Text attached to a RichEditBox
    /// </summary>
    public partial class TextToolbar
    {
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditorProperty =
            DependencyProperty.Register(nameof(Editor), typeof(int), typeof(TextToolbar), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Formatting.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FormattingProperty =
            DependencyProperty.Register(nameof(Formatting), typeof(Format), typeof(TextToolbar), new PropertyMetadata(Format.MarkDown));

        // Using a DependencyProperty as the backing store for TextFormat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FormatterProperty =
            DependencyProperty.Register(nameof(Formatter), typeof(Formatter), typeof(TextToolbar), new PropertyMetadata(null));

        public RichEditBox Editor
        {
            get { return (RichEditBox)GetValue(EditorProperty); }
            set { SetValue(EditorProperty, value); }
        }

        public Format Formatting
        {
            get { return (Format)GetValue(FormattingProperty); }
            set { SetValue(FormattingProperty, value); }
        }

        public Formatter Formatter
        {
            get { return (Formatter)GetValue(FormatterProperty); }
            set { SetValue(FormatterProperty, value); }
        }

        /// <summary>
        /// Gets the Custom Toolbar buttons for Additional Formatting Functions
        /// </summary>
        public ObservableCollection<CustomToolbarItem> CustomItems { get; } = new ObservableCollection<CustomToolbarItem>();

        private List<DefaultButton> removedButtons = new List<DefaultButton>();

        public enum Format
        {
            /// <summary>
            /// Utilises the Built-In Markdown Fomatter
            /// </summary>
            MarkDown,

            /// <summary>
            /// Utilises the Built-In RichText Fomatter
            /// </summary>
            RichText,

            /// <summary>
            /// Utilises the provided Custom Formatter using the Formatter Property
            /// </summary>
            Custom
        }
    }
}