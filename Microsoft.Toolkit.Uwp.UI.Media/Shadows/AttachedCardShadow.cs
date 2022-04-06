// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// A performant rectangular <see cref="DropShadow"/> which can be attached to any <see cref="FrameworkElement"/>. It uses Win2D to create a clipped area of the outline of the element such that transparent elements don't see the shadow below them, and the shadow can be attached without having to project to another surface. It is animatable, can be shared via a resource, and used in a <see cref="Style"/>.
    /// </summary>
    /// <remarks>
    /// This shadow will not work on <see cref="FrameworkElement"/> which is directly clipping to its bounds (e.g. a <see cref="Windows.UI.Xaml.Controls.Border"/> using a <see cref="Windows.UI.Xaml.Controls.Control.CornerRadius"/>). An extra <see cref="Windows.UI.Xaml.Controls.Border"/> can instead be applied around the clipped border with the Shadow to create the desired effect. Most existing controls due to how they're templated will not encounter this behavior or require this workaround.
    /// </remarks>
    public sealed class AttachedCardShadow : AttachedShadowBase
    {
        private const float MaxBlurRadius = 72;

        private static readonly TypedResourceKey<CompositionGeometricClip> ClipResourceKey = "Clip";
        private static readonly TypedResourceKey<CompositionPathGeometry> PathGeometryResourceKey = "PathGeometry";
        private static readonly TypedResourceKey<CompositionMaskBrush> OpacityMaskBrushResourceKey = "OpacityMask";
        private static readonly TypedResourceKey<ShapeVisual> OpacityMaskShapeVisualResourceKey = "OpacityMaskShapeVisual";
        private static readonly TypedResourceKey<CompositionRoundedRectangleGeometry> OpacityMaskGeometryResourceKey = "OpacityMaskGeometry";
        private static readonly TypedResourceKey<CompositionSpriteShape> OpacityMaskSpriteShapeResourceKey = "OpacityMaskSpriteShape";
        private static readonly TypedResourceKey<CompositionVisualSurface> OpacityMaskShapeVisualSurfaceResourceKey = "OpacityMaskShapeVisualSurface";
        private static readonly TypedResourceKey<CompositionSurfaceBrush> OpacityMaskShapeVisualSurfaceBrushResourceKey = "OpacityMaskShapeVisualSurfaceBrush";
        private static readonly TypedResourceKey<CompositionVisualSurface> OpacityMaskVisualSurfaceResourceKey = "OpacityMaskVisualSurface";
        private static readonly TypedResourceKey<CompositionSurfaceBrush> OpacityMaskSurfaceBrushResourceKey = "OpacityMaskSurfaceBrush";
        private static readonly TypedResourceKey<SpriteVisual> OpacityMaskVisualResourceKey = "OpacityMaskVisual";
        private static readonly TypedResourceKey<CompositionRoundedRectangleGeometry> RoundedRectangleGeometryResourceKey = "RoundedGeometry";
        private static readonly TypedResourceKey<CompositionSpriteShape> ShapeResourceKey = "Shape";
        private static readonly TypedResourceKey<ShapeVisual> ShapeVisualResourceKey = "ShapeVisual";
        private static readonly TypedResourceKey<CompositionSurfaceBrush> SurfaceBrushResourceKey = "SurfaceBrush";
        private static readonly TypedResourceKey<CompositionVisualSurface> VisualSurfaceResourceKey = "VisualSurface";

        /// <summary>
        /// The <see cref="DependencyProperty"/> for <see cref="CornerRadius"/>
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(double),
                typeof(AttachedCardShadow),
                new PropertyMetadata(4d, OnDependencyPropertyChanged)); // Default WinUI ControlCornerRadius is 4

        /// <summary>
        /// The <see cref="DependencyProperty"/> for <see cref="InnerContentClipMode"/>.
        /// </summary>
        public static readonly DependencyProperty InnerContentClipModeProperty =
            DependencyProperty.Register(
                nameof(InnerContentClipMode),
                typeof(InnerContentClipMode),
                typeof(AttachedCardShadow),
                new PropertyMetadata(InnerContentClipMode.CompositionGeometricClip, OnDependencyPropertyChanged));

        /// <summary>
        /// Gets or sets the roundness of the shadow's corners.
        /// </summary>
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Gets or sets the mode use to clip inner content from the shadow.
        /// </summary>
        public InnerContentClipMode InnerContentClipMode
        {
            get => (InnerContentClipMode)GetValue(InnerContentClipModeProperty);
            set => SetValue(InnerContentClipModeProperty, value);
        }

        /// <inheritdoc/>
        public override bool IsSupported => SupportsCompositionVisualSurface;

        /// <inheritdoc/>
        protected internal override bool SupportsOnSizeChangedEvent => true;

        /// <inheritdoc/>
        protected internal override void OnElementContextInitialized(AttachedShadowElementContext context)
        {
            UpdateVisualOpacityMask(context);
            base.OnElementContextInitialized(context);
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged(AttachedShadowElementContext context, DependencyProperty property, object oldValue, object newValue)
        {
            if (property == CornerRadiusProperty)
            {
                UpdateShadowClip(context);
                UpdateVisualOpacityMask(context);

                var geometry = context.GetResource(RoundedRectangleGeometryResourceKey);
                if (geometry != null)
                {
                    geometry.CornerRadius = new Vector2((float)(double)newValue);
                }
            }
            else if (property == InnerContentClipModeProperty)
            {
                UpdateShadowClip(context);
                UpdateVisualOpacityMask(context);
                SetElementChildVisual(context);
            }
            else
            {
                base.OnPropertyChanged(context, property, oldValue, newValue);
            }

            base.OnPropertyChanged(context, property, oldValue, newValue);
        }

        /// <inheritdoc/>
        protected override CompositionBrush GetShadowMask(AttachedShadowElementContext context)
        {
            if (!SupportsCompositionVisualSurface)
            {
                return null;
            }

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

            return surfaceBrush;
        }

        /// <inheritdoc/>
        protected override CompositionClip GetShadowClip(AttachedShadowElementContext context)
        {
            if (InnerContentClipMode != InnerContentClipMode.CompositionGeometricClip)
            {
                context.RemoveAndDisposeResource(PathGeometryResourceKey);
                context.RemoveAndDisposeResource(ClipResourceKey);
                return null;
            }

            // The way this shadow works without the need to project on another element is because
            // we're clipping the inner part of the shadow which would be cast on the element
            // itself away. This method is creating an outline so that we are only showing the
            // parts of the shadow that are outside the element's context.
            // Note: This does cause an issue if the element does clip itself to its bounds, as then
            // the shadowed area is clipped as well.
            var pathGeom = context.GetResource(PathGeometryResourceKey) ??
                           context.AddResource(PathGeometryResourceKey, context.Compositor.CreatePathGeometry());
            var clip = context.GetResource(ClipResourceKey) ?? context.AddResource(ClipResourceKey, context.Compositor.CreateGeometricClip(pathGeom));

            // Create rounded rectangle geometry at a larger size that compensates for the size of the stroke,
            // as we want the inside edge of the stroke to match the edges of the element.
            // Additionally, the inside edge of the stroke will have a smaller radius than the radius we specified.
            // Using "(StrokeThickness / 2) + Radius" as our rectangle's radius will give us an inside stroke radius that matches the radius we want.
            var canvasRectangle = CanvasGeometry.CreateRoundedRectangle(
                null,
                -MaxBlurRadius / 2,
                -MaxBlurRadius / 2,
                (float)context.Element.ActualWidth + MaxBlurRadius,
                (float)context.Element.ActualHeight + MaxBlurRadius,
                (MaxBlurRadius / 2) + (float)CornerRadius,
                (MaxBlurRadius / 2) + (float)CornerRadius);

            var canvasStroke = canvasRectangle.Stroke(MaxBlurRadius);

            pathGeom.Path = new CompositionPath(canvasStroke);

            return clip;
        }

        /// <summary>
        /// Updates the <see cref="CompositionBrush"/> used to mask <paramref name="context"/>.<see cref="AttachedShadowElementContext.SpriteVisual">SpriteVisual</see>.
        /// </summary>
        /// <param name="context">The <see cref="AttachedShadowElementContext"/> whose <see cref="SpriteVisual"/> will be masked.</param>
        private void UpdateVisualOpacityMask(AttachedShadowElementContext context)
        {
            if (InnerContentClipMode != InnerContentClipMode.CompositionMaskBrush)
            {
                context.RemoveAndDisposeResource(OpacityMaskShapeVisualResourceKey);
                context.RemoveAndDisposeResource(OpacityMaskGeometryResourceKey);
                context.RemoveAndDisposeResource(OpacityMaskSpriteShapeResourceKey);
                context.RemoveAndDisposeResource(OpacityMaskShapeVisualSurfaceResourceKey);
                context.RemoveAndDisposeResource(OpacityMaskShapeVisualSurfaceBrushResourceKey);
                return;
            }

            // Create a rounded rectangle Visual with a thick outline and no fill, then use a VisualSurface of it as an opacity mask for the shadow.
            // This will have the effect of clipping the inner content of the shadow, so that the casting element is not covered by the shadow,
            // while the shadow is still rendered outside of the element. Similar to what takes place in GetVisualClip,
            // except here we use a brush to mask content instead of a pure geometric clip.
            var shapeVisual = context.GetResource(OpacityMaskShapeVisualResourceKey) ??
                context.AddResource(OpacityMaskShapeVisualResourceKey, context.Compositor.CreateShapeVisual());

            CompositionRoundedRectangleGeometry geom = context.GetResource(OpacityMaskGeometryResourceKey) ??
                context.AddResource(OpacityMaskGeometryResourceKey, context.Compositor.CreateRoundedRectangleGeometry());
            CompositionSpriteShape shape = context.GetResource(OpacityMaskSpriteShapeResourceKey) ??
                context.AddResource(OpacityMaskSpriteShapeResourceKey, context.Compositor.CreateSpriteShape(geom));

            geom.Offset = new Vector2(MaxBlurRadius / 2);
            geom.CornerRadius = new Vector2((MaxBlurRadius / 2) + (float)CornerRadius);
            shape.StrokeThickness = MaxBlurRadius;
            shape.StrokeBrush = shape.StrokeBrush ?? context.Compositor.CreateColorBrush(Colors.Black);

            if (!shapeVisual.Shapes.Contains(shape))
            {
                shapeVisual.Shapes.Add(shape);
            }

            var visualSurface = context.GetResource(OpacityMaskShapeVisualSurfaceResourceKey) ??
                context.AddResource(OpacityMaskShapeVisualSurfaceResourceKey, context.Compositor.CreateVisualSurface());
            visualSurface.SourceVisual = shapeVisual;

            geom.Size = new Vector2((float)context.Element.ActualWidth, (float)context.Element.ActualHeight) + new Vector2(MaxBlurRadius);
            shapeVisual.Size = visualSurface.SourceSize = new Vector2((float)context.Element.ActualWidth, (float)context.Element.ActualHeight) + new Vector2(MaxBlurRadius * 2);

            var surfaceBrush = context.GetResource(OpacityMaskShapeVisualSurfaceBrushResourceKey) ??
                context.AddResource(OpacityMaskShapeVisualSurfaceBrushResourceKey, context.Compositor.CreateSurfaceBrush());
            surfaceBrush.Surface = visualSurface;
        }

        /// <inheritdoc/>
        protected override void SetElementChildVisual(AttachedShadowElementContext context)
        {
            if (context.TryGetResource(OpacityMaskShapeVisualSurfaceBrushResourceKey, out var opacityMask))
            {
                var visualSurface = context.GetResource(OpacityMaskVisualSurfaceResourceKey) ??
                    context.AddResource(OpacityMaskVisualSurfaceResourceKey, context.Compositor.CreateVisualSurface());
                visualSurface.SourceVisual = context.SpriteVisual;
                context.SpriteVisual.RelativeSizeAdjustment = Vector2.Zero;
                context.SpriteVisual.Size = new Vector2((float)context.Element.ActualWidth, (float)context.Element.ActualHeight);
                visualSurface.SourceOffset = new Vector2(-MaxBlurRadius);
                visualSurface.SourceSize = new Vector2((float)context.Element.ActualWidth, (float)context.Element.ActualHeight) + new Vector2(MaxBlurRadius * 2);

                var surfaceBrush = context.GetResource(OpacityMaskSurfaceBrushResourceKey) ??
                    context.AddResource(OpacityMaskSurfaceBrushResourceKey, context.Compositor.CreateSurfaceBrush());
                surfaceBrush.Surface = visualSurface;
                surfaceBrush.Stretch = CompositionStretch.None;

                CompositionMaskBrush maskBrush = context.GetResource(OpacityMaskBrushResourceKey) ??
                    context.AddResource(OpacityMaskBrushResourceKey, context.Compositor.CreateMaskBrush());
                maskBrush.Source = surfaceBrush;
                maskBrush.Mask = opacityMask;

                var visual = context.GetResource(OpacityMaskVisualResourceKey) ??
                    context.AddResource(OpacityMaskVisualResourceKey, context.Compositor.CreateSpriteVisual());
                visual.RelativeSizeAdjustment = Vector2.One;
                visual.Offset = new Vector3(-MaxBlurRadius, -MaxBlurRadius, 0);
                visual.Size = new Vector2(MaxBlurRadius * 2);
                visual.Brush = maskBrush;
                ElementCompositionPreview.SetElementChildVisual(context.Element, visual);
            }
            else
            {
                base.SetElementChildVisual(context);
                context.RemoveAndDisposeResource(OpacityMaskVisualSurfaceResourceKey);
                context.RemoveAndDisposeResource(OpacityMaskSurfaceBrushResourceKey);
                context.RemoveAndDisposeResource(OpacityMaskVisualResourceKey);
                context.RemoveAndDisposeResource(OpacityMaskBrushResourceKey);
            }
        }

        /// <inheritdoc />
        protected internal override void OnSizeChanged(AttachedShadowElementContext context, Size newSize, Size previousSize)
        {
            var sizeAsVec2 = newSize.ToVector2();

            var geometry = context.GetResource(RoundedRectangleGeometryResourceKey);
            if (geometry != null)
            {
                geometry.Size = sizeAsVec2;
            }

            var visualSurface = context.GetResource(VisualSurfaceResourceKey);
            if (geometry != null)
            {
                visualSurface.SourceSize = sizeAsVec2;
            }

            var shapeVisual = context.GetResource(ShapeVisualResourceKey);
            if (geometry != null)
            {
                shapeVisual.Size = sizeAsVec2;
            }

            UpdateShadowClip(context);

            base.OnSizeChanged(context, newSize, previousSize);
        }
    }
}