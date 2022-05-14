// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI
{
    using LayoutDictionary = System.Collections.Generic.IDictionary<string, Microsoft.Toolkit.Uwp.UI.GridLayoutDefinition>;

    /// <summary>
    /// Provides Layouts attached property for <see cref="Grid"/> element.
    /// </summary>
    public static partial class GridExtensions
    {
        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding <see cref="LayoutsProperty"/> to a <see cref="Grid"/>
        /// </summary>
        public static readonly DependencyProperty LayoutsProperty =
            DependencyProperty.RegisterAttached("Layouts", typeof(LayoutDictionary), typeof(GridExtensions), new PropertyMetadata(null));

        /// <summary>
        /// Gets the <see cref="LayoutsProperty"/> associated with the specified <see cref="Grid"/>
        /// </summary>
        /// <param name="obj">The <see cref="Grid"/> from which to get the associated <see cref="LayoutsProperty"/> value</param>
        /// <returns>The <see cref="LayoutsProperty"/> value associated with the <see cref="Grid"/> or null</returns>
        public static LayoutDictionary GetLayouts(Grid obj)
        {
            var dictionary = (LayoutDictionary)obj.GetValue(LayoutsProperty);
            if (dictionary is null)
            {
                dictionary = new Dictionary<string, GridLayoutDefinition>();
                SetLayouts(obj, dictionary);
            }

            return dictionary;
        }

        /// <summary>
        /// Sets the <see cref="LayoutsProperty"/> associated with the specified <see cref="Grid"/>
        /// </summary>
        /// <param name="obj">The <see cref="Grid"/> to associated the <see cref="LayoutsProperty"/> to</param>
        /// <param name="value">The <see cref="LayoutsProperty"/> to bind to the <see cref="Grid"/></param>
        public static void SetLayouts(Grid obj, LayoutDictionary value) => obj.SetValue(LayoutsProperty, value);
    }
}
