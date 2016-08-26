using System;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="Loading"/> class.
        /// </summary>
        public Loading()
        {
            LoadingVerticalAlignment = VerticalAlignment.Stretch;
            LoadingHorizontalAlignment = HorizontalAlignment.Stretch;

            RootGrid = new Grid();
            BackgroundGrid = new Grid();
            ContentGrid = new Grid();
            RootGrid.Children.Add(BackgroundGrid);
            RootGrid.Children.Add(ContentGrid);
            Content = RootGrid;
        }

        private void CreateLoadingControl()
        {
            if (IsLoading)
            {
                ContentGrid.Children.Clear();
                if (LoadingBackground == null && LoadingOpacity == 0d)
                {
                    BackgroundGrid = null;
                }
                else
                {
                    BackgroundGrid.Background = LoadingBackground;
                    BackgroundGrid.Opacity = LoadingOpacity;
                }

                CreateStoryboard(translateBegin: 40d, translateEnd: 0d, opacityBegin: 0d, opacityEnd: 1d);
                Animation.Begin();

                var contentControl = LoadingContent?.LoadContent() as FrameworkElement;
                if (contentControl == null)
                {
                    return;
                }

                contentControl.HorizontalAlignment = LoadingHorizontalAlignment;
                contentControl.VerticalAlignment = LoadingVerticalAlignment;

                ContentGrid.Children.Add(contentControl);
            }
            else
            {
                CreateStoryboard(translateBegin: 0d, translateEnd: 40d, opacityBegin: 1d, opacityEnd: 0d);
                Animation.Begin();
            }
        }

        private void CreateStoryboard(double translateBegin, double translateEnd, double opacityBegin, double opacityEnd)
        {
            Animation = new Storyboard();
            ContentGrid.RenderTransform = new CompositeTransform();
            var scaleYAnimation = new DoubleAnimationUsingKeyFrames();
            var opacityAnimation = new DoubleAnimationUsingKeyFrames();
            var visibilityAnimation = new ObjectAnimationUsingKeyFrames();

            var scaleFrame1 = new EasingDoubleKeyFrame
            {
                EasingFunction = new QuadraticEase
                {
                    EasingMode = EasingMode.EaseInOut
                },
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)),
                Value = translateBegin
            };

            var scaleFrame2 = new EasingDoubleKeyFrame
            {
                EasingFunction = new QuadraticEase
                {
                    EasingMode = EasingMode.EaseInOut
                },
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3)),
                Value = translateEnd
            };

            var opacityFrame1 = new EasingDoubleKeyFrame
            {
                EasingFunction = new QuadraticEase
                {
                    EasingMode = EasingMode.EaseInOut
                },
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)),
                Value = opacityBegin
            };

            var opacityFrame2 = new EasingDoubleKeyFrame
            {
                EasingFunction = new QuadraticEase
                {
                    EasingMode = EasingMode.EaseInOut
                },
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2)),
                Value = opacityEnd
            };

            var visibilityFrame = new DiscreteObjectKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)),
                Value = Visibility.Visible
            };

            var visibilityFrameEnd = new DiscreteObjectKeyFrame();
            if (!IsLoading)
            {
                visibilityFrameEnd = new DiscreteObjectKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3)),
                    Value = Visibility.Collapsed
                };
            }

            scaleYAnimation.KeyFrames.Add(scaleFrame1);
            scaleYAnimation.KeyFrames.Add(scaleFrame2);

            opacityAnimation.KeyFrames.Add(opacityFrame1);
            opacityAnimation.KeyFrames.Add(opacityFrame2);

            visibilityAnimation.KeyFrames.Add(visibilityFrame);
            if (!IsLoading)
            {
                visibilityAnimation.KeyFrames.Add(visibilityFrameEnd);
            }

            Storyboard.SetTargetProperty(scaleYAnimation, "(ContentGrid.RenderTransform).(CompositeTransform.TranslateY)");
            Storyboard.SetTargetProperty(opacityAnimation, "(RootGrid.Opacity)");
            Storyboard.SetTargetProperty(visibilityAnimation, "(this.Visibility)");

            Storyboard.SetTarget(scaleYAnimation, ContentGrid);
            Storyboard.SetTarget(opacityAnimation, RootGrid);
            Storyboard.SetTarget(visibilityAnimation, this);

            Animation.Children.Add(scaleYAnimation);
            Animation.Children.Add(opacityAnimation);
            Animation.Children.Add(visibilityAnimation);
        }
    }
}
