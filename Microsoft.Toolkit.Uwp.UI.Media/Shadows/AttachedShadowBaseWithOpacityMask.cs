using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Media.Shadows
{
    /// <summary>
    /// A base class for attached shadows that use an opacity mask to clip content from the shadow.
    /// </summary>
    public abstract class AttachedShadowBaseWithOpacityMask : AttachedShadowBase
    {
        private const string AlphaMaskSourceKey = "AttachedShadowAlphaMask";
        private const string SpriteVisualSourceKey = "AttachedShadowSpriteVisual";

        private static readonly TypedResourceKey<CompositionEffectBrush> OpacityMaskEffectBrushResourceKey = "AttachedShadowSpriteVisualEffectBrush";
        private static readonly TypedResourceKey<CompositionBrush> OpacityMaskResourceKey = "AttachedShadowSpriteVisualOpacityMask";
        private static readonly TypedResourceKey<SpriteVisual> OpacityMaskVisualResourceKey = "AttachedShadowSpriteVisualOpacityMaskVisual";
        private static readonly TypedResourceKey<CompositionVisualSurface> OpacityMaskVisualSurfaceResourceKey = "AttachedShadowSpriteVisualOpacityMaskSurface";
        private static readonly TypedResourceKey<CompositionSurfaceBrush> OpacityMaskSurfaceBrushResourceKey =
            "AttachedShadowSpriteVisualOpacityMaskSurfaceBrush";
        private static readonly TypedResourceKey<AlphaMaskEffect> AlphaMaskEffectResourceKey = "AttachedShadowSpriteVisualAlphaMaskEffect";

        /// <summary>
        /// Update the opacity mask for the shadow's <see cref="SpriteVisual"/>.
        /// </summary>
        /// <param name="context">The <see cref="AttachedShadowElementContext"/> this operation will be performed on.</param>
        protected void UpdateVisualOpacityMask(AttachedShadowElementContext context)
        {
            if (!context.IsInitialized)
            {
                return;
            }

            var brush = GetisualOpacityMask(context);
            if (brush != null)
            {
                context.AddResource(OpacityMaskResourceKey, brush);
            }
            else
            {
                context.RemoveResource(OpacityMaskResourceKey)?.Dispose();
            }
        }

        /// <summary>
        /// Override and return a <see cref="CompositionBrush"/> that serves as an opacity mask for the shadow's <see cref="SpriteVisual"/>
        /// </summary>
        protected abstract CompositionBrush GetisualOpacityMask(AttachedShadowElementContext context);

        protected override void OnElementContextInitialized(AttachedShadowElementContext context)
        {
            UpdateVisualOpacityMask(context);
            base.OnElementContextInitialized(context);
        }

        protected override void OnElementContextUninitialized(AttachedShadowElementContext context)
        {
            context.RemoveAndDisposeResource(OpacityMaskResourceKey);
            context.RemoveAndDisposeResource(OpacityMaskVisualResourceKey);
            context.RemoveAndDisposeResource(OpacityMaskVisualSurfaceResourceKey);
            context.RemoveAndDisposeResource(OpacityMaskSurfaceBrushResourceKey);
            context.RemoveAndDisposeResource(OpacityMaskEffectBrushResourceKey);
            context.RemoveAndDisposeResource(AlphaMaskEffectResourceKey);
            base.OnElementContextUninitialized(context);
        }

        protected override void SetElementChildVisual(AttachedShadowElementContext context)
        {
            if (context.TryGetResource(OpacityMaskResourceKey, out var opacityMask))
            {
                var visualSurface = context.GetResource(OpacityMaskVisualSurfaceResourceKey) ?? context.AddResource(
                    OpacityMaskVisualSurfaceResourceKey,
                    context.Compositor.CreateVisualSurface());
                visualSurface.SourceVisual = context.SpriteVisual;
                visualSurface.StartAnimation(nameof(visualSurface.SourceSize), context.Compositor.CreateExpressionAnimation("this.SourceVisual.Size"));

                var surfaceBrush = context.GetResource(OpacityMaskSurfaceBrushResourceKey) ?? context.AddResource(
                    OpacityMaskSurfaceBrushResourceKey,
                    context.Compositor.CreateSurfaceBrush(visualSurface));
                var alphaMask = context.GetResource(AlphaMaskEffectResourceKey) ?? context.AddResource(AlphaMaskEffectResourceKey, new AlphaMaskEffect());
                alphaMask.Source = new CompositionEffectSourceParameter(SpriteVisualSourceKey);
                alphaMask.AlphaMask = new CompositionEffectSourceParameter(AlphaMaskSourceKey);

                using (var factory = context.Compositor.CreateEffectFactory(alphaMask))
                {
                    context.RemoveResource(OpacityMaskEffectBrushResourceKey)?.Dispose();
                    var brush = context.AddResource(OpacityMaskEffectBrushResourceKey, factory.CreateBrush());
                    brush.SetSourceParameter(SpriteVisualSourceKey, surfaceBrush);
                    brush.SetSourceParameter(AlphaMaskSourceKey, opacityMask);

                    var visual = context.GetResource(OpacityMaskVisualResourceKey) ?? context.AddResource(
                        OpacityMaskVisualResourceKey,
                        context.Compositor.CreateSpriteVisual());
                    visual.RelativeSizeAdjustment = Vector2.One;
                    visual.Brush = brush;
                    ElementCompositionPreview.SetElementChildVisual(context.Element, visual);
                }
            }
            else
            {
                base.SetElementChildVisual(context);
            }
        }
    }
}
