// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Numerics;
using Microsoft.UI;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace CommunityToolkit.WinUI.UI
{
    /// <summary>
    /// A helper to add a composition based drop shadow to a <see cref="FrameworkElement"/>.
    /// </summary>
    public sealed class AttachedDropShadow : AttachedShadowBase
    {
        private const float MaxBlurRadius = 72;

        /// <inheritdoc/>
        public override bool IsSupported => true;

        /// <inheritdoc/>
        protected internal override bool SupportsOnSizeChangedEvent => true;

        private static readonly TypedResourceKey<CompositionRoundedRectangleGeometry> RoundedRectangleGeometryResourceKey = "RoundedGeometry";
        private static readonly TypedResourceKey<CompositionSpriteShape> ShapeResourceKey = "Shape";
        private static readonly TypedResourceKey<ShapeVisual> ShapeVisualResourceKey = "ShapeVisual";
        private static readonly TypedResourceKey<CompositionSurfaceBrush> SurfaceBrushResourceKey = "SurfaceBrush";
        private static readonly TypedResourceKey<CompositionVisualSurface> VisualSurfaceResourceKey = "VisualSurface";

        /// <summary>
        /// Gets or sets a value indicating whether the panel uses an alpha mask to create a more precise shadow vs. a quicker rectangle shape.
        /// </summary>
        /// <remarks>
        /// Turn this off to lose fidelity and gain performance of the panel.
        /// </remarks>
        public bool IsMasked
        {
            get { return (bool)GetValue(IsMaskedProperty); }
            set { SetValue(IsMaskedProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsMasked"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsMaskedProperty =
            DependencyProperty.Register(nameof(IsMasked), typeof(bool), typeof(AttachedDropShadow), new PropertyMetadata(true, OnDependencyPropertyChanged));

        /// <summary>
        /// Gets or sets the roundness of the shadow's corners.
        /// </summary>
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// The <see cref="DependencyProperty"/> for <see cref="CornerRadius"/>
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(double),
                typeof(AttachedDropShadow),
                new PropertyMetadata(4d, OnDependencyPropertyChanged)); // Default WinUI ControlCornerRadius is 4

        /// <summary>
        /// Gets or sets the <see cref="Panel"/> to be used as a backdrop to cast shadows on.
        /// </summary>
        public FrameworkElement CastTo
        {
            get { return (FrameworkElement)GetValue(CastToProperty); }
            set { SetValue(CastToProperty, value); }
        }

        /// <summary>
        /// The <see cref="DependencyProperty"/> for <see cref="CastTo"/>
        /// </summary>
        public static readonly DependencyProperty CastToProperty =
            DependencyProperty.Register(nameof(CastTo), typeof(FrameworkElement), typeof(AttachedDropShadow), new PropertyMetadata(null, OnCastToPropertyChanged)); // TODO: Property Change

        private ContainerVisual _container;

        private static void OnCastToPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AttachedDropShadow shadow)
            {
                if (e.OldValue is FrameworkElement element)
                {
                    ElementCompositionPreview.SetElementChildVisual(element, null);
                    element.SizeChanged -= shadow.CastToElement_SizeChanged;
                }

                if (e.NewValue is FrameworkElement elementNew)
                {
                    var prevContainer = shadow._container;

                    var child = ElementCompositionPreview.GetElementChildVisual(elementNew);
                    if (child is ContainerVisual visual)
                    {
                        shadow._container = visual;
                    }
                    else
                    {
                        var compositor = ElementCompositionPreview.GetElementVisual(shadow.CastTo).Compositor;
                        shadow._container = compositor.CreateContainerVisual();

                        ElementCompositionPreview.SetElementChildVisual(elementNew, shadow._container);
                    }

                    // Need to remove all old children from previous container if it's changed
                    if (prevContainer != null && prevContainer != shadow._container)
                    {
                        foreach (var context in shadow.EnumerateElementContexts())
                        {
                            if (context.IsInitialized &&
                                prevContainer.Children.Contains(context.SpriteVisual))
                            {
                                prevContainer.Children.Remove(context.SpriteVisual);
                            }
                        }
                    }

                    // Make sure all child shadows are hooked into container
                    foreach (var context in shadow.EnumerateElementContexts())
                    {
                        if (context.IsInitialized)
                        {
                            shadow.SetElementChildVisual(context);
                        }
                    }

                    elementNew.SizeChanged += shadow.CastToElement_SizeChanged;

                    // Re-trigger updates to all shadow locations for new parent
                    shadow.CastToElement_SizeChanged(null, null);
                }
            }
        }

        private void CastToElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Don't use sender or 'e' here as related to container element not
            // element for shadow, grab values off context. (Also may be null from internal call.)
            foreach (var context in EnumerateElementContexts())
            {
                if (context.IsInitialized)
                {
                    // TODO: Should we use ActualWidth/Height instead of RenderSize?
                    OnSizeChanged(context, context.Element.RenderSize, context.Element.RenderSize);
                }
            }
        }

        /// <inheritdoc/>
        protected internal override void OnElementContextUninitialized(AttachedShadowElementContext context)
        {
            if (_container != null && _container.Children.Contains(context.SpriteVisual))
            {
                _container.Children.Remove(context.SpriteVisual);
            }

            context.SpriteVisual?.StopAnimation("Size");

            context.Element.LayoutUpdated -= Element_LayoutUpdated;

            if (context.VisibilityToken != null)
            {
                context.Element.UnregisterPropertyChangedCallback(UIElement.VisibilityProperty, context.VisibilityToken.Value);
                context.VisibilityToken = null;
            }

            base.OnElementContextUninitialized(context);
        }

        /// <inheritdoc/>
        protected override void SetElementChildVisual(AttachedShadowElementContext context)
        {
            if (_container != null && !_container.Children.Contains(context.SpriteVisual))
            {
                _container.Children.InsertAtTop(context.SpriteVisual);
            }

            // Handles size changing and other elements around it updating.
            context.Element.LayoutUpdated -= Element_LayoutUpdated;
            context.Element.LayoutUpdated += Element_LayoutUpdated;

            if (context.VisibilityToken != null)
            {
                context.Element.UnregisterPropertyChangedCallback(UIElement.VisibilityProperty, context.VisibilityToken.Value);
                context.VisibilityToken = null;
            }

            context.VisibilityToken = context.Element.RegisterPropertyChangedCallback(UIElement.VisibilityProperty, Element_VisibilityChanged);
        }

        private void Element_LayoutUpdated(object sender, object e)
        {
            // Update other shadows to account for layout changes
            CastToElement_SizeChanged(null, null);
        }

        private void Element_VisibilityChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (sender is FrameworkElement element)
            {
                var context = GetElementContext(element);

                if (element.Visibility == Visibility.Collapsed)
                {
                    if (_container != null && _container.Children.Contains(context.SpriteVisual))
                    {
                        _container.Children.Remove(context.SpriteVisual);
                    }
                }
                else
                {
                    if (_container != null && !_container.Children.Contains(context.SpriteVisual))
                    {
                        _container.Children.InsertAtTop(context.SpriteVisual);
                    }
                }
            }

            // Update other shadows to account for layout changes
            CastToElement_SizeChanged(null, null);
        }

        /// <inheritdoc/>
        protected override CompositionBrush GetShadowMask(AttachedShadowElementContext context)
        {
            CompositionBrush mask = null;

            if (DesignTimeHelpers.IsRunningInLegacyDesignerMode)
            {
                return null;
            }

            if (context.Element != null)
            {
                if (IsMasked)
                {
                    // We check for IAlphaMaskProvider first, to ensure that we use the custom
                    // alpha mask even if Content happens to extend any of the other classes
                    if (context.Element is IAlphaMaskProvider maskedControl)
                    {
                        if (maskedControl.WaitUntilLoaded && !context.Element.IsLoaded)
                        {
                            context.Element.Loaded += CustomMaskedElement_Loaded;
                        }
                        else
                        {
                            mask = maskedControl.GetAlphaMask();
                        }
                    }
                    else if (context.Element is Image)
                    {
                        mask = ((Image)context.Element).GetAlphaMask();
                    }
                    else if (context.Element is Shape)
                    {
                        mask = ((Shape)context.Element).GetAlphaMask();
                    }
                    else if (context.Element is TextBlock)
                    {
                        mask = ((TextBlock)context.Element).GetAlphaMask();
                    }
                }

                // If we don't have a mask and have specified rounded corners, we'll generate a simple quick mask.
                // This is the same code from link:AttachedCardShadow.cs:GetShadowMask
                if (mask == null && SupportsCompositionVisualSurface && CornerRadius > 0)
                {
                    // Create rounded rectangle geometry and add it to a shape
                    var geometry = context.GetResource(RoundedRectangleGeometryResourceKey) ?? context.AddResource(
                        RoundedRectangleGeometryResourceKey,
                        context.Compositor.CreateRoundedRectangleGeometry());
                    geometry.CornerRadius = new Vector2((float)CornerRadius);

                    var shape = context.GetResource(ShapeResourceKey) ?? context.AddResource(ShapeResourceKey, context.Compositor.CreateSpriteShape(geometry));
                    shape.FillBrush = context.Compositor.CreateColorBrush(Colors.Black);

                    // Create a ShapeVisual so that our geometry can be rendered to a visual
                    var shapeVisual = context.GetResource(ShapeVisualResourceKey) ??
                                      context.AddResource(ShapeVisualResourceKey, context.Compositor.CreateShapeVisual());
                    shapeVisual.Shapes.Add(shape);

                    // Create a CompositionVisualSurface, which renders our ShapeVisual to a texture
                    var visualSurface = context.GetResource(VisualSurfaceResourceKey) ??
                                        context.AddResource(VisualSurfaceResourceKey, context.Compositor.CreateVisualSurface());
                    visualSurface.SourceVisual = shapeVisual;

                    // Create a CompositionSurfaceBrush to render our CompositionVisualSurface to a brush.
                    // Now we have a rounded rectangle brush that can be used on as the mask for our shadow.
                    var surfaceBrush = context.GetResource(SurfaceBrushResourceKey) ?? context.AddResource(
                        SurfaceBrushResourceKey,
                        context.Compositor.CreateSurfaceBrush(visualSurface));

                    geometry.Size = visualSurface.SourceSize = shapeVisual.Size = context.Element.RenderSize.ToVector2();

                    mask = surfaceBrush;
                }
            }

            // Position our shadow in the correct spot to match the corresponding element.
            context.SpriteVisual.Offset = context.Element.CoordinatesFrom(CastTo).ToVector3();

            BindSizeAndScale(context.SpriteVisual, context.Element);

            return mask;
        }

        private static void BindSizeAndScale(CompositionObject source, UIElement target)
        {
            var visual = ElementCompositionPreview.GetElementVisual(target);
            var bindSizeAnimation = source.Compositor.CreateExpressionAnimation($"{nameof(visual)}.Size * {nameof(visual)}.Scale.XY");

            bindSizeAnimation.SetReferenceParameter(nameof(visual), visual);

            // Start the animation
            source.StartAnimation("Size", bindSizeAnimation);
        }

        private void CustomMaskedElement_Loaded(object sender, RoutedEventArgs e)
        {
            var context = GetElementContext(sender as FrameworkElement);

            context.Element.Loaded -= CustomMaskedElement_Loaded;

            UpdateShadowClip(context);
            UpdateShadowMask(context);
        }

        /// <inheritdoc/>
        protected internal override void OnSizeChanged(AttachedShadowElementContext context, Size newSize, Size previousSize)
        {
            context.SpriteVisual.Offset = context.Element.CoordinatesFrom(CastTo).ToVector3();

            UpdateShadowClip(context);

            base.OnSizeChanged(context, newSize, previousSize);
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged(AttachedShadowElementContext context, DependencyProperty property, object oldValue, object newValue)
        {
            if (property == IsMaskedProperty)
            {
                UpdateShadowMask(context);
            }
            else if (property == CornerRadiusProperty)
            {
                var geometry = context.GetResource(RoundedRectangleGeometryResourceKey);
                if (geometry != null)
                {
                    geometry.CornerRadius = new Vector2((float)(double)newValue);
                }

                UpdateShadowMask(context);
            }
            else
            {
                base.OnPropertyChanged(context, property, oldValue, newValue);
            }
        }
    }
}
