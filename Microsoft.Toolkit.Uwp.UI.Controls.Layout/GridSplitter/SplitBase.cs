using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public partial class SplitBase : Control
    {
        /// <summary>
        /// Gets or sets the content of the sizer, by default is the grip symbol.
        /// </summary>
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Content"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(SplitBase), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the cursor to use when hovering over the sizer.
        /// </summary>
        public CoreCursorType GripperCursor
        {
            get { return (CoreCursorType)GetValue(GripperCursorProperty); }
            set { SetValue(GripperCursorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="GripperCursor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GripperCursorProperty =
            DependencyProperty.Register(nameof(GripperCursor), typeof(CoreCursorType), typeof(SplitBase), new PropertyMetadata(CoreCursorType.SizeWestEast));

        /// <summary>
        /// Gets or sets the direction that the sizer will interact with.
        /// </summary>
        public ContentResizeDirection ResizeDirection
        {
            get { return (ContentResizeDirection)GetValue(ResizeDirectionProperty); }
            set { SetValue(ResizeDirectionProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ResizeDirection"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ResizeDirectionProperty =
            DependencyProperty.Register(nameof(ResizeDirection), typeof(ContentResizeDirection), typeof(SplitBase), new PropertyMetadata(ContentResizeDirection.Vertical));

        /// <summary>
        /// Gets or sets the control that the <see cref="ContentSizer"/> is resizing. Be default, this will be the visual ancestor of the <see cref="ContentSizer"/>.
        /// </summary>
        public FrameworkElement TargetControl
        {
            get { return (FrameworkElement)GetValue(TargetControlProperty); }
            set { SetValue(TargetControlProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TargetControl"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TargetControlProperty =
            DependencyProperty.Register(nameof(TargetControl), typeof(FrameworkElement), typeof(SplitBase), new PropertyMetadata(null, OnTargetControlChanged));

        private static void OnTargetControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Check if our width can be manipulated
            if (d is ContentSizer sizer && e.NewValue is FrameworkElement element)
            {
                // TODO: For Auto we might want to do detection logic (TBD) here first?
                if (sizer.ResizeDirection != ContentResizeDirection.Horizontal && double.IsNaN(element.Width))
                {
                    element.Width = element.DesiredSize.Width;
                }

                if (sizer.ResizeDirection != ContentResizeDirection.Vertical && double.IsNaN(element.Height))
                {
                    element.Height = element.DesiredSize.Height;
                }
            }
        }

        /// Identifies the <see cref="ResizeBehavior"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ResizeBehaviorProperty
            = DependencyProperty.Register(
                nameof(ResizeBehavior),
                typeof(GridResizeBehavior),
                typeof(GridSplitter),
                new PropertyMetadata(GridResizeBehavior.BasedOnAlignment));

        /// <summary>
        /// Gets or sets which Columns or Rows the Splitter resizes.
        /// </summary>
        public GridResizeBehavior ResizeBehavior
        {
            get { return (GridResizeBehavior)GetValue(ResizeBehaviorProperty); }
            set { SetValue(ResizeBehaviorProperty, value); }
        }
    }

    /// <summary>
    /// Enum to indicate what Columns or Rows the GridSplitter resizes
    /// </summary>
    public enum GridResizeBehavior
    {
        /// <summary>
        /// Determine which columns or rows to resize based on its Alignment.
        /// </summary>
        BasedOnAlignment,

        /// <summary>
        /// Resize the current and next Columns or Rows.
        /// </summary>
        CurrentAndNext,

        /// <summary>
        /// Resize the previous and current Columns or Rows.
        /// </summary>
        PreviousAndCurrent,

        /// <summary>
        /// Resize the previous and next Columns or Rows.
        /// </summary>
        PreviousAndNext
    }
}