using System;
using System.Threading.Tasks;

using Windows.UI.Xaml.Input;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    /// <summary>
    /// Defines the functionality for handling the manipulation of the <see cref="Pivorama"/> control.
    /// </summary>
    public partial class Pivorama
    {
        private bool _isAnimating;

        private void Panel_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            if (_isAnimating)
            {
                return;
            }

            var point = e.GetCurrentPoint(this);
            int sign = Math.Sign(point.Properties.MouseWheelDelta);
            if (sign > 0)
            {
                if (_scrollViewer != null && _scrollViewer.VerticalOffset == 0)
                {
                    _headerContainer?.TranslateDeltaX(1);
                    _panelContainer?.TranslateDeltaX(1);

                    AnimatePrev();
                    e.Handled = true;
                }
            }
            else
            {
                if (!(_scrollViewer != null && _scrollViewer.VerticalOffset < _scrollViewer.ScrollableHeight))
                {
                    _headerContainer?.TranslateDeltaX(-1);
                    _panelContainer?.TranslateDeltaX(-1);

                    AnimateNext();
                    e.Handled = true;
                }
            }
        }

        private void Panel_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            double deltaX = e.Delta.Translation.X;

            if (e.IsInertial)
            {
                e.Complete();
            }
            else
            {
                if (Math.Abs(e.Cumulative.Translation.X) >= ItemWidthEx)
                {
                    e.Complete();
                }
                else
                {
                    _headerContainer?.TranslateDeltaX(deltaX);
                    _panelContainer?.TranslateDeltaX(deltaX);

                    if (_tabsContainer != null)
                    {
                        if (Math.Sign(deltaX) > 0)
                        {
                            _tabsContainer.TranslateDeltaX(deltaX * _tabs.PrevTabWidth / ItemWidthEx);
                        }
                        else
                        {
                            _tabsContainer.TranslateDeltaX(deltaX * _tabs.SelectedTabWidth / ItemWidthEx);
                        }
                    }
                }
            }
            e.Handled = true;
        }

        private void Panel_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.IsInertial)
            {
                if (Math.Sign(e.Cumulative.Translation.X) < 0)
                {
                    AnimateNext();
                }
                else
                {
                    AnimatePrev();
                }
            }
            else
            {
                if (Offset > ItemWidthEx / 2.0)
                {
                    AnimateNext();
                }
                else
                {
                    AnimatePrev();
                }
            }
            e.Handled = true;
        }

        private async void AnimateNext(double duration = 500)
        {
            if (_headerContainer != null && _panelContainer != null && _tabsContainer != null)
            {
                double delta = ItemWidthEx - Offset;
                double position = Position - delta;
                duration = duration * delta / ItemWidthEx;

                _isAnimating = true;

                var t1 = _headerContainer.AnimateXAsync(position, duration);
                var t2 = AnimateTabsNextAsync(duration * 1);
                var t3 = _panelContainer.AnimateXAsync(position, duration);
                await Task.WhenAll(t1, t2, t3);

                _isAnimating = false;

                Index = (int)(-position / ItemWidthEx);
                _tabsContainer.TranslateX(0);
            }
        }

        private async void AnimatePrev(double duration = 500)
        {
            if (_headerContainer != null && _panelContainer != null && _tabsContainer != null)
            {
                double delta = Offset;
                double position = Position + delta;
                duration = duration * delta / ItemWidthEx;

                _isAnimating = true;

                var t1 = _headerContainer.AnimateXAsync(position, duration);
                var t2 = AnimateTabsPrevAsync(duration * 1);
                var t3 = _panelContainer.AnimateXAsync(position, duration);
                await Task.WhenAll(t1, t2, t3);

                _isAnimating = false;

                Index = (int)(-position / ItemWidthEx);
                _tabsContainer.TranslateX(0);
            }
        }

        private async Task AnimateTabsNextAsync(double duration)
        {
            if (_tabsContainer != null)
            {
                double x = _tabsContainer.GetTranslateX();
                if (x > 0)
                {
                    await _tabsContainer.AnimateXAsync(0, duration, null);
                }
                else
                {
                    await _tabsContainer.AnimateXAsync(-_tabs.SelectedTabWidth, duration, null);
                }
            }
        }

        private async Task AnimateTabsPrevAsync(double duration)
        {
            if (_tabsContainer != null)
            {
                double x = _tabsContainer.GetTranslateX();
                if (x < 0)
                {
                    await _tabsContainer.AnimateXAsync(0, duration, null);
                }
                else
                {
                    await _tabsContainer.AnimateXAsync(_tabs.PrevTabWidth, duration, null);
                }
            }
        }
    }
}
