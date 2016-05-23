using System;
using Windows.UI.Xaml;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    /// <summary>
    /// Defines the properties for the <see cref="PivoramaPanel"/> control.
    /// </summary>
    public partial class PivoramaPanel
    {
        /// <summary>
        /// Defines the <see cref="ItemTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(
                nameof(ItemTemplate),
                typeof(DataTemplate),
                typeof(PivoramaPanel),
                new PropertyMetadata(null, (d, e) => { ((PivoramaPanel)d).InvalidateMeasure(); }));

        /// <summary>
        /// Defines the <see cref="ItemWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(
            nameof(ItemWidth),
            typeof(double),
            typeof(PivoramaPanel),
            new PropertyMetadata(440.0, (d, e) => { ((PivoramaPanel)d).InvalidateMeasure(); }));

        /// <summary>
        /// Defines the event called when the selected index of the <see cref="PivoramaPanel"/> changes.
        /// </summary>
        public event EventHandler<int> SelectedIndexChanged;

        /// <summary>
        /// Gets the maximum number of items within the <see cref="PivoramaPanel"/>.
        /// </summary>
        protected virtual int MaxItems => 16;

        /// <summary>
        /// Gets or sets the item template.
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ItemTemplateProperty);
            }
            set
            {
                SetValue(ItemTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the item width.
        /// </summary>
        public double ItemWidth
        {
            get
            {
                return (double)GetValue(ItemWidthProperty);
            }
            set
            {
                SetValue(ItemWidthProperty, value);
            }
        }
    }
}