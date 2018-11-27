// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using Microsoft.Toolkit.Forms.UI.XamlHost;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    [Designer(typeof(MediaPlayerElementDesigner))]
    public class MediaPlayerElement : WindowsXamlHostBase
    {
        internal Windows.UI.Xaml.Controls.MediaPlayerElement UwpControl => GetUwpInternalObject() as Windows.UI.Xaml.Controls.MediaPlayerElement;

        private string _source;

        private System.Collections.Generic.Dictionary<string, object> DesignerProperties { get; set; }

        public MediaPlayerElement()
            : this(typeof(Windows.UI.Xaml.Controls.MediaPlayerElement).FullName)
        {
        }

        protected MediaPlayerElement(string name)
            : base(name)
        {
            // Return immediately if control is instantiated by the Visual Studio Designer
            // https://stackoverflow.com/questions/1166226/detecting-design-mode-from-a-controls-constructor
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (UwpControl != null && TransportControls != null)
            {
                TransportControls.IsFullWindowButtonVisible = false;
            }
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.MediaPlayerElement.SetMediaPlayer"/>
        /// </summary>
        public void SetMediaPlayer(MediaPlayer mediaPlayer) => UwpControl.SetMediaPlayer(mediaPlayer.UwpInstance);

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.MediaPlayerElement.TransportControls"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MediaTransportControls TransportControls
        {
            get => UwpControl.TransportControls;
            set => UwpControl.TransportControls = value.UwpInstance;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.MediaPlayerElement.Stretch"/>
        /// </summary>
        [DefaultValue(Stretch.Uniform)]
        [Category("Appearance")]
        public Stretch Stretch
        {
            get => (Stretch)this.GetUwpControlValue();
            set => this.SetUwpControlValue((Windows.UI.Xaml.Media.Stretch)value);
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.MediaPlayerElement.Source"/>
        /// </summary>
        [DisplayName("Source")]
        public string Source
        {
            get
            {
                if (DesignMode)
                {
                    return _source;
                }

                return ((Windows.Media.Core.MediaSource)UwpControl.Source)?.Uri?.ToString();
            }

            set
            {
                if (DesignMode)
                {
                    _source = value;
                    return;
                }

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
        [DefaultValue(null)]
        public ImageSource PosterSource
        {
            get => (Windows.UI.Xaml.Media.ImageSource)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value.UwpInstance);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaPlayerElement.IsFullWindow"/>
        /// </summary>
        [DefaultValue(false)]
        public bool IsFullWindow
        {
            get => (bool)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaPlayerElement.AutoPlay"/>
        /// </summary>
        [DefaultValue(true)]
        public bool AutoPlay
        {
            get => (bool)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaPlayerElement.AreTransportControlsEnabled"/>
        /// </summary>
        [DefaultValue(false)]
        public bool AreTransportControlsEnabled
        {
            get => (bool)this.GetUwpControlValue();
            set => this.SetUwpControlValue(value);
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.MediaPlayerElement.MediaPlayer"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MediaPlayer MediaPlayer
        {
            get => UwpControl.MediaPlayer;
        }
    }
}
