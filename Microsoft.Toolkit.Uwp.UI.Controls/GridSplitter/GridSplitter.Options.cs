using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents the control that redistributes space between columns or rows of a Grid control.
    /// </summary>
    public partial class GridSplitter
    {
        /// <summary>
        /// Identifies the <see cref="ResizeDirection"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ResizeDirectionProperty
            = DependencyProperty.Register(
                                            nameof(ResizeDirection),
                                            typeof(GridResizeDirection),
                                            typeof(GridSplitter),
                                            new PropertyMetadata(GridResizeDirection.Auto, OnResizeDirectionChange));

        /// <summary>
        /// Identifies the <see cref="ResizeBehavior"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ResizeBehaviorProperty
            = DependencyProperty.Register(
                                            nameof(ResizeBehavior),
                                            typeof(GridResizeBehavior),
                                            typeof(GridSplitter),
                                            new PropertyMetadata(GridResizeBehavior.BasedOnAlignment));

        /// <summary>
        /// Identifies the <see cref="DragIncrement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DragIncrementProperty =
                    DependencyProperty.Register(
                                nameof(DragIncrement),
                                typeof(double),
                                typeof(GridSplitter),
                                new PropertyMetadata(1.0));

        /// <summary>
        /// Gets or sets whether the Splitter resizes the Columns, Rows, or Both.
        /// </summary>
        public GridResizeDirection ResizeDirection
        {
            get { return (GridResizeDirection)GetValue(ResizeDirectionProperty); }

            set { SetValue(ResizeDirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets which Columns or Rows the Splitter resizes.
        /// </summary>
        public GridResizeBehavior ResizeBehavior
        {
            get { return (GridResizeBehavior)GetValue(ResizeBehaviorProperty); }

            set { SetValue(ResizeBehaviorProperty, value); }
        }

        /// <summary>
        /// Gets or sets units to Restricts splitter to move a multiple of the specified units.
        /// </summary>
        public double DragIncrement
        {
            get { return (double)GetValue(DragIncrementProperty); }
            set { SetValue(DragIncrementProperty, value); }
        }

        private static void OnResizeDirectionChange(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var gridSplitter = (GridSplitter)o;
            gridSplitter.InitializeData();
            gridSplitter.UpdateDisplayIcon();
        }
    }
}
