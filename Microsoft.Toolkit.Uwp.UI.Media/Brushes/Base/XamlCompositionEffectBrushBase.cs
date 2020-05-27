// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Media.Extensions;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;
using Windows.UI.Composition;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// A custom <see cref="XamlCompositionBrushBase"/> <see langword="class"/> that's ready to be used with a custom <see cref="PipelineBuilder"/> pipeline
    /// </summary>
    public abstract class XamlCompositionEffectBrushBase : XamlCompositionBrushBase
    {
        /// <summary>
        /// The initialization <see cref="AsyncMutex"/> instance
        /// </summary>
        private readonly AsyncMutex connectedMutex = new AsyncMutex();

        /// <summary>
        /// A method that builds and returns the <see cref="PipelineBuilder"/> pipeline to use in the current instance.<para/>
        /// This method can also be used to store any needed <see cref="EffectSetter{T}"/> or <see cref="EffectAnimation{T}"/> instances in local fields, for later use (they will need to be called upon <see cref="XamlCompositionBrushBase.CompositionBrush"/>).
        /// </summary>
        /// <returns>A <see cref="PipelineBuilder"/> instance to create the brush to display</returns>
        protected abstract PipelineBuilder OnBrushRequested();

        private bool _isEnabled = true;

        /// <summary>
        /// Gets or sets a value indicating whether the current brush is using the provided pipeline, or the fallback color
        /// </summary>
        public bool IsEnabled
        {
            get => this._isEnabled;
            set => this.OnEnabledToggled(value);
        }

        /// <inheritdoc/>
        protected override async void OnConnected()
        {
            using (await this.connectedMutex.LockAsync())
            {
                if (this.CompositionBrush == null)
                {
                    // Abort if effects aren't supported
                    if (!CompositionCapabilities.GetForCurrentView().AreEffectsSupported())
                    {
                        return;
                    }

                    if (this._isEnabled)
                    {
                        this.CompositionBrush = await this.OnBrushRequested().BuildAsync();
                    }
                    else
                    {
                        this.CompositionBrush = await PipelineBuilder.FromColor(this.FallbackColor).BuildAsync();
                    }
                }
            }

            base.OnConnected();
        }

        /// <inheritdoc/>
        protected override async void OnDisconnected()
        {
            using (await this.connectedMutex.LockAsync())
            {
                if (this.CompositionBrush != null)
                {
                    this.CompositionBrush.Dispose();
                    this.CompositionBrush = null;
                }
            }

            base.OnDisconnected();
        }

        /// <summary>
        /// Updates the <see cref="XamlCompositionBrushBase.CompositionBrush"/> property depending on the input value
        /// </summary>
        /// <param name="value">The new value being set to the <see cref="IsEnabled"/> property</param>
        protected async void OnEnabledToggled(bool value)
        {
            using (await this.connectedMutex.LockAsync())
            {
                if (this._isEnabled == value)
                {
                    return;
                }

                this._isEnabled = value;

                if (this.CompositionBrush != null)
                {
                    // Abort if effects aren't supported
                    if (!CompositionCapabilities.GetForCurrentView().AreEffectsSupported())
                    {
                        return;
                    }

                    if (this._isEnabled)
                    {
                        this.CompositionBrush = await this.OnBrushRequested().BuildAsync();
                    }
                    else
                    {
                        this.CompositionBrush = await PipelineBuilder.FromColor(this.FallbackColor).BuildAsync();
                    }
                }
            }
        }
    }
}
