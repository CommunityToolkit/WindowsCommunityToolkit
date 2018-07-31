// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="Windows.UI.Xaml.Controls.MediaTransportControls"/>
    /// </summary>
    public class MediaTransportControls
    {
        internal Windows.UI.Xaml.Controls.MediaTransportControls UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTransportControls"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.MediaTransportControls"/>
        /// </summary>
        public MediaTransportControls(Windows.UI.Xaml.Controls.MediaTransportControls instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsZoomEnabled"/>
        /// </summary>
        public bool IsZoomEnabled
        {
            get => UwpInstance.IsZoomEnabled;
            set => UwpInstance.IsZoomEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsZoomButtonVisible"/>
        /// </summary>
        public bool IsZoomButtonVisible
        {
            get => UwpInstance.IsZoomButtonVisible;
            set => UwpInstance.IsZoomButtonVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsVolumeEnabled"/>
        /// </summary>
        public bool IsVolumeEnabled
        {
            get => UwpInstance.IsVolumeEnabled;
            set => UwpInstance.IsVolumeEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsVolumeButtonVisible"/>
        /// </summary>
        public bool IsVolumeButtonVisible
        {
            get => UwpInstance.IsVolumeButtonVisible;
            set => UwpInstance.IsVolumeButtonVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsStopEnabled"/>
        /// </summary>
        public bool IsStopEnabled
        {
            get => UwpInstance.IsStopEnabled;
            set => UwpInstance.IsStopEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsStopButtonVisible"/>
        /// </summary>
        public bool IsStopButtonVisible
        {
            get => UwpInstance.IsStopButtonVisible;
            set => UwpInstance.IsStopButtonVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsSeekEnabled"/>
        /// </summary>
        public bool IsSeekEnabled
        {
            get => UwpInstance.IsSeekEnabled;
            set => UwpInstance.IsSeekEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsSeekBarVisible"/>
        /// </summary>
        public bool IsSeekBarVisible
        {
            get => UwpInstance.IsSeekBarVisible;
            set => UwpInstance.IsSeekBarVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsPlaybackRateEnabled"/>
        /// </summary>
        public bool IsPlaybackRateEnabled
        {
            get => UwpInstance.IsPlaybackRateEnabled;
            set => UwpInstance.IsPlaybackRateEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsPlaybackRateButtonVisible"/>
        /// </summary>
        public bool IsPlaybackRateButtonVisible
        {
            get => UwpInstance.IsPlaybackRateButtonVisible;
            set => UwpInstance.IsPlaybackRateButtonVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsFullWindowEnabled"/>
        /// </summary>
        public bool IsFullWindowEnabled
        {
            get => UwpInstance.IsFullWindowEnabled;
            set => UwpInstance.IsFullWindowEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsFullWindowButtonVisible"/>
        /// </summary>
        public bool IsFullWindowButtonVisible
        {
            get => UwpInstance.IsFullWindowButtonVisible;
            set => UwpInstance.IsFullWindowButtonVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsFastRewindEnabled"/>
        /// </summary>
        public bool IsFastRewindEnabled
        {
            get => UwpInstance.IsFastRewindEnabled;
            set => UwpInstance.IsFastRewindEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsFastRewindButtonVisible"/>
        /// </summary>
        public bool IsFastRewindButtonVisible
        {
            get => UwpInstance.IsFastRewindButtonVisible;
            set => UwpInstance.IsFastRewindButtonVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsFastForwardEnabled"/>
        /// </summary>
        public bool IsFastForwardEnabled
        {
            get => UwpInstance.IsFastForwardEnabled;
            set => UwpInstance.IsFastForwardEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsFastForwardButtonVisible"/>
        /// </summary>
        public bool IsFastForwardButtonVisible
        {
            get => UwpInstance.IsFastForwardButtonVisible;
            set => UwpInstance.IsFastForwardButtonVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsCompact"/>
        /// </summary>
        public bool IsCompact
        {
            get => UwpInstance.IsCompact;
            set => UwpInstance.IsCompact = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsSkipForwardEnabled"/>
        /// </summary>
        public bool IsSkipForwardEnabled
        {
            get => UwpInstance.IsSkipForwardEnabled;
            set => UwpInstance.IsSkipForwardEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsSkipForwardButtonVisible"/>
        /// </summary>
        public bool IsSkipForwardButtonVisible
        {
            get => UwpInstance.IsSkipForwardButtonVisible;
            set => UwpInstance.IsSkipForwardButtonVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsSkipBackwardEnabled"/>
        /// </summary>
        public bool IsSkipBackwardEnabled
        {
            get => UwpInstance.IsSkipBackwardEnabled;
            set => UwpInstance.IsSkipBackwardEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsSkipBackwardButtonVisible"/>
        /// </summary>
        public bool IsSkipBackwardButtonVisible
        {
            get => UwpInstance.IsSkipBackwardButtonVisible;
            set => UwpInstance.IsSkipBackwardButtonVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsPreviousTrackButtonVisible"/>
        /// </summary>
        public bool IsPreviousTrackButtonVisible
        {
            get => UwpInstance.IsPreviousTrackButtonVisible;
            set => UwpInstance.IsPreviousTrackButtonVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsNextTrackButtonVisible"/>
        /// </summary>
        public bool IsNextTrackButtonVisible
        {
            get => UwpInstance.IsNextTrackButtonVisible;
            set => UwpInstance.IsNextTrackButtonVisible = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.FastPlayFallbackBehaviour"/>
        /// </summary>
        public Windows.UI.Xaml.Media.FastPlayFallbackBehaviour FastPlayFallbackBehaviour
        {
            get => UwpInstance.FastPlayFallbackBehaviour;
            set => UwpInstance.FastPlayFallbackBehaviour = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.ShowAndHideAutomatically"/>
        /// </summary>
        public bool ShowAndHideAutomatically
        {
            get => UwpInstance.ShowAndHideAutomatically;
            set => UwpInstance.ShowAndHideAutomatically = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsRepeatEnabled"/>
        /// </summary>
        public bool IsRepeatEnabled
        {
            get => UwpInstance.IsRepeatEnabled;
            set => UwpInstance.IsRepeatEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsRepeatButtonVisible"/>
        /// </summary>
        public bool IsRepeatButtonVisible
        {
            get => UwpInstance.IsRepeatButtonVisible;
            set => UwpInstance.IsRepeatButtonVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsCompactOverlayEnabled"/>
        /// </summary>
        public bool IsCompactOverlayEnabled
        {
            get => UwpInstance.IsCompactOverlayEnabled;
            set => UwpInstance.IsCompactOverlayEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.MediaTransportControls.IsCompactOverlayButtonVisible"/>
        /// </summary>
        public bool IsCompactOverlayButtonVisible
        {
            get => UwpInstance.IsCompactOverlayButtonVisible;
            set => UwpInstance.IsCompactOverlayButtonVisible = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.UI.Xaml.Controls.MediaTransportControls"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.MediaTransportControls"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.MediaTransportControls"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MediaTransportControls(
            Windows.UI.Xaml.Controls.MediaTransportControls args)
        {
            return FromMediaTransportControls(args);
        }

        /// <summary>
        /// Creates a <see cref="MediaTransportControls"/> from <see cref="Windows.UI.Xaml.Controls.MediaTransportControls"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.UI.Xaml.Controls.MediaTransportControls"/> instance containing the event data.</param>
        /// <returns><see cref="MediaTransportControls"/></returns>
        public static MediaTransportControls FromMediaTransportControls(Windows.UI.Xaml.Controls.MediaTransportControls args)
        {
            return new MediaTransportControls(args);
        }
    }
}