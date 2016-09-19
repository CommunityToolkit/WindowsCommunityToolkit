﻿// ******************************************************************
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
using System.Collections;
using System.Collections.Specialized;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// An items control that presents enumerable content similar to the live tiles on the
    /// start menu.
    /// </summary>
    [TemplatePart(Name = ScrollerPartName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = CurrentPartName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = NextPartName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TranslatePartName, Type = typeof(TranslateTransform))]
    [TemplatePart(Name = StackPartName, Type = typeof(StackPanel))]
    public class RotatorTile : Control
    {
        private const string ScrollerPartName = "Scroller";
        private const string CurrentPartName = "Current";
        private const string NextPartName = "Next";
        private const string TranslatePartName = "Translate";
        private const string StackPartName = "Stack";

        private static readonly Random _randomizer = new Random(); // randomizer for randomizing when a tile swaps content
        private int _currentIndex = -1; // current index in the items displayed
        private DispatcherTimer _timer;  // timer for triggering when to flip the content
        private FrameworkElement _currentElement; // FrameworkElement holding a reference to the current element being display
        private FrameworkElement _nextElement; // FrameworkElement holding a reference to the next element being display
        private FrameworkElement _scroller;  // Container Element that's being translated to animate from one item to the next
        private TranslateTransform _translate; // Translate Transform used when animating the transition
        private StackPanel _stackPanel; // StackPanel that contains the live tile elements
        private bool _suppressFlipOnSet; // Prevents the SelectedItem change handler to cause a flip
        private WeakEventListener<RotatorTile, object, NotifyCollectionChangedEventArgs> _inccWeakEventListener;

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(RotatorTile), new PropertyMetadata(null, OnItemsSourcePropertyChanged));

        /// <summary>
        /// Identifies the <see cref="ItemTemplate"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(RotatorTile), null);

        /// <summary>
        /// Identifies the <see cref="RotateDirection"/> property.
        /// </summary>
        public static readonly DependencyProperty RotateDirectionProperty =
            DependencyProperty.Register(nameof(RotateDirection), typeof(RotateDirection), typeof(RotatorTile), new PropertyMetadata(RotateDirection.Up));

        /// <summary>
        /// Identifies the <see cref="CurrentItem"/> property.
        /// </summary>
        public static readonly DependencyProperty CurrentItemProperty =
            DependencyProperty.Register(nameof(CurrentItem), typeof(object), typeof(RotatorTile), new PropertyMetadata(null, OnCurrentItemPropertyChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="RotatorTile"/> class.
        /// </summary>
        public RotatorTile()
        {
            DefaultStyleKey = typeof(RotatorTile);

            Unloaded += RotatorTile_Unloaded;
            Loaded += RotatorTile_Loaded;
            SizeChanged += RotatorTile_SizeChanged;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _scroller = GetTemplateChild(ScrollerPartName) as FrameworkElement;
            _currentElement = GetTemplateChild(CurrentPartName) as FrameworkElement;
            _nextElement = GetTemplateChild(NextPartName) as FrameworkElement;
            _translate = GetTemplateChild(TranslatePartName) as TranslateTransform;
            _stackPanel = GetTemplateChild(StackPartName) as StackPanel;
            if (_stackPanel != null)
            {
                _stackPanel.Orientation = Direction == RotateDirection.Up ? Orientation.Vertical : Orientation.Horizontal;
            }

            if (ItemsSource != null)
            {
                Start();
            }

            base.OnApplyTemplate();
        }

        private void RotatorTile_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_currentElement != null && _nextElement != null)
            {
                _currentElement.Width = _nextElement.Width = e.NewSize.Width;
                _currentElement.Height = _nextElement.Height = e.NewSize.Height;
            }

            // Set content area to twice the size in the slide direction
            if (_scroller != null)
            {
                if (Direction == RotateDirection.Up)
                {
                    _scroller.Height = e.NewSize.Height * 2;
                }
                else
                {
                    _scroller.Width = e.NewSize.Width * 2;
                }
            }

            // Set clip to control
            Clip = new RectangleGeometry() { Rect = new Rect(default(Point), e.NewSize) };
        }

        private void RotatorTile_Loaded(object sender, RoutedEventArgs e)
        {
            // Start timer after control has loaded
            _timer?.Start();
        }

        private void RotatorTile_Unloaded(object sender, RoutedEventArgs e)
        {
            // Stop timer and reset animation when control unloads
            _timer?.Stop();

            if (_translate != null)
            {
                _translate.Y = 0;
            }
        }

        /// <summary>
        /// Triggered when it's time to flip to the next live tile.
        /// </summary>
        private void Timer_Tick(object sender, object e)
        {
            var item = GetItemAt(_currentIndex + 1);
            _timer.Interval = TimeSpan.FromSeconds(_randomizer.Next(5) + 5); // randomize next flip
            UpdateNextItem();
        }

        private void UpdateNextItem()
        {
            _currentIndex++;
            CurrentItem = GetItemAt(_currentIndex);
        }

        private void RotateToNextItem()
        {
            // Check if there's more than one item. if not, don't start animation
            bool hasTwoOrMoreItems = false;
            if (ItemsSource is IEnumerable)
            {
                var enumerator = (ItemsSource as IEnumerable).GetEnumerator();
                int count = 0;
                while (enumerator.MoveNext())
                {
                    count++;
                    if (count > 1)
                    {
                        hasTwoOrMoreItems = true;
                        break;
                    }
                }
            }

            if (!hasTwoOrMoreItems)
            {
                return;
            }

            var sb = new Storyboard();
            if (_translate != null)
            {
                var anim = new DoubleAnimation
                {
                    Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                    From = 0
                };
                if (Direction == RotateDirection.Up)
                {
                    anim.To = -ActualHeight;
                }
                else if (Direction == RotateDirection.Left)
                {
                    anim.To = -ActualWidth;
                }

                anim.FillBehavior = FillBehavior.HoldEnd;
                anim.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
                Storyboard.SetTarget(anim, _translate);
                if (Direction == RotateDirection.Up)
                {
                    Storyboard.SetTargetProperty(anim, "Y");
                }
                else
                {
                    Storyboard.SetTargetProperty(anim, "X");
                }

                sb.Children.Add(anim);
            }

            sb.Completed += (a, b) =>
            {
                // Reset back and swap images, getting the next image ready
                sb.Stop();
                if (_translate != null)
                {
                    _translate.X = _translate.Y = 0;
                }

                if (_currentElement != null)
                {
                    _currentElement.DataContext = _nextElement.DataContext;
                }

                if (_nextElement != null)
                {
                    _nextElement.DataContext = GetNext(); // Preload the next tile
                }
            };
            sb.Begin();
        }

        private object GetCurrent()
        {
            return GetItemAt(_currentIndex);
        }

        private object GetNext()
        {
            if (_currentIndex < 0)
            {
                return null;
            }

            return GetItemAt(_currentIndex + 1);
        }

        private object GetItemAt(int index)
        {
            if (ItemsSource != null && index > -1)
            {
                if (ItemsSource is IList)
                {
                    var list = ItemsSource as IList;
                    if (list.Count > 0)
                    {
                        index = index % list.Count;
                        return list[index];
                    }
                }
                else if (ItemsSource is IEnumerable)
                {
                    var items = ItemsSource as IEnumerable;
                    var ienum = ((IEnumerable)ItemsSource).GetEnumerator();
                    int count = 0;
                    while (ienum.MoveNext())
                    {
                        count++;
                    }

                    if (count > 0)
                    {
                        index = index % count;
                        ienum.Reset();
                        for (int i = 0; i < index; i++)
                        {
                            ienum.MoveNext();
                        }

                        return ienum.Current;
                    }
                }
            }

            return null;
        }

        private int GetIndexOf(object item)
        {
            if (ItemsSource is IEnumerable)
            {
                int i = 0;
                var ienum = ((IEnumerable)ItemsSource).GetEnumerator();
                while (ienum.MoveNext())
                {
                    if (ienum.Current == item)
                    {
                        return i;
                    }

                    i++;
                }
            }

            return -1;
        }

        private void Start()
        {
            _currentIndex = 0;
            var currentItem = GetCurrent();
            if (_currentElement != null)
            {
                _currentElement.DataContext = currentItem;
            }

            if (_nextElement != null)
            {
                _nextElement.DataContext = GetNext();
            }

            if (currentItem == null)
            {
                _currentIndex = -1;
                _timer?.Stop();
                return;
            }

            if (_timer == null)
            {
                _timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
                _timer.Tick += Timer_Tick;
                _timer.Interval = TimeSpan.FromSeconds(_randomizer.Next(5) + 5);
            }

            _timer.Start();
            _suppressFlipOnSet = true;
            CurrentItem = currentItem;
            _suppressFlipOnSet = false;
        }

        /// <summary>
        /// Gets or sets the ItemsSource
        /// </summary>
        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (RotatorTile)d;
            ctrl.OnCollectionChanged(e.OldValue, e.NewValue);
        }

        private void OnCollectionChanged(object oldValue, object newValue)
        {
            if (oldValue is INotifyCollectionChanged)
            {
                var incc = (INotifyCollectionChanged)oldValue;
                incc.CollectionChanged -= Incc_CollectionChanged;
                _inccWeakEventListener?.Detach();
                _inccWeakEventListener = null;
            }

            if (newValue is IEnumerable)
            {
                if (newValue is INotifyCollectionChanged)
                {
                    var incc = (INotifyCollectionChanged)newValue;
                    _inccWeakEventListener = new WeakEventListener<RotatorTile, object, NotifyCollectionChangedEventArgs>(this)
                    {
                        OnEventAction = (instance, source, eventArgs) => instance.Incc_CollectionChanged(source, eventArgs),
                        OnDetachAction = (listener) => incc.CollectionChanged -= listener.OnEvent
                    };
                    incc.CollectionChanged += _inccWeakEventListener.OnEvent;
                }

                Start();
            }
            else
            {
                _timer?.Stop();
            }
        }

        private void Incc_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems?.Count > 0)
                {
                    int endIndex = e.OldStartingIndex + e.OldItems.Count;
                    if (_currentIndex >= e.NewStartingIndex && _currentIndex < endIndex)
                    {
                        UpdateNextItem(); // Current item was removed. Replace with the next one
                    }
                    else if (_currentIndex > endIndex)
                    {
                        // items were removed before the current item. Just update the changed index
                        _currentIndex -= (endIndex - e.NewStartingIndex) - 1;
                    }
                    else if (e.NewStartingIndex == _currentIndex + 1)
                    {
                        // upcoming item was changed, so update the datacontext
                        _nextElement.DataContext = GetNext();
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                int endIndex = e.NewStartingIndex + e.NewItems.Count;
                if (e.NewItems?.Count > 0)
                {
                    if (_currentIndex < 0)
                    {
                        // First item loaded. Start the rotator
                        Start();
                    }
                    else if (_currentIndex >= e.NewStartingIndex)
                    {
                        // Items were inserted before the current item. Update the index
                        _currentIndex += e.NewItems.Count;
                    }
                    else if (_currentIndex + 1 == e.NewStartingIndex)
                    {
                        // upcoming item was changed, so update the datacontext
                        _nextElement.DataContext = GetNext();
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                int endIndex = e.OldStartingIndex + e.OldItems.Count;
                if (_currentIndex >= e.OldStartingIndex && _currentIndex < endIndex + 1)
                {
                    // The current or next item was moved. Get its new location
                    UpdateNextItem();
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {
                int endIndex = e.OldStartingIndex + e.OldItems.Count;
                if (_currentIndex >= e.OldStartingIndex && _currentIndex < endIndex)
                {
                    // The current item was moved. Get its new location
                    _currentIndex = GetIndexOf(CurrentItem);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                // Significant change or clear. Restart.
                Start();
            }
        }

        /// <summary>
        /// Gets or sets the currently selected visible item
        /// </summary>
        public object CurrentItem
        {
            get { return GetValue(CurrentItemProperty); }
            set { SetValue(CurrentItemProperty, value); }
        }

        private static void OnCurrentItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (RotatorTile)d;
            if (ctrl._suppressFlipOnSet)
            {
                return;
            }

            int index = ctrl.GetIndexOf(e.NewValue);
            if (index > -1)
            {
                ctrl._currentIndex = index;
                ctrl._nextElement.DataContext = e.NewValue;
                ctrl.RotateToNextItem();
                ctrl._timer.Stop();
                ctrl._timer.Start();
            }
        }

        /// <summary>
        /// Gets or sets the item template
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the direction the tile slides in.
        /// </summary>
        public RotateDirection Direction
        {
            get { return (RotateDirection)GetValue(RotateDirectionProperty); }
            set { SetValue(RotateDirectionProperty, value); }
        }

        /// <summary>
        /// Tile Slide Direction
        /// </summary>
        public enum RotateDirection
        {
            /// <summary>Up</summary>
            Up,

            /// <summary>Left</summary>
            Left,
        }
    }
}
