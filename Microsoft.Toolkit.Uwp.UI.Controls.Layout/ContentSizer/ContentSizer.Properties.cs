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
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Properties of the <see cref="ContentSizer"/> control.
    /// </summary>
    public partial class ContentSizer
    {
        /// <summary>
        /// Gets or sets the content template for the <see cref="Content"/>. By default is a TextBlock.
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
            DependencyProperty.Register(nameof(ContentTemplate), typeof(DataTemplate), typeof(ContentSizer), new PropertyMetadata(null));

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
            DependencyProperty.Register(nameof(GripperForeground), typeof(Brush), typeof(ContentSizer), new PropertyMetadata(default(Brush)));

    }
}
