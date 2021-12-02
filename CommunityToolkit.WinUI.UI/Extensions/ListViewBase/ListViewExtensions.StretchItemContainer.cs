// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml;

namespace CommunityToolkit.WinUI.UI
{
    /// <summary>
    /// Provides attached dependency properties for the <see cref="Microsoft.UI.Xaml.Controls.ListViewBase"/>
    /// </summary>
    public static partial class ListViewExtensions
    {
        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for setting the container content stretch direction on the <see cref="Microsoft.UI.Xaml.Controls.ListViewBase"/>
        /// </summary>
        public static readonly DependencyProperty ItemContainerStretchDirectionProperty = DependencyProperty.RegisterAttached("ItemContainerStretchDirection", typeof(ItemContainerStretchDirection), typeof(ListViewExtensions), new PropertyMetadata(null, OnItemContainerStretchDirectionPropertyChanged));

                /// <summary>
        /// Gets the stretch <see cref="ItemContainerStretchDirection"/> associated with the specified <see cref="Microsoft.UI.Xaml.Controls.ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="Microsoft.UI.Xaml.Controls.ListViewBase"/> to get the associated <see cref="ItemContainerStretchDirection"/> from</param>
        /// <returns>The <see cref="ItemContainerStretchDirection"/> associated with the <see cref="Microsoft.UI.Xaml.Controls.ListViewBase"/></returns>
        public static ItemContainerStretchDirection GetItemContainerStretchDirection(Microsoft.UI.Xaml.Controls.ListViewBase obj)
        {
            return (ItemContainerStretchDirection)obj.GetValue(ItemContainerStretchDirectionProperty);
        }

        /// <summary>
        /// Sets the stretch <see cref="ItemContainerStretchDirection"/> associated with the specified <see cref="Microsoft.UI.Xaml.Controls.ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="Microsoft.UI.Xaml.Controls.ListViewBase"/> to associate the <see cref="ItemContainerStretchDirection"/> with</param>
        /// <param name="value">The <see cref="ItemContainerStretchDirection"/> for binding to the <see cref="Microsoft.UI.Xaml.Controls.ListViewBase"/></param>
        public static void SetItemContainerStretchDirection(Microsoft.UI.Xaml.Controls.ListViewBase obj, ItemContainerStretchDirection value)
        {
            obj.SetValue(ItemContainerStretchDirectionProperty, value);
        }
    }
}
