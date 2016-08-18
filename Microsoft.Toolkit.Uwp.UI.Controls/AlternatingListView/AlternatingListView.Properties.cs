using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Defines the properties for the <see cref="AlternatingListView"/> control.
    /// </summary>
    public partial class AlternatingListView
    {
        /// <summary>
        /// Defines the <see cref="OddRowBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OddRowBackgroundProperty = DependencyProperty.Register(
            nameof(OddRowBackground),
            typeof(Brush),
            typeof(AlternatingListView),
            new PropertyMetadata(null));

        /// <summary>
        /// Defines the <see cref="EvenRowBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EvenRowBackgroundProperty = DependencyProperty.Register(
            nameof(EvenRowBackground),
            typeof(Brush),
            typeof(AlternatingListView),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the odd row background brush.
        /// </summary>
        public Brush OddRowBackground
        {
            get { return (Brush)GetValue(OddRowBackgroundProperty); }
            set { SetValue(OddRowBackgroundProperty, (Brush)value); }
        }

        /// <summary>
        /// Gets or sets the even row background brush.
        /// </summary>
        public Brush EvenRowBackground
        {
            get { return (Brush)GetValue(EvenRowBackgroundProperty); }
            set { SetValue(EvenRowBackgroundProperty, (Brush)value); }
        }
    }
}
