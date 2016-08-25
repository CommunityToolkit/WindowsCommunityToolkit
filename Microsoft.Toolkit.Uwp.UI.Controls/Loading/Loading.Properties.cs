using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Loading control allows to show an loading animation with some xaml in it.
    /// </summary>
    public sealed partial class Loading : ContentControl
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

        protected Grid RootGrid { get; }

        protected Grid BackgroundGrid { get; set; }

        protected Grid ContentGrid { get; }

        protected Storyboard Animation { get; set; }

        public VerticalAlignment LoadingVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(LoadingVerticalAlignmentProperty); }
            set { SetValue(LoadingVerticalAlignmentProperty, value); }
        }

        public HorizontalAlignment LoadingHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(LoadingHorizontalAlignmentProperty); }
            set { SetValue(LoadingHorizontalAlignmentProperty, value); }
        }

        public DataTemplate LoadingContent
        {
            get { return (DataTemplate)GetValue(LoadingContentProperty); }
            set { SetValue(LoadingContentProperty, value); }
        }

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public double LoadingOpacity
        {
            get { return (double)GetValue(LoadingOpacityProperty); }
            set { SetValue(LoadingOpacityProperty, value); }
        }

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
