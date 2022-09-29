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

            // Create ShapeVisual, and CompositionSpriteShape with geometry, these will provide the visuals for the opacity mask.
            ShapeVisual shapeVisual = context.GetResource(OpacityMaskShapeVisualResourceKey) ??
                context.AddResource(OpacityMaskShapeVisualResourceKey, context.Compositor.CreateShapeVisual());

            CompositionRoundedRectangleGeometry geometry = context.GetResource(OpacityMaskGeometryResourceKey) ??
                context.AddResource(OpacityMaskGeometryResourceKey, context.Compositor.CreateRoundedRectangleGeometry());
            CompositionSpriteShape shape = context.GetResource(OpacityMaskSpriteShapeResourceKey) ??
                context.AddResource(OpacityMaskSpriteShapeResourceKey, context.Compositor.CreateSpriteShape(geometry));

            // Set the attributes of the geometry, and add the CompositionSpriteShape to the ShapeVisual.
            // The geometry will have a thick outline and no fill, meaning that when used as a mask,
            // the shadow will only be rendered on the outer area covered by the outline, clipping out its inner portion.
            geometry.Offset = new Vector2(MaxBlurRadius / 2);
            geometry.CornerRadius = new Vector2((MaxBlurRadius / 2) + (float)CornerRadius);
            shape.StrokeThickness = MaxBlurRadius;
            shape.StrokeBrush = shape.StrokeBrush ?? context.Compositor.CreateColorBrush(Colors.Black);

            if (!shapeVisual.Shapes.Contains(shape))
            {
                shapeVisual.Shapes.Add(shape);
            }

            // Create CompositionVisualSurface using the ShapeVisual as the source visual.
            CompositionVisualSurface visualSurface = context.GetResource(OpacityMaskShapeVisualSurfaceResourceKey) ??
                context.AddResource(OpacityMaskShapeVisualSurfaceResourceKey, context.Compositor.CreateVisualSurface());
            visualSurface.SourceVisual = shapeVisual;

            geometry.Size = new Vector2((float)context.Element.ActualWidth, (float)context.Element.ActualHeight) + new Vector2(MaxBlurRadius);
            shapeVisual.Size = visualSurface.SourceSize = new Vector2((float)context.Element.ActualWidth, (float)context.Element.ActualHeight) + new Vector2(MaxBlurRadius * 2);

            // Create a CompositionSurfaceBrush using the CompositionVisualSurface as the source, this essentially converts the ShapeVisual into a brush.
            // This brush can then be used as a mask.
            CompositionSurfaceBrush opacityMask = context.GetResource(OpacityMaskShapeVisualSurfaceBrushResourceKey) ??
                context.AddResource(OpacityMaskShapeVisualSurfaceBrushResourceKey, context.Compositor.CreateSurfaceBrush());
            opacityMask.Surface = visualSurface;
        }

        /// <inheritdoc/>
        protected override void SetElementChildVisual(AttachedShadowElementContext context)
        {
            if (context.TryGetResource(OpacityMaskShapeVisualSurfaceBrushResourceKey, out CompositionSurfaceBrush opacityMask))
            {
                // If the resource for OpacityMaskShapeVisualSurfaceBrushResourceKey exists it means this.InnerContentClipMode == CompositionVisualSurface,
                // which means we need to take some steps to set up an opacity mask.

                // Create a CompositionVisualSurface, and use the SpriteVisual containing the shadow as the source.
                CompositionVisualSurface shadowVisualSurface = context.GetResource(OpacityMaskVisualSurfaceResourceKey) ??
                    context.AddResource(OpacityMaskVisualSurfaceResourceKey, context.Compositor.CreateVisualSurface());
                shadowVisualSurface.SourceVisual = context.SpriteVisual;
                context.SpriteVisual.RelativeSizeAdjustment = Vector2.Zero;
                context.SpriteVisual.Size = new Vector2((float)context.Element.ActualWidth, (float)context.Element.ActualHeight);

                // Adjust the offset and size of the CompositionVisualSurface to accommodate the thick outline of the shape created in UpdateVisualOpacityMask().
                shadowVisualSurface.SourceOffset = new Vector2(-MaxBlurRadius);
                shadowVisualSurface.SourceSize = new Vector2((float)context.Element.ActualWidth, (float)context.Element.ActualHeight) + new Vector2(MaxBlurRadius * 2);

                // Create a CompositionSurfaceBrush from the CompositionVisualSurface. This allows us to render the shadow in a brush.
                CompositionSurfaceBrush shadowSurfaceBrush = context.GetResource(OpacityMaskSurfaceBrushResourceKey) ??
                    context.AddResource(OpacityMaskSurfaceBrushResourceKey, context.Compositor.CreateSurfaceBrush());
                shadowSurfaceBrush.Surface = shadowVisualSurface;
                shadowSurfaceBrush.Stretch = CompositionStretch.None;

                // Create a CompositionMaskBrush, using the CompositionSurfaceBrush of the shadow as the source,
                // and the CompositionSurfaceBrush created in UpdateVisualOpacityMask() as the mask.
                // This creates a brush that renders the shadow with its inner portion clipped out.
                CompositionMaskBrush maskBrush = context.GetResource(OpacityMaskBrushResourceKey) ??
                    context.AddResource(OpacityMaskBrushResourceKey, context.Compositor.CreateMaskBrush());
                maskBrush.Source = shadowSurfaceBrush;
                maskBrush.Mask = opacityMask;

                // Create a SpriteVisual and set its brush to the CompositionMaskBrush created in the previous step,
                // then set it as the child of the element in the context.
                SpriteVisual visual = context.GetResource(OpacityMaskVisualResourceKey) ??
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

                // Reset context.SpriteVisual.Size and RelativeSizeAdjustment to default values
                // as they may be changed in the block above.
                context.SpriteVisual.Size = Vector2.Zero;
                context.SpriteVisual.RelativeSizeAdjustment = Vector2.One;

                context.RemoveAndDisposeResource(OpacityMaskVisualSurfaceResourceKey);
                context.RemoveAndDisposeResource(OpacityMaskSurfaceBrushResourceKey);
                context.RemoveAndDisposeResource(OpacityMaskVisualResourceKey);
                context.RemoveAndDisposeResource(OpacityMaskBrushResourceKey);
            }
        }

        /// <inheritdoc />
        protected internal override void OnSizeChanged(AttachedShadowElementContext context, Size newSize, Size previousSize)
        {
            Vector2 sizeAsVec2 = newSize.ToVector2();

            if (context.TryGetResource(RoundedRectangleGeometryResourceKey, out CompositionRoundedRectangleGeometry geometry))
            {
                geometry.Size = sizeAsVec2;
            }

            if (context.TryGetResource(VisualSurfaceResourceKey, out CompositionVisualSurface visualSurface))
            {
                visualSurface.SourceSize = sizeAsVec2;
            }

            if (context.TryGetResource(ShapeVisualResourceKey, out ShapeVisual shapeVisual))
            {
                shapeVisual.Size = sizeAsVec2;
            }

            if (context.TryGetResource(OpacityMaskVisualSurfaceResourceKey, out CompositionVisualSurface opacityMaskVisualSurface))
            {
                opacityMaskVisualSurface.SourceSize = sizeAsVec2 + new Vector2(MaxBlurRadius * 2);
            }

            if (InnerContentClipMode is InnerContentClipMode.CompositionMaskBrush)
            {
                context.SpriteVisual.Size = sizeAsVec2;
            }

            UpdateShadowClip(context);
            UpdateVisualOpacityMask(context);

            base.OnSizeChanged(context, newSize, previousSize);
        }
    }
}