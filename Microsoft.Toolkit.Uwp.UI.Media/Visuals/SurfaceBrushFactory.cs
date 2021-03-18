// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// A builder type for a <see cref="CompositionSurfaceBrush"/> to apply to the visual of UI elements.
    /// </summary>
    [ContentProperty(Name = nameof(Effects))]
    public sealed class SurfaceBrushFactory : PipelineVisualFactoryBase, IPipelineEffect
    {
        private CompositionSurfaceBrush _brush;

        /// <inheritdoc/>
        public CompositionBrush Brush => _brush;

        private Uri _uri;

        /// <summary>
        /// Gets or sets the source of an image to use to start the Pipeline.
        /// </summary>
        public Uri Source
        {
            get
            {
                return _uri;
            }

            set
            {
                _uri = value;

                if (_uri != null)
                {
                    //// TODO: I think we want a few different options, like make this a provider and have another one to take the Win2D Path somehow maybe?
                    _brush = Window.Current.Compositor.CreateSurfaceBrush(LoadedImageSurface.StartLoadFromUri(_uri));
                }
                else
                {
                    _brush = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="CompositionSurfaceBrush.Stretch"/> property of the underlying <see cref="CompositionSurfaceBrush"/>.
        /// </summary>
        public CompositionStretch Stretch
        {
            //// TODO: Need to expose the other properties.
            get => _brush.Stretch;
            set => _brush.Stretch = value;
        }

        /// <summary>
        /// Gets or sets the collection of effects to use in the current pipeline.
        /// </summary>
        public IList<PipelineEffect> Effects
        {
            get
            {
                if (GetValue(EffectsProperty) is not IList<PipelineEffect> effects)
                {
                    effects = new List<PipelineEffect>();

                    SetValue(EffectsProperty, effects);
                }

                return effects;
            }
            set => SetValue(EffectsProperty, value);
        }

        /// <summary>
        /// Identifies the <seealso cref="Effects"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EffectsProperty = DependencyProperty.Register(
            nameof(Effects),
            typeof(IList<PipelineEffect>),
            typeof(PipelineVisualFactory),
            new PropertyMetadata(null));

        /// <inheritdoc/>
        public override async ValueTask<Visual> GetAttachedVisualAsync(UIElement element)
        {
            var visual = (SpriteVisual)await base.GetAttachedVisualAsync(element);

            foreach (IPipelineEffect effect in Effects)
            {
                effect.NotifyCompositionBrushInUse(visual.Brush);
            }

            return visual;
        }

        /// <inheritdoc/>
        protected override PipelineBuilder OnPipelineRequested()
        {
            PipelineBuilder builder = PipelineBuilder.FromBrush(Brush);

            foreach (IPipelineEffect effect in Effects)
            {
                builder = effect.AppendToBuilder(builder);
            }

            return builder;
        }

        /// <inheritdoc/>
        public PipelineBuilder AppendToBuilder(PipelineBuilder builder)
        {
            // Not used as we're exploting IPipelineEffect to reuse animations, but not actually be called as part of a pipeline...
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void NotifyCompositionBrushInUse(CompositionBrush brush)
        {
            // Not used as we're exploting IPipelineEffect to reuse animations, but not actually be called as part of a pipeline...
            throw new System.NotImplementedException();
        }
    }
}
