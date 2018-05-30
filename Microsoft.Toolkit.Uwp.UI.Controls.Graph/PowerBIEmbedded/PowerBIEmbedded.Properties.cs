// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.PowerBI.Api.V2.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Defines the properties for the <see cref="PowerBiEmbedded"/> control.
    /// </summary>
    public partial class PowerBiEmbedded : Control
    {
        /// <summary>
        /// Identifies the <see cref="ClientId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ClientIdProperty = DependencyProperty.Register(
            nameof(ClientId),
            typeof(string),
            typeof(PowerBiEmbedded),
            new PropertyMetadata(null, OnPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="GroupId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GroupIdProperty = DependencyProperty.Register(
            nameof(GroupId),
            typeof(string),
            typeof(PowerBiEmbedded),
            new PropertyMetadata(null, OnPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="EmbedUrl"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EmbedUrlProperty = DependencyProperty.Register(
            nameof(EmbedUrl),
            typeof(string),
            typeof(PowerBiEmbedded),
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
    }
}
