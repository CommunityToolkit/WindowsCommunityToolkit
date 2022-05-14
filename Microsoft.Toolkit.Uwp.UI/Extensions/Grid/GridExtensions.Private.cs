// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Private methods on GridExtensions to enable dynamic layout switching capability.
    /// </summary>
    public partial class GridExtensions
    {
        private static readonly DependencyProperty LoadedCallbackRegisteredProperty =
            DependencyProperty.RegisterAttached("LoadedCallbackRegistered", typeof(bool), typeof(GridExtensions), new PropertyMetadata(false));

        private static void UpdateLayout(Grid grid)
        {
            if (grid.IsLoaded)
            {
                if (GetLayouts(grid).TryGetValue(GetActiveLayout(grid), out var layout))
                {
                    layout.Apply(grid);
                }
            }
            else if (!(bool)grid.GetValue(LoadedCallbackRegisteredProperty))
            {
                grid.SetValue(LoadedCallbackRegisteredProperty, true);
                grid.Loaded += OnGridLoaded;
            }
        }

        private static void OnGridLoaded(object sender, RoutedEventArgs args)
        {
            var grid = (Grid)sender;
            grid.Loaded -= OnGridLoaded;
            UpdateLayout(grid);
        }
    }
}
