using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents the control that redistributes space between columns or rows of a Grid control.
    /// </summary>
    [TemplatePart(Name = SPLITTERRNAME, Type = typeof(Thumb))]
    [TemplatePart(Name = ICONDISPLAYNAME, Type = typeof(Thumb))]
    public partial class GridSplitter : Control
    {
        private const string SPLITTERRNAME = "Splitter";
        private const string ICONDISPLAYNAME = "IconDisplay";
        private const double Epsilon = 0.00000153;

        // Symbol GripperBarVertical in Segoe MDL2 Assets
        private const string GripperBarVertical = "\xE784";

        // Symbol GripperBarHorizontal in Segoe MDL2 Assets
        private const string GripperBarHorizontal = "\xE76F";

        private static readonly CoreCursor ColumnsSplitterCursor = new CoreCursor(CoreCursorType.SizeWestEast, 1);
        private static readonly CoreCursor RowSplitterCursor = new CoreCursor(CoreCursorType.SizeNorthSouth, 1);
        private static readonly CoreCursor ArrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        private static DisplayInformation _dpi;
        private Thumb _splitter;
        private TextBlock _iconDisplay;

        // Data used for holding resizing options
        private ResizeData _resizeData;

        /// <summary>
        /// Gets GridSplitter Container Grid
        /// </summary>
        private Grid Resizable => Parent as Grid;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridSplitter"/> class.
        /// </summary>
        public GridSplitter()
        {
            DefaultStyleKey = typeof(GridSplitter);
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_splitter != null)
            {
                // Unhook registered events
                _splitter.DragStarted -= Splitter_DragStarted;
                _splitter.DragDelta -= Splitter_DragDelta;
                _splitter.DragCompleted -= Splitter_DragCompleted;
            }

            _splitter = GetTemplateChild(SPLITTERRNAME) as Thumb;
            _iconDisplay = GetTemplateChild(ICONDISPLAYNAME) as TextBlock;
            if (_splitter == null)
            {
                return;
            }

            InitializeData();
            UpdateDisplayIcon();

            // Register Events
            _splitter.DragStarted += Splitter_DragStarted;
            _splitter.DragDelta += Splitter_DragDelta;
            _splitter.DragCompleted += Splitter_DragCompleted;
        }
    }
}
