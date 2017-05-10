using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    [TemplatePart(Name = "AnchorCanvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "OrbitGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "CenterContent", Type = typeof(ContentPresenter))]
    public sealed class SpaceView : ItemsControl
    {
        public SpaceView()
        {
            DefaultStyleKey = typeof(SpaceView);

            IsTabStop = false;
            TabNavigation = KeyboardNavigationMode.Once;
            KeyDown += SpaceView_KeyDown;
            _orbits = new Dictionary<object, Ellipse>();
            _anchors = new Dictionary<object, Line>();

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                var items = new List<SpaceViewItem>();
                items.Add(new SpaceViewItem() { Distance = 0.1, Diameter = 0.5, Label = "test" });
                items.Add(new SpaceViewItem() { Distance = 0.1, Diameter = 0.5, Label = "test" });
                items.Add(new SpaceViewItem() { Distance = 0.1, Diameter = 0.5, Label = "test" });
                ItemsSource = items;
            }
        }

        private const double _animationDuration = 200;

        private SpaceViewPanel _panel;
        private Grid _orbitsContainer;
        private Canvas _anchorCanvas;
        private ContentPresenter _centerContent;
        private Compositor _compositor;

        private Dictionary<object, Ellipse> _orbits;
        private Dictionary<object, Line> _anchors;

        public event EventHandler<SpaceViewItemClickedEventArgs> ItemClicked;


        protected override void OnApplyTemplate()
        {
            _centerContent = (ContentPresenter)GetTemplateChild("CenterContent");
            if (_centerContent == null)
            {
                return;
            }

            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            base.OnApplyTemplate();
        }

        public bool OrbitsEnabled
        {
            get { return (bool)GetValue(OrbitsEnabledProperty); }
            set { SetValue(OrbitsEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrbitsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrbitsEnabledProperty =
            DependencyProperty.Register("OrbitsEnabled", typeof(bool), typeof(SpaceView), new PropertyMetadata(false, OnOrbitsEnabledChanged));

        public bool IsItemClickEnabled
        {
            get { return (bool)GetValue(IsItemClickEnabledProperty); }
            set { SetValue(IsItemClickEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsItemClickEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsItemClickEnabledProperty =
            DependencyProperty.Register("IsItemClickEnabled", typeof(bool), typeof(SpaceView), new PropertyMetadata(false, OnItemClickEnabledChanged));

        public bool AnchorsEnabled
        {
            get { return (bool)GetValue(AnchorsEnabledProperty); }
            set { SetValue(AnchorsEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnchorsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnchorsEnabledProperty =
            DependencyProperty.Register("AnchorsEnabled", typeof(bool), typeof(SpaceView), new PropertyMetadata(false, OnAchorsEnabledChanged));

        public double MinItemSize
        {
            get { return (double)GetValue(MinItemSizeProperty); }
            set { SetValue(MinItemSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinItemSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinItemSizeProperty =
            DependencyProperty.Register("MinItemSize", typeof(double), typeof(SpaceView), new PropertyMetadata(20d, OnItemSizePropertyChanged));

        public double MaxItemSize
        {
            get { return (double)GetValue(MaxItemSizeProperty); }
            set { SetValue(MaxItemSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxItemSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxItemSizeProperty =
            DependencyProperty.Register("MaxItemSize", typeof(double), typeof(SpaceView), new PropertyMetadata(50d, OnItemSizePropertyChanged));

        public Brush AnchorColor
        {
            get { return (Brush)GetValue(AnchorColorProperty); }
            set { SetValue(AnchorColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnchorColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnchorColorProperty =
            DependencyProperty.Register("AnchorColor", typeof(Brush), typeof(SpaceView), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnAnchorPropertyChanged));

        public Brush OrbitColor
        {
            get { return (Brush)GetValue(OrbitColorProperty); }
            set { SetValue(OrbitColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrbitColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrbitColorProperty =
            DependencyProperty.Register("OrbitColor", typeof(Brush), typeof(SpaceView), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnOrbitPropertyChanged));

        public DoubleCollection OrbitDashArray
        {
            get { return (DoubleCollection)GetValue(OrbitDashArrayProperty); }
            set { SetValue(OrbitDashArrayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrbitDashArray.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrbitDashArrayProperty =
            DependencyProperty.Register("OrbitDashArray", typeof(DoubleCollection), typeof(SpaceView), new PropertyMetadata(null, OnOrbitPropertyChanged));

        public double AnchorThickness
        {
            get { return (double)GetValue(AnchorThicknessProperty); }
            set { SetValue(AnchorThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnchorThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnchorThicknessProperty =
            DependencyProperty.Register("AnchorThickness", typeof(double), typeof(double), new PropertyMetadata(1d, OnAnchorPropertyChanged));

        public double OrbitThickness
        {
            get { return (double)GetValue(OrbitThicknessProperty); }
            set { SetValue(OrbitThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrbitThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrbitThicknessProperty =
            DependencyProperty.Register("OrbitThickness", typeof(double), typeof(SpaceView), new PropertyMetadata(1d, OnOrbitPropertyChanged));

        private static void OnOrbitPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sv = d as SpaceView;
            if (sv._orbitsContainer == null)
            {
                return;
            }

            foreach (var orbit in sv._orbitsContainer.Children)
            {
                sv.SetOrbitProperties(orbit as Ellipse);
            }
        }

        private static void OnAnchorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sv = d as SpaceView;
            if (sv._anchorCanvas == null)
            {
                return;
            }

            foreach (var anchor in sv._anchorCanvas.Children)
            {
                sv.SetAnchorProperties(anchor as Line);
            }
        }

        public object CenterContent
        {
            get { return (int)GetValue(CenterContentProperty); }
            set { SetValue(CenterContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CenterContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CenterContentProperty =
            DependencyProperty.Register("CenterContent", typeof(int), typeof(SpaceView), new PropertyMetadata(0));

        private static void OnAchorsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sv = d as SpaceView;

            if (e.NewValue == e.OldValue) return;

            if (!(bool)e.NewValue)
            {
                sv.ClearAnchors();
            }
            else
            {
                sv.ItemsPanelRoot?.InvalidateArrange();
            }
        }

        private static void OnOrbitsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sv = d as SpaceView;

            if (e.NewValue == e.OldValue) return;

            if (!(bool)e.NewValue)
            {
                sv.ClearOrbits();
            }
            else
            {
                sv.ItemsPanelRoot?.InvalidateArrange();
            }
        }

        private static void OnItemSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sv = d as SpaceView;

            if (sv.ItemsPanelRoot != null)
            {
                foreach (var element in sv.ItemsPanelRoot.Children)
                {
                    if (element is ContentControl && (element as ContentControl).DataContext is SpaceViewItem)
                    {
                        var item = (element as FrameworkElement).DataContext as SpaceViewItem;
                        if (item.Diameter >= 0)
                        {
                            double diameter = Math.Min(item.Diameter, 1d);
                            var content = (element as ContentControl).Content as FrameworkElement;
                            content.Width = content.Height = (diameter * (sv.MaxItemSize - sv.MinItemSize)) + sv.MinItemSize;
                        }
                    }
                }
            }

            sv.ItemsPanelRoot?.InvalidateArrange();
        }

        private static void OnItemClickEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sv = d as SpaceView;

            if (sv.Items.Count == 0) return;

            foreach (var control in sv._panel.Children)
            {
                if ((bool)e.NewValue)
                {
                    sv.EnableItemInteraction(control as ContentControl);
                }
                else
                {
                    sv.DisableItemInteraction(control as ContentControl);
                }
            }
        }

        /// <summary>
        /// Returns the container used for each item
        /// </summary>
        /// <returns>Returns always a ContentControl</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            var element = new ContentControl();
            ApplyImplicitOffsetAnimation(element);
            return element;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            if (_panel == null && ItemsPanelRoot != null)
            {
                _panel = ItemsPanelRoot as SpaceViewPanel;
                _panel.ItemArranged += SpaceViewPanel_ItemArranged;
                _panel.ItemsArranged += SpaceViewPanel_ItemsArranged;
            }

            var control = element as ContentControl;
            var spaceViewItem = item as SpaceViewItem;
            FrameworkElement spaceViewElement = null;

            if (control != null)
            {
                spaceViewElement = ItemTemplate?.LoadContent() as FrameworkElement;
                if (spaceViewElement == null)
                {
                    var itemEllipse = new Ellipse()
                    {
                        Fill = Foreground,
                    };

                    if (spaceViewItem != null && spaceViewItem.Image != null)
                    {
                        itemEllipse.Fill = new ImageBrush() { ImageSource = spaceViewItem.Image };
                    }

                    spaceViewElement = itemEllipse;
                }

                control.Content = spaceViewElement;
                control.DataContext = item;

                if (IsItemClickEnabled)
                {
                    EnableItemInteraction(control);
                }
            }
            else if (element is FrameworkElement && (element as FrameworkElement).DataContext is SpaceViewItem)
            {
                spaceViewElement = element as FrameworkElement;
                spaceViewItem = spaceViewElement.DataContext as SpaceViewItem;
            }

            if (spaceViewItem != null)
            {
                element.SetValue(AutomationProperties.NameProperty, spaceViewItem.Label);
                if (spaceViewItem.Diameter >= 0)
                {
                    double diameter = Math.Min(spaceViewItem.Diameter, 1d);
                    spaceViewElement.Width = spaceViewElement.Height = (diameter * (MaxItemSize - MinItemSize)) + MinItemSize;
                }
            }
            else
            {
                element.SetValue(AutomationProperties.NameProperty, item.ToString());
            }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);

            Ellipse orbit;
            _orbits.TryGetValue(element, out orbit);

            if (orbit != null)
            {
                _orbits.Remove(element);
                _orbitsContainer.Children.Remove(orbit);
            }

            Line anchor;
            _anchors.TryGetValue(element, out anchor);

            if (anchor != null)
            {
                _anchors.Remove(element);
                _anchorCanvas.Children.Remove(anchor);
            }
        }

        private void SpaceViewPanel_ItemsArranged(object sender, SpaceViewPanelItemsArrangedArgs e)
        {
            if (AnchorsEnabled)
            {
                if (_anchorCanvas == null)
                {
                    _anchorCanvas = (Canvas)GetTemplateChild("AnchorCanvas");
                    if (_anchorCanvas == null)
                    {
                        return;
                    }
                }

                if (_anchorCanvas.Children.Count != e.Elements.Count)
                {
                    _anchorCanvas.Children.Clear();
                    foreach (var element in e.Elements)
                    {
                        var anchor = CreateAnchor(element.Element, element.XYFromCenter.X, element.XYFromCenter.Y);
                        _anchorCanvas.Children.Add(anchor);
                        _anchors.Add(element, anchor);
                    }
                }
            }
        }

        private void SpaceViewPanel_ItemArranged(object sender, SpaceViewPanelItemArrangedArgs e)
        {
            if (OrbitsEnabled)
            {
                Ellipse orbit;
                _orbits.TryGetValue(e.ElementProperties.Element, out orbit);

                if (orbit == null)
                {
                    if (_orbitsContainer == null)
                    {
                        _orbitsContainer = (Grid)GetTemplateChild("OrbitGrid");
                        if (_orbitsContainer == null)
                        {
                            return;
                        }
                    }

                    orbit = CreateOrbit();
                    _orbits.Add(e.ElementProperties.Element, orbit);
                    _orbitsContainer.Children.Add(orbit);
                }

                orbit.Height = orbit.Width = e.ElementProperties.DistanceFromCenter * 2;
            }
        }

        private void EnableItemInteraction(ContentControl control)
        {
            DisableItemInteraction(control);

            control.IsTabStop = true;
            control.UseSystemFocusVisuals = true;
            control.PointerEntered += Control_PointerEntered;
            control.PointerExited += Control_PointerExited;
            control.PointerPressed += Control_PointerPressed;
            control.PointerReleased += Control_PointerReleased;
            control.KeyDown += Control_KeyDown;
            control.KeyUp += Control_KeyUp;
        }

        private void DisableItemInteraction(ContentControl control)
        {
            control.IsTabStop = false;
            control.UseSystemFocusVisuals = false;
            control.PointerEntered -= Control_PointerEntered;
            control.PointerExited -= Control_PointerExited;
            control.PointerPressed -= Control_PointerPressed;
            control.PointerReleased -= Control_PointerReleased;
            control.KeyDown -= Control_KeyDown;
            control.KeyUp -= Control_KeyUp;
        }

        private void Control_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Space || e.Key == Windows.System.VirtualKey.GamepadA)
            {
                var item = sender as ContentControl;
                item.Scale(1, 1, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
            }
        }

        private void Control_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Space || e.Key == Windows.System.VirtualKey.GamepadA)
            {
                var item = sender as ContentControl;
                item.Scale(0.9f, 0.9f, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
                ItemClicked?.Invoke(this, new SpaceViewItemClickedEventArgs(item, item.DataContext));
            }
        }

        private void Control_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var item = sender as ContentControl;
            item.Scale(1, 1, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
            ItemClicked?.Invoke(this, new SpaceViewItemClickedEventArgs(item, item.DataContext));
        }

        private void Control_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var item = sender as ContentControl;
            item.Scale(0.9f, 0.9f, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
        }

        private void Control_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var item = sender as ContentControl;
            item.Scale(1, 1, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
        }

        private void Control_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var item = sender as ContentControl;
            item.Scale(1.1f, 1.1f, (float)item.ActualWidth / 2, (float)item.ActualHeight / 2, _animationDuration).Start();
        }

        private void SpaceView_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (ItemsPanelRoot == null)
            {
                return;
            }

            if (e.Key == Windows.System.VirtualKey.Left)
            {
                e.Handled = true;
                var currentEllement = FocusManager.GetFocusedElement() as ContentControl;
                if (currentEllement != null)
                {
                    var index = ItemsPanelRoot.Children.IndexOf(currentEllement);
                    var nextIndex = (index + 1) % Items.Count;

                    (ItemsPanelRoot.Children.ElementAt(nextIndex) as ContentControl).Focus(FocusState.Keyboard);
                }
            }
            else if (e.Key == Windows.System.VirtualKey.Right)
            {
                e.Handled = true;
                var currentEllement = FocusManager.GetFocusedElement() as ContentControl;
                if (currentEllement != null)
                {
                    var index = ItemsPanelRoot.Children.IndexOf(currentEllement);
                    var nextIndex = index > 0 ? index - 1 : Items.Count - 1;

                    (ItemsPanelRoot.Children.ElementAt(nextIndex) as ContentControl).Focus(FocusState.Keyboard);
                }
            }
        }

        private void ClearOrbits()
        {
            if (_orbitsContainer == null)
            {
                return;
            }

            _orbitsContainer.Children.Clear();
            _orbits.Clear();
        }

        private Ellipse CreateOrbit()
        {
            var orbit = new Ellipse()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            SetOrbitProperties(orbit);

            return orbit;
        }

        private void SetOrbitProperties(Ellipse orbit)
        {
            orbit.StrokeDashArray = OrbitDashArray;
            orbit.Stroke = OrbitColor;
            orbit.StrokeThickness = OrbitThickness;
        }

        private void SetAnchorProperties(Line anchor)
        {
            anchor.Stroke = AnchorColor;
            anchor.StrokeThickness = AnchorThickness;
        }

        private Line CreateAnchor(UIElement element, double x, double y)
        {
            var anchor = new Line()
            {
                X1 = 0,
                Y1 = 0,
                X2 = 100,
                Y2 = 0
            };

            SetAnchorProperties(anchor);

            var anchorVisual = ElementCompositionPreview.GetElementVisual(anchor);
            var elementVisual = ElementCompositionPreview.GetElementVisual(element);
            var centerVisual = ElementCompositionPreview.GetElementVisual(_centerContent);

            string expression = "";
            var elementY = "(elementVisual.Offset.Y + elementVisual.Size.Y / 2)";
            var centerY = "(centerVisual.Offset.Y + centerVisual.Size.Y / 2)";
            var elementX = "(elementVisual.Offset.X + elementVisual.Size.X / 2)";
            var centerX = "(centerVisual.Offset.X + centerVisual.Size.X / 2)";

            var startingAngle = Math.Atan2(y, x);

            if (startingAngle > Math.PI / 4 && startingAngle < 3 * Math.PI / 4)
                expression = $"Atan((-1 * ({elementX} - {centerX})) / ( {elementY} - {centerY})) - PI / 2";
            else if (startingAngle >= 3 * Math.PI / 4 || startingAngle < -3 * Math.PI / 4)
                expression = $"Atan(({elementY} - {centerY}) / ({elementX} - {centerX})) + PI";
            else if (startingAngle >= -3 * Math.PI / 4 && startingAngle < Math.PI / -4)
                expression = $"Atan(({elementX} - {centerX}) / (-1 * ({elementY} - {centerY}))) + PI  / 2";
            else
                expression = $"Atan(({elementY} - {centerY}) / ({elementX} - {centerX}))";

            anchorVisual.CenterPoint = new Vector3(0);
            var rotationExpression = _compositor.CreateExpressionAnimation();
            rotationExpression.Expression = expression;
            rotationExpression.SetReferenceParameter("centerVisual", centerVisual);
            rotationExpression.SetReferenceParameter("elementVisual", elementVisual);
            anchorVisual.StartAnimation(nameof(anchorVisual.RotationAngle), rotationExpression);

            var offsetExpression = _compositor.CreateExpressionAnimation();
            offsetExpression.Expression = "Vector3(centerVisual.Offset.X + centerVisual.Size.X / 2, centerVisual.Offset.Y + centerVisual.Size.Y / 2, 0)";
            offsetExpression.SetReferenceParameter("centerVisual", centerVisual);
            anchorVisual.StartAnimation(nameof(anchorVisual.Offset), offsetExpression);

            var scaleExpression = _compositor.CreateExpressionAnimation();
            scaleExpression.Expression = $"Vector3(Pow(Pow({elementX} - {centerX}, 2) + Pow({elementY} - {centerY}, 2), 0.5)/100, 1, 1)";
            scaleExpression.SetReferenceParameter("centerVisual", centerVisual);
            scaleExpression.SetReferenceParameter("elementVisual", elementVisual);
            anchorVisual.StartAnimation(nameof(anchorVisual.Scale), scaleExpression);

            return anchor;
        }

        private void ClearAnchors()
        {
            if (_anchorCanvas == null)
            {
                return;
            }

            _anchorCanvas.Children.Clear();
            _anchors.Clear();
        }

        private void ApplyImplicitOffsetAnimation(UIElement element, double delay = 0)
        {
            // don't use animations if running in designer
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) return;

            if (_compositor == null) return;

            var easeIn = _compositor.CreateCubicBezierEasingFunction(new Vector2(0.03f, 1.11f), new Vector2(.66f, 1.11f));

            var offsetAnimation = _compositor.CreateVector3KeyFrameAnimation();
            offsetAnimation.Target = nameof(Visual.Offset);
            offsetAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue", easeIn);
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(200);
            offsetAnimation.DelayTime = TimeSpan.FromMilliseconds(delay);

            var implicitAnimations = _compositor.CreateImplicitAnimationCollection();
            implicitAnimations[nameof(Visual.Offset)] = offsetAnimation;

            ElementCompositionPreview.GetElementVisual(element).ImplicitAnimations = implicitAnimations;
        }
    }
}
