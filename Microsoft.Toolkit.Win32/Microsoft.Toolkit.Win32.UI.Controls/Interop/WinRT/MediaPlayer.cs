// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// <see cref="Windows.Media.Playback.MediaPlayer"/>
    /// </summary>
    public class MediaPlayer
    {
        internal Windows.Media.Playback.MediaPlayer UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlayer"/> class, a
        /// Wpf-enabled wrapper for <see cref="Windows.Media.Playback.MediaPlayer"/>
        /// </summary>
        public MediaPlayer(Windows.Media.Playback.MediaPlayer instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.Media.Playback.MediaPlayer.Volume"/>
        /// </summary>
        public double Volume
        {
            get => UwpInstance.Volume;
            set => UwpInstance.Volume = value;
        }

        /* OBSOLETE
        /// <summary>
        /// Gets or sets <see cref="Windows.Media.Playback.MediaPlayer.Position"/>
        /// </summary>
        public System.TimeSpan Position
        {
            get => UwpInstance.Position;
            set => UwpInstance.Position = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.Media.Playback.MediaPlayer.PlaybackRate"/>
        /// </summary>
        public double PlaybackRate
        {
            get => UwpInstance.PlaybackRate;
            set => UwpInstance.PlaybackRate = value;
        }
        */

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.Media.Playback.MediaPlayer.IsLoopingEnabled"/>
        /// </summary>
        public bool IsLoopingEnabled
        {
            get => UwpInstance.IsLoopingEnabled;
            set => UwpInstance.IsLoopingEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.Media.Playback.MediaPlayer.IsMuted"/>
        /// </summary>
        public bool IsMuted
        {
            get => UwpInstance.IsMuted;
            set => UwpInstance.IsMuted = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.Media.Playback.MediaPlayer.AutoPlay"/>
        /// </summary>
        public bool AutoPlay
        {
            get => UwpInstance.AutoPlay;
            set => UwpInstance.AutoPlay = value;
        }

        /*  OBSOLETE
        /// <summary>
        /// Gets <see cref="Windows.Media.Playback.MediaPlayer.CurrentState"/>
        /// </summary>
        public Windows.Media.Playback.MediaPlayerState CurrentState
        {
            get => UwpInstance.CurrentState;
        }

        /// <summary>
        /// Gets <see cref="Windows.Media.Playback.MediaPlayer.NaturalDuration"/>
        /// </summary>
        public System.TimeSpan NaturalDuration
        {
            get => UwpInstance.NaturalDuration;
        }

        /// <summary>
        /// Gets <see cref="Windows.Media.Playback.MediaPlayer.PlaybackMediaMarkers"/>
        /// </summary>
        public Windows.Media.Playback.PlaybackMediaMarkerSequence PlaybackMediaMarkers
        {
            get => UwpInstance.PlaybackMediaMarkers;
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Windows.Media.Playback.MediaPlayer.IsProtected"/>
        /// </summary>
        public bool IsProtected
        {
            get => UwpInstance.IsProtected;
        }

        /// <summary>
        /// Gets <see cref="Windows.Media.Playback.MediaPlayer.BufferingProgress"/>
        /// </summary>
        public double BufferingProgress
        {
            get => UwpInstance.BufferingProgress;
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Windows.Media.Playback.MediaPlayer.CanPause"/>
        /// </summary>
        public bool CanPause
        {
            get => UwpInstance.CanPause;
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Windows.Media.Playback.MediaPlayer.CanSeek"/>
        /// </summary>
        public bool CanSeek
        {
            get => UwpInstance.CanSeek;
        }
        */

        /// <summary>
        /// Gets or sets <see cref="Windows.Media.Playback.MediaPlayer.AudioDeviceType"/>
        /// </summary>
        public Windows.Media.Playback.MediaPlayerAudioDeviceType AudioDeviceType
        {
            get => UwpInstance.AudioDeviceType;
            set => UwpInstance.AudioDeviceType = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.Media.Playback.MediaPlayer.AudioCategory"/>
        /// </summary>
        public Windows.Media.Playback.MediaPlayerAudioCategory AudioCategory
        {
            get => UwpInstance.AudioCategory;
            set => UwpInstance.AudioCategory = value;
        }

        /// <summary>
        /// Gets <see cref="Windows.Media.Playback.MediaPlayer.SystemMediaTransportControls"/>
        /// </summary>
        public Windows.Media.SystemMediaTransportControls SystemMediaTransportControls
        {
            get => UwpInstance.SystemMediaTransportControls;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.Media.Playback.MediaPlayer.TimelineControllerPositionOffset"/>
        /// </summary>
        public System.TimeSpan TimelineControllerPositionOffset
        {
            get => UwpInstance.TimelineControllerPositionOffset;
            set => UwpInstance.TimelineControllerPositionOffset = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.Media.Playback.MediaPlayer.TimelineController"/>
        /// </summary>
        public Windows.Media.MediaTimelineController TimelineController
        {
            get => UwpInstance.TimelineController;
            set => UwpInstance.TimelineController = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.Media.Playback.MediaPlayer.StereoscopicVideoRenderMode"/>
        /// </summary>
        public Windows.Media.Playback.StereoscopicVideoRenderMode StereoscopicVideoRenderMode
        {
            get => UwpInstance.StereoscopicVideoRenderMode;
            set => UwpInstance.StereoscopicVideoRenderMode = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.Media.Playback.MediaPlayer.RealTimePlayback"/>
        /// </summary>
        public bool RealTimePlayback
        {
            get => UwpInstance.RealTimePlayback;
            set => UwpInstance.RealTimePlayback = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.Media.Playback.MediaPlayer.AudioDevice"/>
        /// </summary>
        public Windows.Devices.Enumeration.DeviceInformation AudioDevice
        {
            get => UwpInstance.AudioDevice;
            set => UwpInstance.AudioDevice = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.Media.Playback.MediaPlayer.AudioBalance"/>
        /// </summary>
        public double AudioBalance
        {
            get => UwpInstance.AudioBalance;
            set => UwpInstance.AudioBalance = value;
        }

        /// <summary>
        /// Gets <see cref="Windows.Media.Playback.MediaPlayer.CommandManager"/>
        /// </summary>
        public Windows.Media.Playback.MediaPlaybackCommandManager CommandManager
        {
            get => UwpInstance.CommandManager;
        }

        /// <summary>
        /// Gets <see cref="Windows.Media.Playback.MediaPlayer.BreakManager"/>
        /// </summary>
        public Windows.Media.Playback.MediaBreakManager BreakManager
        {
            get => UwpInstance.BreakManager;
        }

        /// <summary>
        /// Gets <see cref="Windows.Media.Playback.MediaPlayer.PlaybackSession"/>
        /// </summary>
        public Windows.Media.Playback.MediaPlaybackSession PlaybackSession
        {
            get => UwpInstance.PlaybackSession;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.Media.Playback.MediaPlayer.IsVideoFrameServerEnabled"/>
        /// </summary>
        public bool IsVideoFrameServerEnabled
        {
            get => UwpInstance.IsVideoFrameServerEnabled;
            set => UwpInstance.IsVideoFrameServerEnabled = value;
        }

        /// <summary>
        /// Gets <see cref="Windows.Media.Playback.MediaPlayer.AudioStateMonitor"/>
        /// </summary>
        public Windows.Media.Audio.AudioStateMonitor AudioStateMonitor
        {
            get => UwpInstance.AudioStateMonitor;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.Media.Playback.MediaPlayer.ProtectionManager"/>
        /// </summary>
        public Windows.Media.Protection.MediaProtectionManager ProtectionManager
        {
            get => UwpInstance.ProtectionManager;
            set => UwpInstance.ProtectionManager = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.Media.Playback.MediaPlayer.Source"/>
        /// </summary>
        public Windows.Media.Playback.IMediaPlaybackSource Source
        {
            get => UwpInstance.Source;
            set => UwpInstance.Source = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Media.Playback.MediaPlayer"/> to <see cref="MediaPlayer"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Media.Playback.MediaPlayer"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MediaPlayer(
            Windows.Media.Playback.MediaPlayer args)
        {
            return FromMediaPlayer(args);
        }

        /// <summary>
        /// Creates a <see cref="MediaPlayer"/> from <see cref="Windows.Media.Playback.MediaPlayer"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Media.Playback.MediaPlayer"/> instance containing the event data.</param>
        /// <returns><see cref="MediaPlayer"/></returns>
        public static MediaPlayer FromMediaPlayer(Windows.Media.Playback.MediaPlayer args)
        {
            return new MediaPlayer(args);
        }
    }
}