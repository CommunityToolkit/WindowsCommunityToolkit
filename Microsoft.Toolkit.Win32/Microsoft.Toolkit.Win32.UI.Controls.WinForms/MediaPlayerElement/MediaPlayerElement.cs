using System;
using System.ComponentModel;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    [Designer(typeof(MediaPlayerElementDesigner))]
    public class MediaPlayerElement : WindowsXamlHostBaseExt
    {
        internal Windows.UI.Xaml.Controls.MediaPlayerElement UwpControl { get; set; }

        public MediaPlayerElement()
            : this(typeof(Windows.UI.Xaml.Controls.MediaPlayerElement).FullName)
        {
            TransportControls.IsFullWindowButtonVisible = false;
        }

        protected MediaPlayerElement(string name)
            : base(name)
        {
            UwpControl = XamlElement as Windows.UI.Xaml.Controls.MediaPlayerElement;
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.MediaPlayerElement.SetMediaPlayer"/>
        /// </summary>
        public void SetMediaPlayer(MediaPlayer mediaPlayer) => UwpControl.SetMediaPlayer(mediaPlayer.UwpInstance);

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.MediaPlayerElement.TransportControls"/>
        /// </summary>
        [Browsable(false)]
        public MediaTransportControls TransportControls
        {
            get => UwpControl.TransportControls;
            set => UwpControl.TransportControls = value.UwpInstance;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.MediaPlayerElement.Stretch"/>
        /// </summary>
        [DefaultValue(Stretch.UniformToFill)]
        [Category("Appearance")]
        public Stretch Stretch
        {
            get => (Stretch)UwpControl.Stretch;
            set => UwpControl.Stretch = (Windows.UI.Xaml.Media.Stretch)value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.MediaPlayerElement.Source"/>
        /// </summary>
        [DisplayName("Source")]
        public string Source
        {
            get => ((Windows.Media.Core.MediaSource)UwpControl.Source)?.Uri?.ToString();
            set
            {
                if (value == null)
                {
                    UwpControl.Source = null;
                }
                else
                {
                    UwpControl.Source = Windows.Media.Core.MediaSource.CreateFromUri(new Uri(value));
                }
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.MediaPlayerElement.PosterSource"/>
        /// </summary>
        public ImageSource PosterSource
        {
            get => UwpControl.PosterSource;
            set => UwpControl.PosterSource = value.UwpInstance;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaPlayerElement.IsFullWindow"/>
        /// </summary>
        public bool IsFullWindow
        {
            get => UwpControl.IsFullWindow;
            set => UwpControl.IsFullWindow = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaPlayerElement.AutoPlay"/>
        /// </summary>
        public bool AutoPlay
        {
            get => UwpControl.AutoPlay;
            set => UwpControl.AutoPlay = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaPlayerElement.AreTransportControlsEnabled"/>
        /// </summary>
        public bool AreTransportControlsEnabled
        {
            get => UwpControl.AreTransportControlsEnabled;
            set => UwpControl.AreTransportControlsEnabled = value;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.MediaPlayerElement.MediaPlayer"/>
        /// </summary>
        public MediaPlayer MediaPlayer
        {
            get => UwpControl.MediaPlayer;
        }
    }
}
