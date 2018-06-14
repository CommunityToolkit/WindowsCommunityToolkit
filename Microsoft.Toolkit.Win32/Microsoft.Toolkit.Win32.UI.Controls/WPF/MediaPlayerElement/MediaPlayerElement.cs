using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Windows.Interop;
using uwpControls = global::Windows.UI.Xaml.Controls;
using uwpMediaPlayer = global::Windows.Media.Playback;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class MediaPlayerElement : WindowsXamlHost
    {
        protected uwpControls.MediaPlayerElement UwpControl => this.XamlRoot as uwpControls.MediaPlayerElement;

        // Summary:
        //     Initializes a new instance of the MediaPlayerElement class.
        public MediaPlayerElement()
            : base()
        {
            TypeName = "Windows.UI.Xaml.Controls.MediaPlayerElement";
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Bind dependency properties across controls
            // properties of FrameworkElement
            Bind(nameof(Style), StyleProperty, uwpControls.MediaPlayerElement.StyleProperty);
            Bind(nameof(MaxHeight), MaxHeightProperty, uwpControls.MediaPlayerElement.MaxHeightProperty);
            Bind(nameof(FlowDirection), FlowDirectionProperty, uwpControls.MediaPlayerElement.FlowDirectionProperty);
            Bind(nameof(Margin), MarginProperty, uwpControls.MediaPlayerElement.MarginProperty);
            Bind(nameof(HorizontalAlignment), HorizontalAlignmentProperty, uwpControls.MediaPlayerElement.HorizontalAlignmentProperty);
            Bind(nameof(VerticalAlignment), VerticalAlignmentProperty, uwpControls.MediaPlayerElement.VerticalAlignmentProperty);
            Bind(nameof(MinHeight), MinHeightProperty, uwpControls.MediaPlayerElement.MinHeightProperty);
            Bind(nameof(Height), HeightProperty, uwpControls.MediaPlayerElement.HeightProperty);
            Bind(nameof(MinWidth), MinWidthProperty, uwpControls.MediaPlayerElement.MinWidthProperty);
            Bind(nameof(MaxWidth), MaxWidthProperty, uwpControls.MediaPlayerElement.MaxWidthProperty);
            Bind(nameof(UseLayoutRounding), UseLayoutRoundingProperty, uwpControls.MediaPlayerElement.UseLayoutRoundingProperty);
            Bind(nameof(Name), NameProperty, uwpControls.MediaPlayerElement.NameProperty);
            Bind(nameof(Tag), TagProperty, uwpControls.MediaPlayerElement.TagProperty);
            Bind(nameof(DataContext), DataContextProperty, uwpControls.MediaPlayerElement.DataContextProperty);
            Bind(nameof(Width), WidthProperty, uwpControls.MediaPlayerElement.WidthProperty);

            // MediaPlayerElement specific properties
            Bind(nameof(AutoPlay), AutoPlayProperty, uwpControls.MediaPlayerElement.AutoPlayProperty);
            Bind(nameof(Source), SourceProperty, uwpControls.MediaPlayerElement.SourceProperty, new MediaSourceConverter());
            Bind(nameof(Stretch), StretchProperty, uwpControls.MediaPlayerElement.StretchProperty);
            Bind(nameof(AreTransportControlsEnabled), AreTransportControlsEnabledProperty, uwpControls.MediaPlayerElement.AreTransportControlsEnabledProperty);
            Bind(nameof(PosterSource), PosterSourceProperty, uwpControls.MediaPlayerElement.PosterSourceProperty);
            Bind(nameof(IsFullWindow), IsFullWindowProperty, uwpControls.MediaPlayerElement.IsFullWindowProperty);

            // Bind(nameof(MediaPlayer), MediaPlayerProperty, uwpControls.MediaPlayerElement.MediaPlayerProperty);
        }

        // Summary:
        //     Identifies the AreTransportControlsEnabled dependency property.
        //
        // Returns:
        //     The identifier for the AreTransportControlsEnabled dependency property.
        public static DependencyProperty AreTransportControlsEnabledProperty { get; } = DependencyProperty.Register(nameof(AreTransportControlsEnabled), typeof(bool), typeof(MediaPlayerElement));

        // Summary:
        //     Identifies the AutoPlay dependency property.
        //
        // Returns:
        //     The identifier for the AutoPlay dependency property.
        public static DependencyProperty AutoPlayProperty { get; } = DependencyProperty.Register(nameof(AutoPlay), typeof(bool), typeof(MediaPlayerElement));

        // Summary:
        //     Identifies the IsFullWindow dependency property.
        //
        // Returns:
        //     The identifier for the IsFullWindow dependency property.
        public static DependencyProperty IsFullWindowProperty { get; } = DependencyProperty.Register(nameof(IsFullWindow), typeof(bool), typeof(MediaPlayerElement));

        // Summary:
        //     Identifies the MediaPlayer dependency property.
        //
        // Returns:
        //     The identifier for the MediaPlayer dependency property.
        public static DependencyProperty MediaPlayerProperty { get; } = DependencyProperty.Register(nameof(MediaPlayer), typeof(uwpMediaPlayer.MediaPlayer), typeof(MediaPlayerElement));

        // Summary:
        //     Identifies the PosterSource dependency property.
        //
        // Returns:
        //     The identifier for the PosterSource dependency property.
        public static DependencyProperty PosterSourceProperty { get; } = DependencyProperty.Register(nameof(PosterSource), typeof(ImageSource), typeof(MediaPlayerElement));

        // Summary:
        //     Identifies the Source dependency property.
        //
        // Returns:
        //     The identifier for the Source dependency property.
        public static DependencyProperty SourceProperty { get; } = DependencyProperty.Register(nameof(Source), typeof(string), typeof(MediaPlayerElement));

        // Summary:
        //     Identifies the Stretch dependency property.
        //
        // Returns:
        //     The identifier for the Stretch dependency property.
        public static DependencyProperty StretchProperty { get; } = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(MediaPlayerElement));

        // Summary:
        //     Sets the MediaPlayer instance used to render media.
        //
        // Parameters:
        //   mediaPlayer:
        //     The new MediaPlayer instance used to render media.
        public void SetMediaPlayer(uwpMediaPlayer.MediaPlayer mediaPlayer) => UwpControl.SetMediaPlayer(mediaPlayer);

        // Summary:
        //     Gets or sets the transport controls for the media.
        //
        // Returns:
        //     The transport controls for the media.
        public uwpControls.MediaTransportControls TransportControls
        {
            get => UwpControl.TransportControls; set { UwpControl.TransportControls = value; }
        }

        // Summary:
        //     Gets or sets a value that describes how an MediaPlayerElement should be stretched
        //     to fill the destination rectangle.
        //
        // Returns:
        //     A value of the Stretch enumeration that specifies how the source visual media
        //     is rendered. The default value is **Uniform**.
        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty); set { SetValue(StretchProperty, value); }
        }

        // Summary:
        //     Gets or sets a media source on the MediaElement.
        //
        // Returns:
        //     The source of the media. The default is **null**.
        public string Source
        {
            get => (string)GetValue(SourceProperty); set { SetValue(SourceProperty, value); }
        }

        // Summary:
        //     Gets or sets the image source that is used for a placeholder image during MediaPlayerElement
        //     loading transition states.
        //
        // Returns:
        //     An image source for a transition ImageBrush that is applied to the MediaPlayerElement
        //     content area.
        public ImageSource PosterSource
        {
            get => (ImageSource)GetValue(PosterSourceProperty); set { SetValue(PosterSourceProperty, value); }
        }

        // Summary:
        //     Gets or sets a value that specifies if the MediaPlayerElement is rendering in
        //     full window mode.
        //
        // Returns:
        //     **true** if the MediaPlayerElement is in full window mode; otherwise, **false**.
        //     The default is **false**.
        public bool IsFullWindow
        {
            get => (bool)GetValue(IsFullWindowProperty); set { SetValue(IsFullWindowProperty, value); }
        }

        // Summary:
        //     Gets or sets a value that indicates whether media will begin playback automatically
        //     when the Source property is set.
        //
        // Returns:
        //     **true** if playback is automatic; otherwise, **false**. The default is **true**.
        public bool AutoPlay
        {
            get => (bool)GetValue(AutoPlayProperty); set { SetValue(AutoPlayProperty, value); }
        }

        // Summary:
        //     Gets or sets a value that determines whether the standard transport controls
        //     are enabled.
        //
        // Returns:
        //     **true** if the standard transport controls are enabled; otherwise, **false**.
        //     The default is **false**.
        public bool AreTransportControlsEnabled
        {
            get => (bool)GetValue(AreTransportControlsEnabledProperty); set { SetValue(AreTransportControlsEnabledProperty, value); }
        }

        // Summary:
        //     Gets the MediaPlayer instance used to render media.
        //
        // Returns:
        //     The MediaPlayer instance used to render media.
        public uwpMediaPlayer.MediaPlayer MediaPlayer { get => (uwpMediaPlayer.MediaPlayer)GetValue(MediaPlayerProperty); }
    }
}