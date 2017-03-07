using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The<see cref="FlyoutView"/> control allows the creation of a flyout view that can
    /// open and close with animations.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentControl" />
    public partial class FlyoutView
    {
        /// <summary>
        /// The header template property
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            "HeaderTemplate", typeof(DataTemplate), typeof(FlyoutView), new PropertyMetadata(default(DataTemplate)));

        /// <summary>
        /// The orientation property
        /// </summary>
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register(
            "Placement", typeof(PlacementType), typeof(FlyoutView), new PropertyMetadata(default(PlacementType), OnPlacementChanged));

        /// <summary>
        /// The use dismiss ovelay property
        /// </summary>
        public static readonly DependencyProperty UseDismissOvelayProperty = DependencyProperty.Register(
           "UseDismissOvelay", typeof(bool), typeof(FlyoutView), new PropertyMetadata(default(bool)));

        /// <summary>
        /// The background overlay property
        /// </summary>
        public static readonly DependencyProperty BackgroundOverlayProperty = DependencyProperty.Register(
            "BackgroundOverlay", typeof(SolidColorBrush), typeof(FlyoutView), new PropertyMetadata(default(SolidColorBrush)));

        /// <summary>
        /// The is open property
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            "IsOpen", typeof(bool), typeof(FlyoutView), new PropertyMetadata(default(bool), OnIsOpenChanged));

        /// <summary>
        /// The content width property
        /// </summary>
        public static readonly DependencyProperty ContentWidthProperty = DependencyProperty.Register(
            "ContentWidth", typeof(double), typeof(FlyoutView), new PropertyMetadata(default(double)));

        /// <summary>
        /// The content height property
        /// </summary>
        public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register(
            "ContentHeight", typeof(double), typeof(FlyoutView), new PropertyMetadata(default(double)));

        /// <summary>
        /// Gets or sets the header template.
        /// </summary>
        /// <value>
        /// The header template.
        /// </value>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        /// <value>
        /// The orientation.
        /// </value>
        public PlacementType Placement
        {
            get { return (PlacementType)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [use dismiss ovelay].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use dismiss ovelay]; otherwise, <c>false</c>.
        /// </value>
        public bool UseDismissOvelay
        {
            get { return (bool)GetValue(UseDismissOvelayProperty); }
            set { SetValue(UseDismissOvelayProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background overlay.
        /// </summary>
        /// <value>
        /// The background overlay.
        /// </value>
        public SolidColorBrush BackgroundOverlay
        {
            get { return (SolidColorBrush)GetValue(BackgroundOverlayProperty); }
            set { SetValue(BackgroundOverlayProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
        /// </value>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the content.
        /// </summary>
        /// <value>
        /// The width of the content.
        /// </value>
        public double ContentWidth
        {
            get { return (double)GetValue(ContentWidthProperty); }
            set { SetValue(ContentWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the height of the content.
        /// </summary>
        /// <value>
        /// The height of the content.
        /// </value>
        public double ContentHeight
        {
            get { return (double)GetValue(ContentHeightProperty); }
            set { SetValue(ContentHeightProperty, value); }
        }

        /// <summary>
        /// Called when [is open changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsOpenChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var control = sender as FlyoutView;

            control?.HandleOpening();
        }

        /// <summary>
        /// Called when [placement changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnPlacementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var control = sender as FlyoutView;

            control?.UpdateFromLayout();
        }

        /// <summary>
        /// Handles the open close.
        /// </summary>
        private void HandleOpening()
        {
            if (IsOpen)
            {
                if (UseDismissOvelay)
                {
                    _overlay.Visibility = Visibility.Visible;
                }

                _openStoryboard?.Begin();
            }
            else
            {
                if (UseDismissOvelay)
                {
                    _overlay.Visibility = Visibility.Collapsed;
                }

                _closeStoryboard?.Begin();
            }
        }
    }
}
