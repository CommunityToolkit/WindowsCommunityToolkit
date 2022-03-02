// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
                new PropertyMetadata(GridResizeDirection.Auto, OnResizeDirectionPropertyChanged));

        private static void OnResizeDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GridSplitter splitter && e.NewValue is GridResizeDirection direction &&
                direction != GridResizeDirection.Auto)
            {
                // Update base classes property based on specific polyfill for GridSplitter
                splitter.Orientation =
                    direction == GridResizeDirection.Rows ?
                        Orientation.Horizontal :
                        Orientation.Vertical;
            }
        }

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
        /// Identifies the <see cref="ParentLevel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentLevelProperty
            = DependencyProperty.Register(
                nameof(ParentLevel),
                typeof(int),
                typeof(GridSplitter),
                new PropertyMetadata(default(int)));

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
        /// Gets or sets the level of the parent grid to resize
        /// </summary>
        public int ParentLevel
        {
            get { return (int)GetValue(ParentLevelProperty); }
            set { SetValue(ParentLevelProperty, value); }
        }
    }
}