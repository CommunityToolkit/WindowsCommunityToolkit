// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Base class for splitting/resizing controls
    /// </summary>
    public partial class SplitBase : Control
    {
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
        /// Font family used for gripper.
        /// </summary>
        protected const string GripperDisplayFont = "Segoe MDL2 Assets";

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
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(SplitBase), new PropertyMetadata(null));

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
            DependencyProperty.Register(nameof(ContentTemplate), typeof(DataTemplate), typeof(SplitBase), new PropertyMetadata(null));

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
        /// Gets or sets the foreground color of sizer grip.
        /// </summary>
        public Brush GripperForeground
        {
            get { return (Brush)GetValue(GripperForegroundProperty); }
            set { SetValue(GripperForegroundProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="GripperForeground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GripperForegroundProperty =
            DependencyProperty.Register(nameof(GripperForeground), typeof(Brush), typeof(SplitBase), new PropertyMetadata(default(Brush)));

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
        /// Gets or sets the control that the <see cref="SplitBase"/> is resizing. Be default, this will be the visual ancestor of the <see cref="SplitBase"/>.
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
            if (d is SplitBase splitterBase && e.NewValue is FrameworkElement element)
            {
                // TODO: For Auto we might want to do detection logic (TBD) here first?
                if (splitterBase.ResizeDirection != ContentResizeDirection.Horizontal && double.IsNaN(element.Width))
                {
                    element.Width = element.DesiredSize.Width;
                }

                if (splitterBase.ResizeDirection != ContentResizeDirection.Vertical && double.IsNaN(element.Height))
                {
                    element.Height = element.DesiredSize.Height;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SplitBase"/> control is resizing in the opposite direction.
        /// </summary>
        public bool InvertDragDirection
        {
            get { return (bool)GetValue(InvertDragDirectionProperty); }
            set { SetValue(InvertDragDirectionProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="InvertDragDirection"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InvertDragDirectionProperty =
            DependencyProperty.Register(nameof(InvertDragDirection), typeof(bool), typeof(SplitBase), new PropertyMetadata(false));

        /// <summary>
        /// Check for new requested vertical size is valid or not
        /// </summary>
        /// <param name="target">Target control being resized</param>
        /// <param name="verticalChange">The requested vertical change</param>
        /// <returns>Bool result if requested vertical change is valid or not</returns>
        protected bool IsValidHeight(FrameworkElement target, double verticalChange)
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

            if (newHeight <= ActualHeight)
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
        /// <returns>Bool result if requested horizontal change is valid or not</returns>
        protected bool IsValidWidth(FrameworkElement target, double horizontalChange)
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

            if (newWidth <= ActualWidth)
            {
                return false;
            }

            return true;
        }
    }
}