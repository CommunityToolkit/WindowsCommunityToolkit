// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Provides ActiveLayout attached property for <see cref="Grid"/> element.
    /// </summary>
    public static partial class GridExtensions
    {
        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding <see cref="ActiveLayoutProperty"/> to a <see cref="Grid"/>
        /// </summary>
        public static readonly DependencyProperty ActiveLayoutProperty =
            DependencyProperty.RegisterAttached("ActiveLayout", typeof(string), typeof(GridExtensions), new PropertyMetadata(null, OnActiveLayoutChanged));

        /// <summary>
        /// Gets the <see cref="ActiveLayoutProperty"/> associated with the specified <see cref="Grid"/>
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.Grid"/> from which to get the associated <see cref="ActiveLayoutProperty"/> value</param>
        /// <returns>The <see cref="ActiveLayoutProperty"/> value associated with the <see cref="Grid"/> or null</returns>
        public static string GetActiveLayout(Grid obj) => (string)obj.GetValue(ActiveLayoutProperty);

        /// <summary>
        /// Sets the <see cref="ActiveLayoutProperty"/> associated with the specified <see cref="Grid"/>
        /// </summary>
        /// <param name="obj">The <see cref="Grid"/> to associated the <see cref="ActiveLayoutProperty"/> to</param>
        /// <param name="value">The <see cref="ActiveLayoutProperty"/> to bind to the <see cref="Grid"/></param>
        public static void SetActiveLayout(Grid obj, string value) => obj.SetValue(ActiveLayoutProperty, value);

        private static void OnActiveLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid grid)
            {
                UpdateLayout(grid);
            }
        }
    }
}
