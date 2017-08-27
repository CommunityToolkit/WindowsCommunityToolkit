// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// In App Notification defines a control to show local notification in the app.
    /// </summary>
    public partial class InAppNotification
    {
        /// <summary>
        /// Identifies the <see cref="ShowDismissButton"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowDismissButtonProperty =
            DependencyProperty.Register(nameof(ShowDismissButton), typeof(bool), typeof(InAppNotification), new PropertyMetadata(true, OnShowDismissButtonChanged));

        /// <summary>
        /// Identifies the <see cref="AnimationDuration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register(nameof(AnimationDuration), typeof(int), typeof(InAppNotification), new PropertyMetadata(100, OnAnimationDurationChanged));

        /// <summary>
        /// Identifies the <see cref="VerticalOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register(nameof(VerticalOffset), typeof(double), typeof(InAppNotification), new PropertyMetadata(100, OnVerticalOffsetChanged));

        /// <summary>
        /// Identifies the <see cref="HorizontalOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.Register(nameof(HorizontalOffset), typeof(double), typeof(InAppNotification), new PropertyMetadata(0, OnHorizontalOffsetChanged));

        /// <summary>
        /// Gets or sets a value indicating whether to show the Dismiss button of the control.
        /// </summary>
        public bool ShowDismissButton
        {
            get { return (bool)GetValue(ShowDismissButtonProperty); }
            set { SetValue(ShowDismissButtonProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating the duration of the popup animation (in milliseconds).
        /// </summary>
        public int AnimationDuration
        {
            get { return (int)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating the vertical offset of the popup animation.
        /// </summary>
        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating the horizontal offset of the popup animation.
        /// </summary>
        public double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        private static void OnShowDismissButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var inApNotification = d as InAppNotification;

            if (inApNotification._dismissButton != null)
            {
                bool showDismissButton = (bool)e.NewValue;
                inApNotification._dismissButton.Visibility = showDismissButton ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private static void OnAnimationDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var inApNotification = d as InAppNotification;

            if (inApNotification._visualStateGroup != null)
            {
                int duration = (int)e.NewValue;
                var keyTimeFromAnimationDuration = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(duration));

                foreach (var state in inApNotification._visualStateGroup.States)
                {
                    foreach (var timeline in state.Storyboard.Children)
                    {
                        if (timeline is DoubleAnimationUsingKeyFrames daukf)
                        {
                            var keyFramesCount = daukf.KeyFrames.Count;
                            if (keyFramesCount > 1)
                            {
                                daukf.KeyFrames[keyFramesCount - 1].KeyTime = keyTimeFromAnimationDuration;
                            }
                        }

                        if (timeline is ObjectAnimationUsingKeyFrames oaukf)
                        {
                            var keyFramesCount = oaukf.KeyFrames.Count;
                            if (keyFramesCount > 1)
                            {
                                oaukf.KeyFrames[keyFramesCount - 1].KeyTime = keyTimeFromAnimationDuration;
                            }
                        }
                    }
                }
            }
        }

        private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var inApNotification = d as InAppNotification;

            if (inApNotification._visualStateGroup != null)
            {
                double verticalOffset = (double)e.NewValue;

                foreach (var state in inApNotification._visualStateGroup.States)
                {
                    foreach (var timeline in state.Storyboard.Children)
                    {
                        if (timeline is DoubleAnimationUsingKeyFrames daukf)
                        {
                            var targetProperty = (string)timeline.GetValue(Storyboard.TargetPropertyProperty);

                            if (targetProperty == "(UIElement.RenderTransform).(CompositeTransform.TranslateY)")
                            {
                                var keyFramesCount = daukf.KeyFrames.Count;

                                if (keyFramesCount > 1)
                                {
                                    if (state.Name == "Visible")
                                    {
                                        daukf.KeyFrames[0].Value = verticalOffset;
                                    }

                                    if (state.Name == "Collapsed")
                                    {
                                        daukf.KeyFrames[keyFramesCount - 1].Value = verticalOffset;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var inApNotification = d as InAppNotification;

            if (inApNotification._visualStateGroup != null)
            {
                double horizontalOffset = (double)e.NewValue;

                foreach (var state in inApNotification._visualStateGroup.States)
                {
                    foreach (var timeline in state.Storyboard.Children)
                    {
                        if (timeline is DoubleAnimationUsingKeyFrames daukf)
                        {
                            var targetProperty = (string)timeline.GetValue(Storyboard.TargetPropertyProperty);

                            if (targetProperty == "(UIElement.RenderTransform).(CompositeTransform.TranslateX)")
                            {
                                var keyFramesCount = daukf.KeyFrames.Count;

                                if (keyFramesCount > 1)
                                {
                                    if (state.Name == "Visible")
                                    {
                                        daukf.KeyFrames[0].Value = horizontalOffset;
                                    }

                                    if (state.Name == "Collapsed")
                                    {
                                        daukf.KeyFrames[keyFramesCount - 1].Value = horizontalOffset;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
