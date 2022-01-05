// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
    /// <summary>
    /// Base class for GridSplitter and ContentSizer
    /// </summary>
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