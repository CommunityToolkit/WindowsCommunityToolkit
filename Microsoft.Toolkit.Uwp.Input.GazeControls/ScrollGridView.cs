using Microsoft.Toolkit.Uwp.UI;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.Input.GazeControls
{
    /// <summary>
    /// A custom version of GridView to manage the scroll bar associated with the GridView
    /// </summary>
    public class ScrollGridView : GridView
    {
        private ScrollViewer _scrollViewer;

        /// <summary>
        /// Overridden version of OnApplyTemplate
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _scrollViewer = GetTemplateChild("ScrollViewer") as ScrollViewer;

            if (GazeScrollbar != null)
            {
                GazeScrollbar.AttachTo(_scrollViewer);
            }

            _scrollViewer.ViewChanged += this.OnScrollViewerViewChanged;
        }

        private void OnScrollViewerViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var item = this.FindDescendant<GridViewItem>();
            GazeScrollbar.LineHeight = item != null ? item.ActualHeight : 1;
        }

        /// <summary>
        /// Gets or sets the gaze optimized vertical scrollbar
        /// </summary>
        public GazeScrollbar GazeScrollbar { get; set; }
    }
}
