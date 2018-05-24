// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Panel that allows for a Master/Details pattern.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ItemsControl" />
    public partial class MasterDetailsView
    {
        /// <summary>
        /// Identifies the <see cref="SelectedItem"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="SelectedItem"/> dependency property.</returns>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(object),
            typeof(MasterDetailsView),
            new PropertyMetadata(null, OnSelectedItemChanged));

        /// <summary>
        /// Identifies the <see cref="DetailsTemplate"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="DetailsTemplate"/> dependency property.</returns>
        public static readonly DependencyProperty DetailsTemplateProperty = DependencyProperty.Register(
            nameof(DetailsTemplate),
            typeof(DataTemplate),
            typeof(MasterDetailsView),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="MasterPaneBackground"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="MasterPaneBackground"/> dependency property.</returns>
        public static readonly DependencyProperty MasterPaneBackgroundProperty = DependencyProperty.Register(
            nameof(MasterPaneBackground),
            typeof(Brush),
            typeof(MasterDetailsView),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="MasterHeader"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="MasterHeader"/> dependency property.</returns>
        public static readonly DependencyProperty MasterHeaderProperty = DependencyProperty.Register(
            nameof(MasterHeader),
            typeof(object),
            typeof(MasterDetailsView),
            new PropertyMetadata(null, OnMasterHeaderChanged));

        /// <summary>
        /// Identifies the <see cref="MasterHeaderTemplate"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="MasterHeaderTemplate"/> dependency property.</returns>
        public static readonly DependencyProperty MasterHeaderTemplateProperty = DependencyProperty.Register(
            nameof(MasterHeaderTemplate),
            typeof(DataTemplate),
            typeof(MasterDetailsView),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="MasterPaneWidth"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="MasterPaneWidth"/> dependency property.</returns>
        public static readonly DependencyProperty MasterPaneWidthProperty = DependencyProperty.Register(
            nameof(MasterPaneWidth),
            typeof(double),
            typeof(MasterDetailsView),
            new PropertyMetadata(320d));

        /// <summary>
        /// Identifies the <see cref="NoSelectionContent"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="NoSelectionContent"/> dependency property.</returns>
        public static readonly DependencyProperty NoSelectionContentProperty = DependencyProperty.Register(
            nameof(NoSelectionContent),
            typeof(object),
            typeof(MasterDetailsView),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="NoSelectionContentTemplate"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="NoSelectionContentTemplate"/> dependency property.</returns>
        public static readonly DependencyProperty NoSelectionContentTemplateProperty = DependencyProperty.Register(
            nameof(NoSelectionContentTemplate),
            typeof(DataTemplate),
            typeof(MasterDetailsView),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewState"/> dependency property
        /// </summary>
        /// <returns>The identifier for the <see cref="ViewState"/> dependency property.</returns>
        public static readonly DependencyProperty ViewStateProperty = DependencyProperty.Register(
            nameof(ViewState),
            typeof(MasterDetailsViewState),
            typeof(MasterDetailsView),
            new PropertyMetadata(default(MasterDetailsViewState)));

        /// <summary>
        /// Identifies the <see cref="MasterCommandBar"/> dependency property
        /// </summary>
        /// <returns>The identifier for the <see cref="MasterCommandBar"/> dependency property.</returns>
        public static readonly DependencyProperty MasterCommandBarProperty = DependencyProperty.Register(
            nameof(MasterCommandBar),
            typeof(CommandBar),
            typeof(MasterDetailsView),
            new PropertyMetadata(null, OnMasterCommandBarChanged));

        /// <summary>
        /// Identifies the <see cref="DetailsCommandBar"/> dependency property
        /// </summary>
        /// <returns>The identifier for the <see cref="DetailsCommandBar"/> dependency property.</returns>
        public static readonly DependencyProperty DetailsCommandBarProperty = DependencyProperty.Register(
            nameof(DetailsCommandBar),
            typeof(CommandBar),
            typeof(MasterDetailsView),
            new PropertyMetadata(null, OnDetailsCommandBarChanged));

        /// <summary>
        /// Identifies the <see cref="CompactModeThresholdWidth"/> dependancy property
        /// </summary>
        public static readonly DependencyProperty CompactModeThresholdWidthProperty = DependencyProperty.Register(
            nameof(CompactModeThresholdWidth),
            typeof(double),
            typeof(MasterDetailsView),
            new PropertyMetadata(720d, OnCompactModeThresholdWidthChanged));

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <returns>The selected item. The default is null.</returns>
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate used to display the details.
        /// </summary>
        public DataTemplate DetailsTemplate
        {
            get { return (DataTemplate)GetValue(DetailsTemplateProperty); }
            set { SetValue(DetailsTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Brush to apply to the background of the list area of the control.
        /// </summary>
        /// <returns>The Brush to apply to the background of the list area of the control.</returns>
        public Brush MasterPaneBackground
        {
            get { return (Brush)GetValue(MasterPaneBackgroundProperty); }
            set { SetValue(MasterPaneBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content for the master pane's header
        /// </summary>
        /// <returns>
        /// The content of the master pane's header. The default is null.
        /// </returns>
        public object MasterHeader
        {
            get { return (object)GetValue(MasterHeaderProperty); }
            set { SetValue(MasterHeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate used to display the content of the master pane's header.
        /// </summary>
        /// <returns>
        /// The template that specifies the visualization of the master pane header object. The default is null.
        /// </returns>
        public DataTemplate MasterHeaderTemplate
        {
            get { return (DataTemplate)GetValue(MasterHeaderTemplateProperty); }
            set { SetValue(MasterHeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the master pane when the view is expanded.
        /// </summary>
        /// <returns>
        /// The width of the SplitView pane when it's fully expanded. The default is 320
        /// device-independent pixel (DIP).
        /// </returns>
        public double MasterPaneWidth
        {
            get { return (double)GetValue(MasterPaneWidthProperty); }
            set { SetValue(MasterPaneWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content to dsiplay when there is no item selected in the master list.
        /// </summary>
        public object NoSelectionContent
        {
            get { return (object)GetValue(NoSelectionContentProperty); }
            set { SetValue(NoSelectionContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate used to display the content when there is no selection.
        /// </summary>
        /// <returns>
        /// The template that specifies the visualization of the content when there is no
        /// selection. The default is null.
        /// </returns>
        public DataTemplate NoSelectionContentTemplate
        {
            get { return (DataTemplate)GetValue(NoSelectionContentTemplateProperty); }
            set { SetValue(NoSelectionContentTemplateProperty, value); }
        }

        /// <summary>
        /// Gets the current visual state of the control
        /// </summary>
        public MasterDetailsViewState ViewState
        {
            get { return (MasterDetailsViewState)GetValue(ViewStateProperty); }
            private set { SetValue(ViewStateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="CommandBar"/> for the master section.
        /// </summary>
        public CommandBar MasterCommandBar
        {
            get { return (CommandBar)GetValue(MasterCommandBarProperty); }
            set { SetValue(MasterCommandBarProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="CommandBar"/> for the details section.
        /// </summary>
        public CommandBar DetailsCommandBar
        {
            get { return (CommandBar)GetValue(DetailsCommandBarProperty); }
            set { SetValue(DetailsCommandBarProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Threshold width that witll trigger the control to go into compact mode
        /// </summary>
        public double CompactModeThresholdWidth
        {
            get { return (double)GetValue(CompactModeThresholdWidthProperty); }
            set { SetValue(CompactModeThresholdWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a function for mapping the selected item to a different model.
        /// This new model will be the DataContext of the Details area.
        /// </summary>
        public Func<object, object> MapDetails { get; set; }
    }
}
