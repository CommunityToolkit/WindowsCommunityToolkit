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
    /// Properties for <see cref="SizerBase"/>
    /// </summary>
    public partial class SizerBase : Control
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
    }
}