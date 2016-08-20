using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// ColumnResizer is a UI control that add the resizing functionality to a Grid Column.
    /// </summary>
    [TemplatePart(Name = RESIZERNAME, Type = typeof(Grid))]
    public partial class ColumnResizer : Control
    {
        private const string RESIZERNAME = "Resizer";
        private const int FixedOffset = 5;
        private const double EnteredOpacity = .7;
        private const double DefaultOpacity = 1;

        private readonly CoreCursor _resizeCursor = new CoreCursor(CoreCursorType.SizeWestEast, 1);
        private readonly CoreCursor _arrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        private Grid _resizer;
        private bool _isResizing;

        /// <summary>
        /// Gets Column Resizer Container Grid
        /// </summary>
        private Grid Resizable => _resizer.FindVisualAscendant<Grid>();

        /// <summary>
        /// Gets the current Column definition of the parent Grid
        /// </summary>
        private ColumnDefinition CurrentColumn
        {
            get
            {
                var columnResizerColumnProperty = Grid.GetColumn(this);
                return Resizable.ColumnDefinitions[columnResizerColumnProperty - 1];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnResizer"/> class.
        /// </summary>
        public ColumnResizer()
        {
            this.DefaultStyleKey = typeof(ColumnResizer);
        }

        /// <summary>
        /// Override default OnApplyTemplate to capture children controls
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _resizer = GetTemplateChild(RESIZERNAME) as Grid;
            if (_resizer == null)
            {
                return;
            }

            // Resizer Events Registration
            _resizer.PointerPressed += Resizer_PointerPressed;
            _resizer.PointerEntered += Resizer_PointerEntered;
            _resizer.PointerExited += Resizer_PointerExited;

            // Parent Grid (Resizable) Events Registration
            Resizable.PointerReleased += Resizable_PointerReleased;
            Resizable.PointerMoved += Resizable_PointerMoved;
        }
    }
}
