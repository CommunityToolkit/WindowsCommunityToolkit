// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Defines the events for the <see cref="PowerBiEmbedded"/> control.
    /// </summary>
    public partial class PowerBiEmbedded : Control
    {
        private static void OnClientIdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var powerBiEmbeddedControl = d as PowerBiEmbedded;
            powerBiEmbeddedControl.LoadAll();
        }

        private static void OnGroupIdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var powerBiEmbeddedControl = d as PowerBiEmbedded;
            powerBiEmbeddedControl.LoadGroup();
        }

        private static void OnEmbedUrlPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var powerBiEmbeddedControl = d as PowerBiEmbedded;
            powerBiEmbeddedControl.LoadReport();
        }

        private static void OnSelectionReportPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var powerBiEmbeddedControl = d as PowerBiEmbedded;
            if (powerBiEmbeddedControl.SelectionReport != null)
            {
                powerBiEmbeddedControl.LoadReport(
                    powerBiEmbeddedControl.SelectionReport.Id,
                    powerBiEmbeddedControl.SelectionReport.EmbedUrl);
            }
        }
    }
}
