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

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    using Helpers;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Panel that allows for a Master/Details pattern.
    /// </summary>
    /// <seealso cref="ContentControl" />
    public partial class TaskControl
    {
        /// <summary>
        /// Identifies the <see cref="MasterHeader"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="MasterHeader"/> dependency property.</returns>
        public static readonly DependencyProperty MasterHeaderProperty = DependencyProperty.Register(
            nameof(MasterHeader),
            typeof(object),
            typeof(TaskControl),
            new PropertyMetadata(null, OnMasterHeaderChanged));

        /// <summary>
        /// Identifies the <see cref="MasterHeaderTemplate"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="MasterHeaderTemplate"/> dependency property.</returns>
        public static readonly DependencyProperty MasterHeaderTemplateProperty = DependencyProperty.Register(
            nameof(MasterHeaderTemplate),
            typeof(DataTemplate),
            typeof(TaskControl),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TaskProperty = DependencyProperty.Register(
            nameof(Task),
            typeof(ITaskWithNotification),
            typeof(TaskControl),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(nameof(ContentTemplate), typeof(DataTemplate), typeof(TaskControl), new PropertyMetadata(null));

        public static readonly DependencyProperty NotStartedContentProperty =
            DependencyProperty.Register(nameof(NotStartedContent), typeof(object), typeof(TaskControl), new PropertyMetadata(new TextBlock { Text = "NOT STARTED", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center }));

        public static readonly DependencyProperty NotStartedContentTemplateProperty =
            DependencyProperty.Register(nameof(NotStartedContentTemplate), typeof(DataTemplate), typeof(TaskControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ProgressContentProperty =
            DependencyProperty.Register(nameof(ProgressContent), typeof(object), typeof(TaskControl), new PropertyMetadata(new ProgressRing { IsActive = true }));

        public static readonly DependencyProperty ProgressContentTemplateProperty =
            DependencyProperty.Register(nameof(ProgressContentTemplate), typeof(DataTemplate), typeof(TaskControl), new PropertyMetadata(null));

        public static readonly DependencyProperty CanceledContentProperty =
            DependencyProperty.Register(nameof(CanceledContent), typeof(object), typeof(TaskControl), new PropertyMetadata(new TextBlock { Text = "CANCELED", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center }));

        public static readonly DependencyProperty CanceledContentTemplateProperty =
            DependencyProperty.Register(nameof(CanceledContentTemplate), typeof(DataTemplate), typeof(TaskControl), new PropertyMetadata(null));

        public static readonly DependencyProperty FaultedContentProperty =
            DependencyProperty.Register(nameof(FaultedContent), typeof(object), typeof(TaskControl), new PropertyMetadata(new TextBlock { Text = "EXCEPTION", VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center }));

        public static readonly DependencyProperty FaultedContentTemplateProperty =
            DependencyProperty.Register(nameof(FaultedContentTemplate), typeof(DataTemplate), typeof(TaskControl), new PropertyMetadata(null));

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

        public ITaskWithNotification Task
        {
            get { return (ITaskWithNotification)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public object NotStartedContent
        {
            get { return (object)GetValue(NotStartedContentProperty); }
            set { SetValue(NotStartedContentProperty, value); }
        }

        public DataTemplate NotStartedContentTemplate
        {
            get { return (DataTemplate)GetValue(NotStartedContentTemplateProperty); }
            set { SetValue(NotStartedContentTemplateProperty, value); }
        }

        public object ProgressContent
        {
            get { return (object)GetValue(ProgressContentProperty); }
            set { SetValue(ProgressContentProperty, value); }
        }

        public DataTemplate ProgressContentTemplate
        {
            get { return (DataTemplate)GetValue(ProgressContentTemplateProperty); }
            set { SetValue(ProgressContentTemplateProperty, value); }
        }

        public object CanceledContent
        {
            get { return (object)GetValue(CanceledContentProperty); }
            set { SetValue(CanceledContentProperty, value); }
        }

        public DataTemplate CanceledContentTemplate
        {
            get { return (DataTemplate)GetValue(CanceledContentTemplateProperty); }
            set { SetValue(CanceledContentTemplateProperty, value); }
        }

        public object FaultedContent
        {
            get { return (object)GetValue(FaultedContentProperty); }
            set { SetValue(FaultedContentProperty, value); }
        }

        public DataTemplate FaultedContentTemplate
        {
            get { return (DataTemplate)GetValue(FaultedContentTemplateProperty); }
            set { SetValue(FaultedContentTemplateProperty, value); }
        }
    }
}
