using System;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    /// <summary>
    /// Defines the pivorama control.
    /// </summary>
    [TemplatePart(Name = "Frame", Type = typeof(Panel))]
    [TemplatePart(Name = "HeaderContainer", Type = typeof(Panel))]
    [TemplatePart(Name = "Header", Type = typeof(PivoramaPanel))]
    [TemplatePart(Name = "TabsContainer", Type = typeof(Panel))]
    [TemplatePart(Name = "Tabs", Type = typeof(PivoramaTabs))]
    [TemplatePart(Name = "PanelContainer", Type = typeof(Panel))]
    [TemplatePart(Name = "ScrollViewer", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "Clip", Type = typeof(RectangleGeometry))]
    public partial class Pivorama : Control
    {
        private Panel _frame;

        private Panel _headerContainer;

        private PivoramaPanel _header;

        private Panel _tabsContainer;

        private PivoramaTabs _tabs;

        private Panel _panelContainer;

        private ScrollViewer _scrollViewer;

        private RectangleGeometry _clip;

        private bool _isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pivorama"/>.       
        /// </summary>
        public Pivorama()
        {
            DefaultStyleKey = typeof(Pivorama);
        }

        /// <summary>
        /// Called when applying the control's template.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            if (_frame != null)
            {
                _frame.ManipulationDelta -= Panel_ManipulationDelta;
                _frame.ManipulationCompleted -= Panel_ManipulationCompleted;
                _frame.PointerWheelChanged -= Panel_PointerWheelChanged;
                _frame = null;
            }

            if (_header != null)
            {
                _header.SelectedIndexChanged -= PivoramaPanel_SelectedIndexChanged;
                _header = null;
            }

            if (_tabs != null)
            {
                _tabs.SelectedIndexChanged -= PivoramaPanel_SelectedIndexChanged;
                _tabs = null;
            }

            if (_panelContainer != null)
            {
                _panelContainer.ManipulationDelta -= Panel_ManipulationDelta;
                _panelContainer.ManipulationCompleted -= Panel_ManipulationCompleted;
                _panelContainer.PointerWheelChanged -= Panel_PointerWheelChanged;
                _panelContainer = null;
            }

            _frame = GetTemplateChild("Frame") as Panel;

            _headerContainer = GetTemplateChild("HeaderContainer") as Panel;
            _header = GetTemplateChild("Header") as PivoramaPanel;

            if (_header != null)
            {
                _header.SelectedIndexChanged += PivoramaPanel_SelectedIndexChanged;
            }

            _tabsContainer = GetTemplateChild("TabsContainer") as Panel;
            _tabs = GetTemplateChild("Tabs") as PivoramaTabs;

            if (_tabs != null)
            {
                _tabs.SelectedIndexChanged += PivoramaPanel_SelectedIndexChanged;
            }

            _panelContainer = GetTemplateChild("PanelContainer") as Panel;

            _scrollViewer = GetTemplateChild("ScrollViewer") as ScrollViewer;

            _clip = GetTemplateChild("Clip") as RectangleGeometry;

            if (_frame != null)
            {
                _frame.ManipulationDelta += Panel_ManipulationDelta;
                _frame.ManipulationCompleted += Panel_ManipulationCompleted;
                _frame.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateInertia
                                          | ManipulationModes.System;
                _frame.PointerWheelChanged += Panel_PointerWheelChanged;
            }

            if (_panelContainer != null)
            {
                _panelContainer.ManipulationDelta += Panel_ManipulationDelta;
                _panelContainer.ManipulationCompleted += Panel_ManipulationCompleted;
                _panelContainer.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateInertia
                                                   | ManipulationModes.System;
                _panelContainer.PointerWheelChanged += Panel_PointerWheelChanged;
            }

            _isInitialized = true;

            ItemWidthEx = ItemWidth;

            SizeChanged += Pivorama_SizeChanged;

            base.OnApplyTemplate();
        }

        private void PivoramaPanel_SelectedIndexChanged(object sender, int index)
        {
            if (Index != index)
            {
                Index = index - 1;
                AnimateNext(100);
            }
        }

        private void Pivorama_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RefreshLayout();

            if (_clip != null)
            {
                _clip.Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height);
            }
        }

        private void RefreshLayout()
        {
            if (FitToScreen)
            {
                ItemWidthEx = Math.Round(ActualWidth);

                if (_headerContainer != null)
                {
                    _headerContainer.Visibility = Visibility.Collapsed;
                }

                if (_tabsContainer != null)
                {
                    _tabsContainer.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (ItemWidthEx * 2 < ActualWidth)
                {
                    if (_headerContainer != null)
                    {
                        _headerContainer.Visibility = Visibility.Visible;
                    }

                    if (_tabsContainer != null)
                    {
                        _tabsContainer.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    if (_headerContainer != null)
                    {
                        _headerContainer.Visibility = Visibility.Collapsed;
                    }

                    if (_tabsContainer != null)
                    {
                        _tabsContainer.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void SetFitToScreen(bool fitToScreen)
        {
            if (_isInitialized)
            {
                ItemWidthEx = fitToScreen ? ActualWidth : ItemWidth;
                RefreshLayout();
            }
        }

        private void SetItemWidth(double newValue)
        {
            if (_isInitialized)
            {
                ItemWidthEx = FitToScreen ? ActualWidth : newValue;
            }
        }

        private void SetItemWidthEx(double newValue, double oldValue)
        {
            if (_isInitialized)
            {
                int oldIndex = (int)(-Position / oldValue);
                Position = -oldIndex * newValue;
                Index = (int)(-Position / ItemWidthEx);
            }
        }

        private void SetIndex(int newValue)
        {
            if (_isInitialized)
            {
                Position = -newValue * ItemWidthEx;
            }
        }
    }
}