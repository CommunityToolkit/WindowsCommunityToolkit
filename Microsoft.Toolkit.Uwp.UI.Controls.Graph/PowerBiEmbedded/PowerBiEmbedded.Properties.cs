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
            new PropertyMetadata(null, OnClientIdPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="GroupId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GroupIdProperty = DependencyProperty.Register(
            nameof(GroupId),
            typeof(string),
            typeof(PowerBiEmbedded),
            new PropertyMetadata(null, OnGroupIdPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="EmbedUrl"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EmbedUrlProperty = DependencyProperty.Register(
            nameof(EmbedUrl),
            typeof(string),
            typeof(PowerBiEmbedded),
            new PropertyMetadata(null, OnEmbedUrlPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="Id"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
            nameof(Id),
            typeof(string),
            typeof(PowerBiEmbedded),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the Application Client Id(v1)
        /// </summary>
        public string ClientId
        {
            get { return (string)GetValue(ClientIdProperty); }
            set { SetValue(ClientIdProperty, value); }
        }

        /// <summary>
        /// Gets or sets the PowerBI GroupId
        /// </summary>
        public string GroupId
        {
            get { return (string)GetValue(GroupIdProperty); }
            set { SetValue(GroupIdProperty, value); }
        }

        /// <summary>
        /// Gets or sets the PowerBI report EmbedUrl
        /// </summary>
        public string EmbedUrl
        {
            get { return (string)GetValue(EmbedUrlProperty); }
            set { SetValue(EmbedUrlProperty, value); }
        }

        /// <summary>
        /// Gets the PowerBI report Id
        /// </summary>
        public string Id
        {
            get { return (string)GetValue(IdProperty); }
            private set { SetValue(IdProperty, value); }
        }

        internal static readonly DependencyProperty ReportsProperty = DependencyProperty.Register(
            nameof(Reports),
            typeof(IList<Report>),
            typeof(PowerBiEmbedded),
            new PropertyMetadata(null));

        internal IList<Report> Reports
        {
            get { return (IList<Report>)GetValue(ReportsProperty); }
            private set { SetValue(ReportsProperty, value); }
        }

        internal static readonly DependencyProperty SelectionReportProperty = DependencyProperty.Register(
            nameof(SelectionReport),
            typeof(Report),
            typeof(PowerBiEmbedded),
            new PropertyMetadata(null, OnSelectionReportPropertyChanged));

        internal Report SelectionReport
        {
            get { return (Report)GetValue(SelectionReportProperty); }
            set { SetValue(SelectionReportProperty, value); }
        }
    }
}