using Windows.UI.Xaml;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    /// <summary>
    /// Defines the properties for the <see cref="PivoramaTabs"/> panel.
    /// </summary>
    public partial class PivoramaTabs
    {
        /// <summary>
        /// Defines the <see cref="SelectedItemTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemTemplateProperty =
            DependencyProperty.Register(
                nameof(SelectedItemTemplate),
                typeof(DataTemplate),
                typeof(PivoramaTabs),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the previous tab width.
        /// </summary>
        public double PrevTabWidth { get; private set; }

        /// <summary>
        /// Gets the selected tab width.
        /// </summary>
        public double SelectedTabWidth { get; private set; }

        /// <summary>
        /// Gets the maximum number of items within the <see cref="PivoramaTabs"/>.
        /// </summary>
        protected override int MaxItems => 24;

        /// <summary>
        /// Gets or sets the selected item's data template.
        /// </summary>
        public DataTemplate SelectedItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(SelectedItemTemplateProperty);
            }
            set
            {
                SetValue(SelectedItemTemplateProperty, value);
            }
        }
    }
}