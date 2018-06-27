using System;
using System.Windows;
using Microsoft.Windows.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class MediaPlayerElement : WindowsXamlHost
    {
        protected global::Windows.UI.Xaml.Controls.MediaPlayerElement UwpControl => this.XamlRoot as global::Windows.UI.Xaml.Controls.MediaPlayerElement;

        public MediaPlayerElement()
            : this("Windows.UI.Xaml.Controls.MediaPlayerElement")
        {
        }

        // Summary:
        //     Initializes a new instance of the MediaPlayerElement class.
        public MediaPlayerElement(string typeName)
            : base(typeName)
        {
        }

        protected override void OnInitialized(EventArgs e)
        {
            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.NameProperty);
            Bind(nameof(Tag), TagProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.DataContextProperty);
            Bind(nameof(Width), WidthProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.WidthProperty);
            Bind(nameof(MediaPlayer), MediaPlayerProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.MediaPlayerProperty, null, System.ComponentModel.BindingDirection.OneWay);

            // MediaPlayerElement specific properties
            Bind(nameof(Stretch), StretchProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.StretchProperty);
            Bind(nameof(Source), SourceProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.SourceProperty, new MediaSourceConverter());
            Bind(nameof(PosterSource), PosterSourceProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.PosterSourceProperty);
            Bind(nameof(IsFullWindow), IsFullWindowProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.IsFullWindowProperty);
            Bind(nameof(AutoPlay), AutoPlayProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.AutoPlayProperty);
            Bind(nameof(AreTransportControlsEnabled), AreTransportControlsEnabledProperty, global::Windows.UI.Xaml.Controls.MediaPlayerElement.AreTransportControlsEnabledProperty);

            base.OnInitialized(e);
        }

        public static DependencyProperty AreTransportControlsEnabledProperty { get; } = DependencyProperty.Register(nameof(AreTransportControlsEnabled), typeof(bool), typeof(MediaPlayerElement));

        public static DependencyProperty AutoPlayProperty { get; } = DependencyProperty.Register(nameof(AutoPlay), typeof(bool), typeof(MediaPlayerElement));

        public static DependencyProperty IsFullWindowProperty { get; } = DependencyProperty.Register(nameof(IsFullWindow), typeof(bool), typeof(MediaPlayerElement));

        public static DependencyProperty MediaPlayerProperty { get; } = DependencyProperty.Register(nameof(MediaPlayer), typeof(global::Windows.Media.Playback.MediaPlayer), typeof(MediaPlayerElement));

        public static DependencyProperty PosterSourceProperty { get; } = DependencyProperty.Register(nameof(PosterSource), typeof(global::Windows.UI.Xaml.Media.ImageSource), typeof(MediaPlayerElement));

        public static DependencyProperty SourceProperty { get; } = DependencyProperty.Register(nameof(Source), typeof(string), typeof(MediaPlayerElement));

        public static DependencyProperty StretchProperty { get; } = DependencyProperty.Register(nameof(Stretch), typeof(global::Windows.UI.Xaml.Media.Stretch), typeof(MediaPlayerElement));

        public void SetMediaPlayer(global::Windows.Media.Playback.MediaPlayer mediaPlayer) => UwpControl.SetMediaPlayer(mediaPlayer);

        public global::Windows.UI.Xaml.Controls.MediaTransportControls TransportControls
        {
            get => UwpControl.TransportControls;
            set => UwpControl.TransportControls = value;
        }

        public global::Windows.UI.Xaml.Media.Stretch Stretch
        {
            get => (global::Windows.UI.Xaml.Media.Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public global::Windows.UI.Xaml.Media.ImageSource PosterSource
        {
            get => (global::Windows.UI.Xaml.Media.ImageSource)GetValue(PosterSourceProperty);
            set => SetValue(PosterSourceProperty, value);
        }

        public bool IsFullWindow
        {
            get => (bool)GetValue(IsFullWindowProperty);
            set => SetValue(IsFullWindowProperty, value);
        }

        public bool AutoPlay
        {
            get => (bool)GetValue(AutoPlayProperty);
            set => SetValue(AutoPlayProperty, value);
        }

        public bool AreTransportControlsEnabled
        {
            get => (bool)GetValue(AreTransportControlsEnabledProperty);
            set => SetValue(AreTransportControlsEnabledProperty, value);
        }

        public global::Windows.Media.Playback.MediaPlayer MediaPlayer
        {
            get => (global::Windows.Media.Playback.MediaPlayer)GetValue(MediaPlayerProperty);
        }
    }
}