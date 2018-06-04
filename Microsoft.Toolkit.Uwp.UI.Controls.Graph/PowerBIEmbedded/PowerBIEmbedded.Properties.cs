// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Graphics.Display;
using Windows.System.Profile;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Defines the properties for the <see cref="PowerBIEmbedded"/> control.
    /// </summary>
    public partial class PowerBIEmbedded : Control
    {
        /// <summary>
        /// Identifies the <see cref="ClientId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ClientIdProperty = DependencyProperty.Register(
            nameof(ClientId),
            typeof(string),
            typeof(PowerBIEmbedded),
            new PropertyMetadata(null, OnPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="GroupId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GroupIdProperty = DependencyProperty.Register(
            nameof(GroupId),
            typeof(string),
            typeof(PowerBIEmbedded),
            new PropertyMetadata(null, OnPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="EmbedUrl"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EmbedUrlProperty = DependencyProperty.Register(
            nameof(EmbedUrl),
            typeof(string),
            typeof(PowerBIEmbedded),
            new PropertyMetadata(null, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the Application Client Id(v1)
        /// </summary>
        public string ClientId
        {
            get { return ((string)GetValue(ClientIdProperty))?.Trim(); }
            set { SetValue(ClientIdProperty, value?.Trim()); }
        }

        /// <summary>
        /// Gets or sets the PowerBI GroupId
        /// </summary>
        public string GroupId
        {
            get { return ((string)GetValue(GroupIdProperty))?.Trim(); }
            set { SetValue(GroupIdProperty, value?.Trim()); }
        }

        /// <summary>
        /// Gets or sets the PowerBI report EmbedUrl
        /// </summary>
        public string EmbedUrl
        {
            get { return ((string)GetValue(EmbedUrlProperty))?.Trim(); }
            set { SetValue(EmbedUrlProperty, value?.Trim()); }
        }

        private bool IsWindowsPhone
        {
            get
            {
                return AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile";
            }
        }

        private DisplayOrientations Orientations
        {
            get
            {
                if (IsWindowsPhone)
                {
                    var bounds = ApplicationView.GetForCurrentView().VisibleBounds;

                    if (bounds.Height > bounds.Width)
                    {
                        return DisplayOrientations.Portrait;
                    }
                    else
                    {
                        return DisplayOrientations.Landscape;
                    }
                }

                return DisplayOrientations.None;
            }
        }
    }
}
