// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Properties for <see cref="PropertySizer"/>.
    /// </summary>
    public partial class PropertySizer
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="PropertySizer"/> control is resizing in the opposite direction.
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
            DependencyProperty.Register(nameof(IsDragInverted), typeof(bool), typeof(PropertySizer), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the binding to a <c>double</c> value that the <see cref="PropertySizer"/> is manipulating.
        /// </summary>
        /// <example>
        /// &lt;controls:PropertySizer Binding="{Binding OpenPaneLength, ElementName=ViewPanel, Mode=TwoWay}"&gt;
        /// </example>
        public double Binding
        {
            get { return (double)GetValue(BindingProperty); }
            set { SetValue(BindingProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Binding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BindingProperty =
            DependencyProperty.Register(nameof(Binding), typeof(double), typeof(PropertySizer), new PropertyMetadata(null));
    }
}
