// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
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
    }
}
