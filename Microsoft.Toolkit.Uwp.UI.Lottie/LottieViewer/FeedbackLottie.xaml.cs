// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace LottieViewer
{
    public sealed partial class FeedbackLottie : UserControl
    {
        // Shrinks from the expanded JSON file back to the initial size.
        static readonly CompositionSegment ShrinkToInitial =
            new CompositionSegment("ShrinkToInitial", 0.007, 0.1188811, playbackRate: -1, isLoopingEnabled: false);

        // Expands from the initial state to a large JSON file.
        static readonly CompositionSegment ExpandFromInitial = new CompositionSegment("ExpandFromInitial", 0.007, 0.1188811);

        // A loop where the JSON file looks excited about being dropped.
        static readonly CompositionSegment ExcitedDropLoop =
            new CompositionSegment("ExcitedDropLoop", 0.1188811, 0.3426574, playbackRate: 1, isLoopingEnabled: true);

        // Follows on from ExcitedDropLoop.
        static readonly CompositionSegment ExcitedResolution = new CompositionSegment("ExcitedResolution", 0.3426574, 0.489);

        // The explosion at the end of loading.
        static readonly CompositionSegment FinishLoading = new CompositionSegment("FinishLoading", 0.6923078, 1);

        static readonly CompositionSegment FailLoading = new CompositionSegment("FailLoading", 0.4895105, 0.69 /* 0.6923077 */);


        Task _currentPlay = Task.CompletedTask;
        DragNDropHintState _dragNDropHintState = DragNDropHintState.Initial;

        public FeedbackLottie()
        {
            this.InitializeComponent();
        }


        internal void PlayInitialStateAnimation()
        {
            switch (_dragNDropHintState)
            {
                case DragNDropHintState.Failed:
                case DragNDropHintState.Finished:
                case DragNDropHintState.Initial:
                    break;

                case DragNDropHintState.Disabled:
                case DragNDropHintState.Encouraging:
                case DragNDropHintState.Shrinking:
                default:
                    return;
            }

            EnsureVisible();
            _dragNDropHintState = DragNDropHintState.Initial;
            _dragNDropHint.SetProgress(0.007);
        }

        internal async void PlayDragEnterAnimation()
        {
            EnsureVisible();
            if (_dragNDropHintState == DragNDropHintState.Initial ||
                _dragNDropHintState == DragNDropHintState.Shrinking)
            {
                _dragNDropHintState = DragNDropHintState.Encouraging;
                await PlaySegment(ExpandFromInitial);
                await PlaySegment(ExcitedDropLoop);
            }
        }

        internal async Task PlayDroppedAnimation()
        {
            EnsureVisible();
            if (_dragNDropHintState == DragNDropHintState.Encouraging)
            {
                await PlaySegment(ExcitedResolution);
                if (_dragNDropHintState != DragNDropHintState.Encouraging)
                {
                    return;
                }
            }

            _dragNDropHintState = DragNDropHintState.Finished;

            // Fade out. This is only necessary for RS4 builds that
            // do not handle 0-size strokes correctly, leaving crud on
            // the screen.
            _fadeOutStoryboard.Begin();

            await PlaySegment(FinishLoading);
        }


        internal void PlayDragLeaveAnimation()
        {
            EnsureVisible();
            if (_dragNDropHintState == DragNDropHintState.Encouraging)
            {
                _dragNDropHintState = DragNDropHintState.Shrinking;
                PlaySegment(ShrinkToInitial);
            }
        }

        internal async Task PlayLoadFailedAnimation()
        {
            EnsureVisible();
            _dragNDropHintState = DragNDropHintState.Failed;
            await PlaySegment(FailLoading);
            _dragNDropHintState = DragNDropHintState.Initial;
            _dragNDropHint.SetProgress(FailLoading.ToProgress);
        }

        Task PlaySegment(CompositionSegment segment)
        {
            _dragNDropHint.PlaybackRate = segment.PlaybackRate;

            return _currentPlay = _dragNDropHint.PlayAsync(
                fromProgress: segment.FromProgress,
                toProgress: segment.ToProgress,
                looped: segment.IsLoopingEnabled).AsTask();
        }

        void EnsureVisible()
        {
            Debug.WriteLine("Stopping opacity animation");
            _fadeOutStoryboard.Stop();
            _dragNDropHint.Opacity = 1;
        }

        enum DragNDropHintState
        {
            Disabled,
            Initial,
            Encouraging,
            Finished,
            Failed,
            Shrinking,
        }

        /// <summary>
        /// Defines a segment of a composition that can be played by the <see cref="AnimatedVisualPlayer"/>.
        /// </summary>
        sealed class CompositionSegment
        {
            public double FromProgress { get; }
            public double ToProgress { get; }
            public double PlaybackRate { get; }
            public bool IsLoopingEnabled { get; }

            public string Name { get; }
            public CompositionSegment(string name, double fromProgress, double toProgress, double playbackRate, bool isLoopingEnabled)
            {
                Name = name;
                FromProgress = fromProgress;
                ToProgress = toProgress;
                PlaybackRate = playbackRate;
                IsLoopingEnabled = isLoopingEnabled;
            }

            /// <summary>
            /// Defines a segment that plays from <paramref name="fromProgress"/> to <paramref name="toProgress"/>
            /// without looping or repeating.
            /// </summary>
            public CompositionSegment(string name, double fromProgress, double toProgress)
                : this(name, fromProgress, toProgress, playbackRate: 1, isLoopingEnabled: false)
            {
            }
        }
    }
}
