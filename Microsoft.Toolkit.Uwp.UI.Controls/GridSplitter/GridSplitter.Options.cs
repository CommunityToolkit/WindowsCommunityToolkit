using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents the control that redistributes space between columns or rows of a Grid control.
    /// </summary>
    public partial class GridSplitter
    {
        public static readonly DependencyProperty ResizeDirectionProperty
            = DependencyProperty.Register(
                                            nameof(ResizeDirection),
                                            typeof(GridResizeDirection),
                                            typeof(GridSplitter),
                                            new PropertyMetadata(GridResizeDirection.Auto, ResizeDirectionOnChange));

        private static void ResizeDirectionOnChange(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var gridSplitter = (GridSplitter)o;
            gridSplitter._resizeDirection = gridSplitter.GetEffectiveResizeDirection();
        }

        private GridResizeDirection GetEffectiveResizeDirection()
        {
            GridResizeDirection direction = ResizeDirection;

            if (direction == GridResizeDirection.Auto)
            {
                // When HorizontalAlignment is Left, Right or Center, resize Columns
                if (HorizontalAlignment != HorizontalAlignment.Stretch)
                {
                    direction = GridResizeDirection.Columns;
                }
                else if (VerticalAlignment != VerticalAlignment.Stretch)
                {
                    direction = GridResizeDirection.Rows;
                }

                // Fall back to Width vs Height
                else if (ActualWidth <= ActualHeight)
                {
                    direction = GridResizeDirection.Columns;
                }
                else
                {
                    direction = GridResizeDirection.Rows;
                }

            }

            return direction;
        }

        private GridResizeDirection _resizeDirection;

        /// <summary>
        /// Gets or sets whether the Splitter resizes the Columns, Rows, or Both.
        /// </summary>
        public GridResizeDirection ResizeDirection
        {
            get { return (GridResizeDirection)GetValue(ResizeDirectionProperty); }

            set { SetValue(ResizeDirectionProperty, value); }
        }
    }

    /// <summary>
    /// Enum to indicate whether GridSplitter resizes Columns or Rows
    /// </summary>
    public enum GridResizeDirection
    {
        /// <summary>
        /// Determines whether to resize rows or columns based on its Alignment and
        /// width compared to height
        /// </summary>
        Auto,

        /// <summary>
        /// Resize columns when dragging Splitter.
        /// </summary>
        Columns,

        /// <summary>
        /// Resize rows when dragging Splitter.
        /// </summary>
        Rows,

        // NOTE: if you add or remove any values in this enum, be sure to update GridSplitter.IsValidResizeDirection()
    }
}
