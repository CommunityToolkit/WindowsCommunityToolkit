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
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// ContentControl providing functionality for sliding left or right to expose functions
    /// </summary>
    [TemplatePart(Name = PartContentGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PartCommandContainer, Type = typeof(Grid))]
    [TemplatePart(Name = PartLeftCommandPanel, Type = typeof(StackPanel))]
    [TemplatePart(Name = PartRightCommandPanel, Type = typeof(StackPanel))]
    public class SlidableListItem : ContentControl
    {
        /// <summary>
        /// Identifies the <see cref="ExtraSwipeThreshold"/> property
        /// </summary>
        public static readonly DependencyProperty ExtraSwipeThresholdProperty =
            DependencyProperty.Register(nameof(ExtraSwipeThreshold), typeof(int), typeof(SlidableListItem), new PropertyMetadata(default(int)));

        /// <summary>
        /// Identifies the <see cref="IsOffsetLimited"/> property
        /// </summary>
        public static readonly DependencyProperty IsOffsetLimitedProperty =
            DependencyProperty.Register(nameof(IsOffsetLimited), typeof(bool), typeof(SlidableListItem), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="IsRightCommandEnabled"/> property
        /// </summary>
        public static readonly DependencyProperty IsRightCommandEnabledProperty =
            DependencyProperty.Register(nameof(IsRightCommandEnabled), typeof(bool), typeof(SlidableListItem), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="IsLeftCommandEnabled"/> property
        /// </summary>
        public static readonly DependencyProperty IsLeftCommandEnabledProperty =
            DependencyProperty.Register(nameof(IsLeftCommandEnabled), typeof(bool), typeof(SlidableListItem), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="ActivationWidth"/> property
        /// </summary>
        public static readonly DependencyProperty ActivationWidthProperty =
            DependencyProperty.Register(nameof(ActivationWidth), typeof(double), typeof(SlidableListItem), new PropertyMetadata(80));

        /// <summary>
        /// Indeifies the <see cref="LeftIcon"/> property
        /// </summary>
        public static readonly DependencyProperty LeftIconProperty =
            DependencyProperty.Register(nameof(LeftIcon), typeof(Symbol), typeof(SlidableListItem), new PropertyMetadata(Symbol.Favorite));

        /// <summary>
        /// Identifies the <see cref="RightIcon"/> property
        /// </summary>
        public static readonly DependencyProperty RightIconProperty =
            DependencyProperty.Register(nameof(RightIcon), typeof(Symbol), typeof(SlidableListItem), new PropertyMetadata(Symbol.Delete));

        /// <summary>
        /// Identifies the <see cref="LeftLabel"/> property
        /// </summary>
        public static readonly DependencyProperty LeftLabelProperty =
            DependencyProperty.Register(nameof(LeftLabel), typeof(string), typeof(SlidableListItem), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the <see cref="RightLabel"/> property
        /// </summary>
        public static readonly DependencyProperty RightLabelProperty =
            DependencyProperty.Register(nameof(RightLabel), typeof(string), typeof(SlidableListItem), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the <see cref="LeftForeground"/> property
        /// </summary>
        public static readonly DependencyProperty LeftForegroundProperty =
            DependencyProperty.Register(nameof(LeftForeground), typeof(Brush), typeof(SlidableListItem), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        /// <summary>
        /// Identifies the <see cref="RightForeground"/> property
        /// </summary>
        public static readonly DependencyProperty RightForegroundProperty =
            DependencyProperty.Register(nameof(RightForeground), typeof(Brush), typeof(SlidableListItem), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        /// <summary>
        /// Identifies the <see cref="LeftBackground"/> property
        /// </summary>
        public static readonly DependencyProperty LeftBackgroundProperty =
            DependencyProperty.Register(nameof(LeftBackground), typeof(Brush), typeof(SlidableListItem), new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));

        /// <summary>
        /// Identifies the <see cref="RightBackground"/> property
        /// </summary>
        public static readonly DependencyProperty RightBackgroundProperty =
            DependencyProperty.Register(nameof(RightBackground), typeof(Brush), typeof(SlidableListItem), new PropertyMetadata(new SolidColorBrush(Colors.DarkGray)));

        /// <summary>
        /// Identifies the <see cref="MouseSlidingEnabled"/> property
        /// </summary>
        public static readonly DependencyProperty MouseSlidingEnabledProperty =
            DependencyProperty.Register(nameof(MouseSlidingEnabled), typeof(bool), typeof(SlidableListItem), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="LeftCommand"/> property
        /// </summary>
        public static readonly DependencyProperty LeftCommandProperty =
            DependencyProperty.Register(nameof(LeftCommand), typeof(ICommand), typeof(SlidableListItem), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="RightCommand"/> property
        /// </summary>
        public static readonly DependencyProperty RightCommandProperty =
            DependencyProperty.Register(nameof(RightCommand), typeof(ICommand), typeof(SlidableListItem), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="LeftCommandParameter"/> property
        /// </summary>
        public static readonly DependencyProperty LeftCommandParameterProperty =
            DependencyProperty.Register(nameof(LeftCommandParameter), typeof(object), typeof(SlidableListItem), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="RightCommandParameter"/> property
        /// </summary>
        public static readonly DependencyProperty RightCommandParameterProperty =
            DependencyProperty.Register(nameof(RightCommandParameter), typeof(object), typeof(SlidableListItem), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SwipeStatus"/> property
        /// </summary>
        public static readonly DependencyProperty SwipeStatusProperty =
            DependencyProperty.Register(nameof(SwipeStatus), typeof(object), typeof(SlidableListItem), new PropertyMetadata(SwipeStatus.Idle));

        /// <summary>
        /// Identifues the <see cref="IsPointerReleasedOnSwipingHandled"/> property
        /// </summary>
        public static readonly DependencyProperty IsPointerReleasedOnSwipingHandledProperty =
            DependencyProperty.Register("IsPointerReleasedOnSwipingHandled", typeof(bool), typeof(SlidableListItem), new PropertyMetadata(false));

        /// <summary>
        /// Occurs when SwipeStatus has changed
        /// </summary>
        public event TypedEventHandler<SlidableListItem, SwipeStatusChangedEventArgs> SwipeStatusChanged;

        private const string PartContentGrid = "ContentGrid";
        private const string PartCommandContainer = "CommandContainer";
        private const string PartLeftCommandPanel = "LeftCommandPanel";
        private const string PartRightCommandPanel = "RightCommandPanel";
        private const int FinishAnimationDuration = 150;
        private const int SnappedCommandMargin = 20;
        private const int AnimationSetDuration = 200;
        private Grid _contentGrid;
        private CompositeTransform _transform;
        private Grid _commandContainer;
        private CompositeTransform _commandContainerTransform;
        private DoubleAnimation _commandContainerClipTranslateAnimation;
        private StackPanel _leftCommandPanel;
        private CompositeTransform _leftCommandTransform;
        private StackPanel _rightCommandPanel;
        private CompositeTransform _rightCommandTransform;
        private DoubleAnimation _contentAnimation;
        private Storyboard _contentStoryboard;
        private AnimationSet _leftCommandAnimationSet;
        private AnimationSet _rightCommandAnimationSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="SlidableListItem"/> class.
        /// Creates a new instance of <see cref="SlidableListItem"/>
        /// </summary>
        public SlidableListItem()
        {
            DefaultStyleKey = typeof(SlidableListItem);
        }

        /// <summary>
        /// Occurs when the user swipes to the left to activate the right action
        /// </summary>
        public event EventHandler RightCommandRequested;

        /// <summary>
        /// Occurs when the user swipes to the right to activate the left action
        /// </summary>
        public event EventHandler LeftCommandRequested;

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding
        /// layout pass) call <see cref="OnApplyTemplate"/>. In simplest terms, this means the method
        /// is called just before a UI element displays in an application. Override this
        /// method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            if (_contentGrid != null)
            {
                _contentGrid.PointerReleased -= ContentGrid_PointerReleased;
                _contentGrid.ManipulationStarted -= ContentGrid_ManipulationStarted;
                _contentGrid.ManipulationDelta -= ContentGrid_ManipulationDelta;
                _contentGrid.ManipulationCompleted -= ContentGrid_ManipulationCompleted;
            }

            _contentGrid = GetTemplateChild(PartContentGrid) as Grid;

            if (_contentGrid != null)
            {
                _contentGrid.PointerReleased += ContentGrid_PointerReleased;

                _transform = _contentGrid.RenderTransform as CompositeTransform;
                _contentGrid.ManipulationStarted += ContentGrid_ManipulationStarted;
                _contentGrid.ManipulationDelta += ContentGrid_ManipulationDelta;
                _contentGrid.ManipulationCompleted += ContentGrid_ManipulationCompleted;
            }

            Loaded += SlidableListItem_Loaded;
            Unloaded += SlidableListItem_Unloaded;

            base.OnApplyTemplate();
        }

        private void SlidableListItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (_contentStoryboard != null)
            {
                _contentStoryboard.Completed += ContentStoryboard_Completed;
            }
        }

        private void SlidableListItem_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_contentStoryboard != null)
            {
                _contentStoryboard.Completed -= ContentStoryboard_Completed;
            }
        }

        private void ContentGrid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (SwipeStatus != SwipeStatus.Idle && IsPointerReleasedOnSwipingHandled)
            {
                e.Handled = true;
            }
        }

        private void ContentGrid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            if ((!MouseSlidingEnabled && e.PointerDeviceType == PointerDeviceType.Mouse) || (!IsLeftCommandEnabled && !IsRightCommandEnabled))
            {
                return;
            }

            if (_contentStoryboard == null)
            {
                _contentAnimation = new DoubleAnimation();
                Storyboard.SetTarget(_contentAnimation, _transform);
                Storyboard.SetTargetProperty(_contentAnimation, "TranslateX");
                _contentAnimation.To = 0;
                _contentAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(FinishAnimationDuration));

                _contentStoryboard = new Storyboard();
                _contentStoryboard.Children.Add(_contentAnimation);

                _contentStoryboard.Completed += ContentStoryboard_Completed;
            }

            if (_commandContainer == null)
            {
                _commandContainer = GetTemplateChild(PartCommandContainer) as Grid;
                if (_commandContainer != null)
                {
                    _commandContainer.Background = LeftBackground as SolidColorBrush;
                    _commandContainer.Clip = new RectangleGeometry();
                    _commandContainerTransform = new CompositeTransform();
                    _commandContainer.Clip.Transform = _commandContainerTransform;

                    _commandContainerClipTranslateAnimation = new DoubleAnimation();
                    Storyboard.SetTarget(_commandContainerClipTranslateAnimation, _commandContainerTransform);
                    Storyboard.SetTargetProperty(_commandContainerClipTranslateAnimation, "TranslateX");
                    _commandContainerClipTranslateAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(FinishAnimationDuration));
                    _contentStoryboard.Children.Add(_commandContainerClipTranslateAnimation);
                }
            }

            if (_leftCommandPanel == null)
            {
                _leftCommandPanel = GetTemplateChild(PartLeftCommandPanel) as StackPanel;
                if (_leftCommandPanel != null)
                {
                    _leftCommandTransform = _leftCommandPanel.RenderTransform as CompositeTransform;
                }
            }

            if (_rightCommandPanel == null)
            {
                _rightCommandPanel = GetTemplateChild(PartRightCommandPanel) as StackPanel;
                if (_rightCommandPanel != null)
                {
                    _rightCommandTransform = _rightCommandPanel.RenderTransform as CompositeTransform;
                }
            }

            _contentStoryboard.Stop();
            _commandContainer.Opacity = 0;
            _commandContainerTransform.TranslateX = 0;
            _transform.TranslateX = 0;
            SwipeStatus = SwipeStatus.Starting;
        }

        /// <summary>
        /// Handler for when slide manipulation is complete
        /// </summary>
        private void ContentGrid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (SwipeStatus == SwipeStatus.Idle)
            {
                return;
            }

            var x = _transform.TranslateX;
            _contentAnimation.From = x;
            _commandContainerClipTranslateAnimation.From = 0;
            _commandContainerClipTranslateAnimation.To = -x;
            _contentStoryboard.Begin();

            if (SwipeStatus == SwipeStatus.SwipingPassedLeftThreshold)
            {
                RightCommandRequested?.Invoke(this, EventArgs.Empty);
                RightCommand?.Execute(RightCommandParameter);
            }
            else if (SwipeStatus == SwipeStatus.SwipingPassedRightThreshold)
            {
                LeftCommandRequested?.Invoke(this, EventArgs.Empty);
                LeftCommand?.Execute(LeftCommandParameter);
            }

            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { SwipeStatus = SwipeStatus.Idle; }).AsTask();
        }

        /// <summary>
        /// Handler for when slide manipulation is underway
        /// </summary>
        private void ContentGrid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (SwipeStatus == SwipeStatus.Idle)
            {
                return;
            }

            var newTranslationX = _transform.TranslateX + e.Delta.Translation.X;
            bool swipingInDisabledArea = false;
            SwipeStatus newSwipeStatus = SwipeStatus.Idle;

            if (newTranslationX > 0)
            {
                // Swiping from left to right
                if (!IsLeftCommandEnabled)
                {
                    // If swipe is not enabled, only allow swipe a very short distance
                    if (newTranslationX > 0)
                    {
                        swipingInDisabledArea = true;
                        newSwipeStatus = SwipeStatus.DisabledSwipingToRight;
                    }

                    if (newTranslationX > 16)
                    {
                        newTranslationX = 16;
                    }
                }
                else if (IsOffsetLimited)
                {
                    // If offset is limited, there will be a limit how much swipe is possible.
                    // This will be the value of the command panel plus some threshold.
                    // This value can't be less than the ActivationWidth.
                    var swipeThreshold = _leftCommandPanel.ActualWidth + ExtraSwipeThreshold;
                    if (swipeThreshold < ActivationWidth)
                    {
                        swipeThreshold = ActivationWidth;
                    }

                    if (Math.Abs(newTranslationX) > swipeThreshold)
                    {
                        newTranslationX = swipeThreshold;
                    }
                }

                // Don't allow swiping more than almost the whole content grid width
                // (doing this will cause the control to change size).
                if (newTranslationX > (_contentGrid.ActualWidth - 4))
                {
                    newTranslationX = _contentGrid.ActualWidth - 4;
                }
            }
            else
            {
                // Swiping from right to left
                if (!IsRightCommandEnabled)
                {
                    // If swipe is not enabled, only allow swipe a very short distance
                    if (newTranslationX < 0)
                    {
                        swipingInDisabledArea = true;
                        newSwipeStatus = SwipeStatus.DisabledSwipingToLeft;
                    }

                    if (newTranslationX < -16)
                    {
                        newTranslationX = -16;
                    }
                }
                else if (IsOffsetLimited)
                {
                    // If offset is limited, there will be a limit how much swipe is possible.
                    // This will be the value of the command panel plus some threshold.
                    // This value can't be less than the ActivationWidth.
                    var swipeThreshold = _rightCommandPanel.ActualWidth + ExtraSwipeThreshold;
                    if (swipeThreshold < ActivationWidth)
                    {
                        swipeThreshold = ActivationWidth;
                    }

                    if (Math.Abs(newTranslationX) > swipeThreshold)
                    {
                        newTranslationX = -swipeThreshold;
                    }
                }

                // Don't allow swiping more than almost the whole content grid width
                // (doing this will cause the control to change size).
                if (newTranslationX < -(_contentGrid.ActualWidth - 4))
                {
                    newTranslationX = -(_contentGrid.ActualWidth - 4);
                }
            }

            bool hasPassedThreshold = !swipingInDisabledArea && Math.Abs(newTranslationX) >= ActivationWidth;

            if (swipingInDisabledArea)
            {
                // Don't show any command if swiping in disabled area.
                _commandContainer.Opacity = 0;
                _leftCommandPanel.Opacity = 0;
                _rightCommandPanel.Opacity = 0;
            }
            else if (newTranslationX > 0)
            {
                // If swiping from left to right, show left command panel.
                _rightCommandPanel.Opacity = 0;

                _commandContainer.Background = LeftBackground as SolidColorBrush;
                _commandContainer.Opacity = 1;
                _leftCommandPanel.Opacity = 1;

                _commandContainer.Clip.Rect = new Rect(0, 0, Math.Max(newTranslationX - 1, 0), _commandContainer.ActualHeight);

                if (newTranslationX < ActivationWidth)
                {
                    _leftCommandAnimationSet?.Stop();
                    _leftCommandPanel.RenderTransform = _leftCommandTransform;
                    _leftCommandTransform.TranslateX = newTranslationX / 2;

                    newSwipeStatus = SwipeStatus.SwipingToRightThreshold;
                }
                else
                {
                    if (SwipeStatus == SwipeStatus.SwipingToRightThreshold)
                    {
                        // The control was just put below the threshold.
                        // Run an animation to put the text and icon
                        // in the correct position.
                        _leftCommandAnimationSet = _leftCommandPanel.Offset((float)(SnappedCommandMargin - _leftCommandTransform.TranslateX), duration: AnimationSetDuration);
                        _leftCommandAnimationSet.Start();
                    }
                    else if (SwipeStatus != SwipeStatus.SwipingPassedRightThreshold)
                    {
                        // This will cover extrem cases when previous state wasn't
                        // below threshold.
                        _leftCommandAnimationSet?.Stop();
                        _leftCommandPanel.RenderTransform = _leftCommandTransform;
                        _leftCommandTransform.TranslateX = SnappedCommandMargin;
                    }

                    newSwipeStatus = SwipeStatus.SwipingPassedRightThreshold;
                }
            }
            else
            {
                // If swiping from right to left, show right command panel.
                _leftCommandPanel.Opacity = 0;

                _commandContainer.Background = RightBackground as SolidColorBrush;
                _commandContainer.Opacity = 1;
                _rightCommandPanel.Opacity = 1;

                _commandContainer.Clip.Rect = new Rect(_commandContainer.ActualWidth + newTranslationX + 1, 0, Math.Max(-newTranslationX - 1, 0), _commandContainer.ActualHeight);

                if (-newTranslationX < ActivationWidth)
                {
                    _rightCommandAnimationSet?.Stop();
                    _rightCommandPanel.RenderTransform = _rightCommandTransform;
                    _rightCommandTransform.TranslateX = newTranslationX / 2;

                    newSwipeStatus = SwipeStatus.SwipingToLeftThreshold;
                }
                else
                {
                    if (SwipeStatus == SwipeStatus.SwipingToLeftThreshold)
                    {
                        // The control was just put below the threshold.
                        // Run an animation to put the text and icon
                        // in the correct position.
                        _rightCommandAnimationSet = _rightCommandPanel.Offset((float)(-SnappedCommandMargin - _rightCommandTransform.TranslateX), duration: AnimationSetDuration);
                        _rightCommandAnimationSet.Start();
                    }
                    else if (SwipeStatus != SwipeStatus.SwipingPassedLeftThreshold)
                    {
                        // This will cover extrem cases when previous state wasn't
                        // below threshold.
                        _rightCommandAnimationSet?.Stop();
                        _rightCommandPanel.RenderTransform = _rightCommandTransform;
                        _rightCommandTransform.TranslateX = -SnappedCommandMargin;
                    }

                    newSwipeStatus = SwipeStatus.SwipingPassedLeftThreshold;
                }
            }

            _transform.TranslateX = newTranslationX;
            SwipeStatus = newSwipeStatus;
        }

        private void ContentStoryboard_Completed(object sender, object e)
        {
            _commandContainer.Opacity = 0.0;
        }

        /// <summary>
        /// Gets or sets the amount of extra pixels for swipe threshold when <see cref="IsOffsetLimited"/> is enabled.
        /// </summary>
        public int ExtraSwipeThreshold
        {
            get { return (int)GetValue(ExtraSwipeThresholdProperty); }
            set { SetValue(ExtraSwipeThresholdProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether maximum swipe offset is limited or not.
        /// </summary>
        public bool IsOffsetLimited
        {
            get { return (bool)GetValue(IsOffsetLimitedProperty); }
            set { SetValue(IsOffsetLimitedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether right command is enabled or not.
        /// </summary>
        public bool IsRightCommandEnabled
        {
            get { return (bool)GetValue(IsRightCommandEnabledProperty); }
            set { SetValue(IsRightCommandEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether left command is enabled or not.
        /// </summary>
        public bool IsLeftCommandEnabled
        {
            get { return (bool)GetValue(IsLeftCommandEnabledProperty); }
            set { SetValue(IsLeftCommandEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the amount of pixels the content needs to be swiped for an
        /// action to be requested
        /// </summary>
        public double ActivationWidth
        {
            get { return (double)GetValue(ActivationWidthProperty); }
            set { SetValue(ActivationWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the left icon symbol
        /// </summary>
        public Symbol LeftIcon
        {
            get { return (Symbol)GetValue(LeftIconProperty); }
            set { SetValue(LeftIconProperty, value); }
        }

        /// <summary>
        /// Gets or sets the right icon symbol
        /// </summary>
        public Symbol RightIcon
        {
            get { return (Symbol)GetValue(RightIconProperty); }
            set { SetValue(RightIconProperty, value); }
        }

        /// <summary>
        /// Gets or sets the left label
        /// </summary>
        public string LeftLabel
        {
            get { return (string)GetValue(LeftLabelProperty); }
            set { SetValue(LeftLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the right label
        /// </summary>
        public string RightLabel
        {
            get { return (string)GetValue(RightLabelProperty); }
            set { SetValue(RightLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the left foreground color
        /// </summary>
        public Brush LeftForeground
        {
            get { return (Brush)GetValue(LeftForegroundProperty); }
            set { SetValue(LeftForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the right foreground color
        /// </summary>
        public Brush RightForeground
        {
            get { return (Brush)GetValue(RightForegroundProperty); }
            set { SetValue(RightForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the left background color
        /// </summary>
        public Brush LeftBackground
        {
            get { return (Brush)GetValue(LeftBackgroundProperty); }
            set { SetValue(LeftBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the right background color
        /// </summary>
        public Brush RightBackground
        {
            get { return (Brush)GetValue(RightBackgroundProperty); }
            set { SetValue(RightBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether it has the ability to slide the control with the mouse. False by default
        /// </summary>
        public bool MouseSlidingEnabled
        {
            get { return (bool)GetValue(MouseSlidingEnabledProperty); }
            set { SetValue(MouseSlidingEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ICommand for left command request
        /// </summary>
        public ICommand LeftCommand
        {
            get
            {
                return (ICommand)GetValue(LeftCommandProperty);
            }

            set
            {
                SetValue(LeftCommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the ICommand for right command request
        /// </summary>
        public ICommand RightCommand
        {
            get
            {
                return (ICommand)GetValue(RightCommandProperty);
            }

            set
            {
                SetValue(RightCommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the CommandParameter for left command request
        /// </summary>
        public object LeftCommandParameter
        {
            get
            {
                return GetValue(LeftCommandParameterProperty);
            }

            set
            {
                SetValue(LeftCommandParameterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the CommandParameter for right command request
        /// </summary>
        public object RightCommandParameter
        {
            get
            {
                return GetValue(RightCommandParameterProperty);
            }

            set
            {
                SetValue(RightCommandParameterProperty, value);
            }
        }

        /// <summary>
        /// Gets the SwipeStatus for current swipe status
        /// </summary>
        public SwipeStatus SwipeStatus
        {
            get
            {
                return (SwipeStatus)GetValue(SwipeStatusProperty);
            }

            private set
            {
                var oldValue = SwipeStatus;

                if (value != oldValue)
                {
                    SetValue(SwipeStatusProperty, value);

                    var eventArguments = new SwipeStatusChangedEventArgs()
                    {
                        OldValue = oldValue,
                        NewValue = value
                    };

                    SwipeStatusChanged?.Invoke(this, eventArguments);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the PointerReleased event is handled when swiping.
        /// Set this to <value>true</value> to prevent ItemClicked or selection to occur when swiping in a <see cref="ListView"/>
        /// </summary>
        public bool IsPointerReleasedOnSwipingHandled
        {
            get { return (bool)GetValue(IsPointerReleasedOnSwipingHandledProperty); }
            set { SetValue(IsPointerReleasedOnSwipingHandledProperty, value); }
        }
    }
}
