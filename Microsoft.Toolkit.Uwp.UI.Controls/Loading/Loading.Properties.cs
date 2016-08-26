using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Loading control allows to show an loading animation with some xaml in it.
    /// </summary>
    public sealed partial class Loading
    {
        public static readonly DependencyProperty LoadingVerticalAlignmentProperty = DependencyProperty.Register(
            "LoadingVerticalAlignment", typeof(VerticalAlignment), typeof(Loading), new PropertyMetadata(default(VerticalAlignment)));

        public static readonly DependencyProperty LoadingHorizontalAlignmentProperty = DependencyProperty.Register(
            "LoadingHorizontalAlignment", typeof(HorizontalAlignment), typeof(Loading), new PropertyMetadata(default(HorizontalAlignment)));

        public static readonly DependencyProperty LoadingContentProperty = DependencyProperty.Register(
            "LoadingContent", typeof(DataTemplate), typeof(Loading), new PropertyMetadata(default(DataTemplate), LoadingContentPropertyChanged));

        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(
            "IsLoading", typeof(bool), typeof(Loading), new PropertyMetadata(default(bool), IsLoadingPropertyChanged));

        public static readonly DependencyProperty LoadingOpacityProperty = DependencyProperty.Register(
            "LoadingOpacity", typeof(double), typeof(Loading), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty LoadingBackgroundProperty = DependencyProperty.Register(
            "LoadingBackground", typeof(SolidColorBrush), typeof(Loading), new PropertyMetadata(default(SolidColorBrush)));

        private Grid RootGrid { get; }

        private Grid BackgroundGrid { get; set; }

        private Grid ContentGrid { get; }

        private Storyboard Animation { get; set; }

        /// <summary>
        /// Gets or sets loadingVerticalAlignment
        /// </summary>
        public VerticalAlignment LoadingVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(LoadingVerticalAlignmentProperty); }
            set { SetValue(LoadingVerticalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets loadingHorizontalAlignment
        /// </summary>
        public HorizontalAlignment LoadingHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(LoadingHorizontalAlignmentProperty); }
            set { SetValue(LoadingHorizontalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets loadingContent
        /// </summary>
        public DataTemplate LoadingContent
        {
            get { return (DataTemplate)GetValue(LoadingContentProperty); }
            set { SetValue(LoadingContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether isLoading
        /// </summary>
        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        /// <summary>
        /// Gets or sets loadingOpacity
        /// </summary>
        public double LoadingOpacity
        {
            get { return (double)GetValue(LoadingOpacityProperty); }
            set { SetValue(LoadingOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets loadingBackground
        /// </summary>
        public SolidColorBrush LoadingBackground
        {
            get { return (SolidColorBrush)GetValue(LoadingBackgroundProperty); }
            set { SetValue(LoadingBackgroundProperty, value); }
        }

        private static void IsLoadingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Loading)?.CreateLoadingControl();
        }

        private static void LoadingContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Loading)?.CreateLoadingControl();
        }
    }
}
