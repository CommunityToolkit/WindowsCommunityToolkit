using Microsoft.Windows.Toolkit.UI.Controls.Primitives;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    public partial class VariableSizedGrid
    {
        /// <summary>
        /// Gets or sets the margin for each item.
        /// </summary>
        /// <value>The item margin.</value>
        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ItemMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(nameof(ItemMargin), typeof(Thickness), typeof(VariableSizedGrid), new PropertyMetadata(new Thickness(2)));
     

        /// <summary>
        /// Gets or sets the padding applied to each item.
        /// </summary>
        /// <value>The item padding.</value>
        public Thickness ItemPadding
        {
            get { return (Thickness)GetValue(ItemPaddingProperty); }
            set { SetValue(ItemPaddingProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ItemPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.Register(nameof(ItemPadding), typeof(Thickness), typeof(VariableSizedGrid), new PropertyMetadata(new Thickness(2)));
     
        
        /// <summary>
        /// Gets or sets the maximum number of rows or columns.
        /// </summary>
        /// <value>The maximum rows or columns.</value>
        public int MaximumRowsOrColumns
        {
            get { return (int)GetValue(MaximumRowsOrColumnsProperty); }
            set { SetValue(MaximumRowsOrColumnsProperty, value); }
        }

        private static void MaximumRowsOrColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGrid;
            control.InvalidateMeasure();
        }

        /// <summary>
        /// Identifies the <see cref="MaximumRowsOrColumns"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumRowsOrColumnsProperty = DependencyProperty.Register(nameof(MaximumRowsOrColumns), typeof(int), typeof(VariableSizedGrid), new PropertyMetadata(4, MaximumRowsOrColumnsChanged));
        
        
        /// <summary>
        /// Gets or sets the height-to-width aspect ratio for each tile.
        /// </summary>
        /// <value>The aspect ratio.</value>
        public double AspectRatio
        {
            get { return (double)GetValue(AspectRatioProperty); }
            set { SetValue(AspectRatioProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="AspectRatio"/> dependency property.
        /// </summary>
        private static void AspectRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGrid;
            control.InvalidateMeasure();
        }

        /// <summary>
        /// Identifies the <see cref="AspectRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register(nameof(AspectRatio), typeof(double), typeof(VariableSizedGrid), new PropertyMetadata(1.0, AspectRatioChanged));
        
        
        /// <summary>
        /// Gets or sets the dimension by which child elements are stacked.
        /// </summary>
        /// <value>One of the enumeration values that specifies the orientation of child elements.</value>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        private static void OrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGrid;
            control.SetOrientation((Orientation)e.NewValue);
        }

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(VariableSizedGrid), new PropertyMetadata(Orientation.Horizontal, OrientationChanged));
    }
}
