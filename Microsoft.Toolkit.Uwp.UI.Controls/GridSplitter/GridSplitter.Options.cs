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
                new PropertyMetadata(GridResizeBehavior.BasedOnAlignment, OnResizeBehaviorChange));

        /// <summary>
        /// Identifies the <see cref="VerticalIconText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalIconTextProperty
            = DependencyProperty.Register(
                nameof(ResizeBehavior),
                typeof(string),
                typeof(GridSplitter),
                new PropertyMetadata(GripperBarVertical));

        /// <summary>
        /// Identifies the <see cref="HorizontalIconText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalIconTextProperty
            = DependencyProperty.Register(
                nameof(HorizontalIconText),
                typeof(string),
                typeof(GridSplitter),
                new PropertyMetadata(GripperBarHorizontal));

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
        /// Gets or sets Splitter Row resize direction Icon text (Default GripperBarVertical: E784).
        /// </summary>
        public string VerticalIconText
        {
            get { return (string)GetValue(VerticalIconTextProperty); }

            set { SetValue(VerticalIconTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets Splitter Column resize direction Icon text (Default GripperBarHorizontal: E76F).
        /// </summary>
        public string HorizontalIconText
        {
            get { return (string)GetValue(HorizontalIconTextProperty); }

            set { SetValue(HorizontalIconTextProperty, value); }
        }

        private static void OnResizeDirectionChange(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var gridSplitter = (GridSplitter)o;
            gridSplitter._resizeDirection = gridSplitter.GetResizeDirection();
        }

        private static void OnResizeBehaviorChange(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var gridSplitter = (GridSplitter)o;
            gridSplitter._resizeBehavior = gridSplitter.GetResizeBehavior();
        }
    }
}
