// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Base class for splitting/resizing controls
    /// </summary>
    [ContentProperty(Name = nameof(Content))]
    public abstract partial class SizerBase : Control
    {
        /// <summary>
        /// Check for new requested vertical size is valid or not
        /// </summary>
        /// <param name="target">Target control being resized</param>
        /// <param name="verticalChange">The requested vertical change</param>
        /// <param name="parentActualHeight">The parent control's ActualHeight</param>
        /// <returns>Bool result if requested vertical change is valid or not</returns>
        protected static bool IsValidHeight(FrameworkElement target, double verticalChange, double parentActualHeight)
        {
            var newHeight = target.ActualHeight + verticalChange;

            var minHeight = target.MinHeight;
            if (newHeight < 0 || (!double.IsNaN(minHeight) && newHeight < minHeight))
            {
                return false;
            }

            var maxHeight = target.MaxHeight;
            if (!double.IsNaN(maxHeight) && newHeight > maxHeight)
            {
                return false;
            }

            if (newHeight <= parentActualHeight)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check for new requested horizontal size is valid or not
        /// </summary>
        /// <param name="target">Target control being resized</param>
        /// <param name="horizontalChange">The requested horizontal change</param>
        /// <param name="parentActualWidth">The parent control's ActualWidth</param>
        /// <returns>Bool result if requested horizontal change is valid or not</returns>
        protected static bool IsValidWidth(FrameworkElement target, double horizontalChange, double parentActualWidth)
        {
            var newWidth = target.ActualWidth + horizontalChange;

            var minWidth = target.MinWidth;
            if (newWidth < 0 || (!double.IsNaN(minWidth) && newWidth < minWidth))
            {
                return false;
            }

            var maxWidth = target.MaxWidth;
            if (!double.IsNaN(maxWidth) && newWidth > maxWidth)
            {
                return false;
            }

            if (newWidth <= parentActualWidth)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Vertical symbol for GripperBar in Segoe MDL2 Font asset.
        /// </summary>
        protected const string GripperBarVertical = "\xE784";

        /// <summary>
        /// Horizontal symbol for GripperBar in Segoe MDL2 Font asset.
        /// </summary>
        protected const string GripperBarHorizontal = "\xE76F";

        /// <summary>
        /// Distance (horizontal or vertical) to move, in response to keyboard activity.
        /// </summary>
        protected const double GripperKeyboardChange = 8.0d;

        /// <summary>
        /// Gets or sets the content of the splitter control.
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
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(SizerBase), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the content template for the <see cref="Content"/>.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ContentTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(nameof(ContentTemplate), typeof(DataTemplate), typeof(SizerBase), new PropertyMetadata(null));

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
            DependencyProperty.Register(nameof(GripperCursor), typeof(CoreCursorType), typeof(SizerBase), new PropertyMetadata(CoreCursorType.SizeWestEast));

        /// <summary>
        /// Gets or sets the orientation the sizer will be and how it will interact with other elements. Defaults to <see cref="Orientation.Vertical"/>.
        /// </summary>
        /// <remarks>
        /// Note if using <see cref="GridSplitter"/>, use the <see cref="GridSplitter.ResizeDirection"/> property instead.
        /// </remarks>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(SizerBase), new PropertyMetadata(Orientation.Vertical));

        //// TODO: Check if this is ContentSizer only property

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SizerBase"/> control is resizing in the opposite direction.
        /// </summary>
        public bool IsDragInverted
        {
            get { return (bool)GetValue(IsDragInvertedProperty); }
            set { SetValue(IsDragInvertedProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsDragInverted"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDragInvertedProperty =
            DependencyProperty.Register(nameof(IsDragInverted), typeof(bool), typeof(SizerBase), new PropertyMetadata(false));

        /// <summary>
        /// Method to process the requested horizontal resizing.
        /// </summary>
        /// <param name="horizontalChange">The requested horizontal change</param>
        /// <returns><see cref="bool"/> indicates if the change was made</returns>
        protected abstract bool OnHorizontalMove(double horizontalChange);

        /// <summary>
        /// Method to process the requested vertical resizing.
        /// </summary>
        /// <param name="verticalChange">The requested vertical change</param>
        /// <returns><see cref="bool"/> indicates if the change was made</returns>
        protected abstract bool OnVerticalMove(double verticalChange);

        /// <summary>
        /// Called when the control has been initialized.
        /// </summary>
        /// <param name="e">Loaded event args.</param>
        protected abstract void OnLoaded(RoutedEventArgs e);

        /// <summary>
        /// Initializes a new instance of the <see cref="SizerBase"/> class.
        /// </summary>
        public SizerBase()
        {
            this.DefaultStyleKey = typeof(SizerBase);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Unregister Events
            Loaded -= SizerBase_Loaded;
            PointerEntered -= GridSplitter_PointerEntered;
            PointerExited -= GridSplitter_PointerExited;
            PointerPressed -= GridSplitter_PointerPressed;
            PointerReleased -= GridSplitter_PointerReleased;
            ManipulationStarted -= GridSplitter_ManipulationStarted;
            ManipulationCompleted -= GridSplitter_ManipulationCompleted;

            // Register Events
            Loaded += SizerBase_Loaded;
            PointerEntered += GridSplitter_PointerEntered;
            PointerExited += GridSplitter_PointerExited;
            PointerPressed += GridSplitter_PointerPressed;
            PointerReleased += GridSplitter_PointerReleased;
            ManipulationStarted += GridSplitter_ManipulationStarted;
            ManipulationCompleted += GridSplitter_ManipulationCompleted;
        }

        private void SizerBase_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= SizerBase_Loaded;

            OnLoaded(e);
        }
    }
}